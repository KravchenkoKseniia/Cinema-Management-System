# Test-Generation Strategy (Cinema Management System)

> **Purpose.** This file is the prompt/spec fed to an AI agent to generate unit tests for this
> solution **one-shot**. It exists because the raw generation pass produced tests that were mechanically
> correct but lacked judgment: they enshrined bugs as spec, used loose assertions, missed structural cases,
> and drifted into a different style per file. Every rule below is the fix for a concrete finding in 
> that reflection.
>
> **How to use.** Paste this whole file as the system/context prompt, then ask the agent to cover
> a named class (e.g. "generate tests for `MovieService` following the strategy"). If the output
> violates any rule, add the missing rule here and regenerate — do not hand-patch the tests. The
> goal is that a clean generation obeys all of this without follow-up.

---

## 0. Tooling & project facts (non-negotiable)

- **Framework:** xUnit (v3, Microsoft Testing Platform runner — the test project already uses it).
- **Mocking:** Moq. **Assertions:** FluentAssertions (`result.Should()...`).
  - ⚠️ `FluentAssertions` is **not yet referenced** in `CinemaSystem.Test.csproj` — add it, and pin
    to **7.x** (8.0+ requires a paid commercial licence). If that is unacceptable, use xUnit `Assert`
    consistently instead — but pick one and use it in every file.
- **Validators:** use **`FluentValidation.TestHelper`** (`TestValidate()` /
  `ShouldHaveValidationErrorFor()` / `WithErrorMessage()`), never hand-rolled
  `Assert.Contains(result.Errors, ...)`.
- **Style:** file-scoped namespaces; rely on the project's global `using Xunit;` (do **not** repeat
  `using Xunit;` per file); no unused usings.
- Target the real types: services depend on `IUnitOfWork` exposing `IGenericRepository<TEntity>`
  per entity (`Movies`, `Sessions`, `Halls`, `Tickets`, …) plus a single `Save()`. Reuse the
  existing `MapperFixture` (real `MappingProfile`) rather than reconfiguring AutoMapper per file.

---

## 1. Naming & structure

- **One test name convention, everywhere:** `MethodName_StateUnderTest_ExpectedBehavior`
  (e.g. `CreateSession_MovieDoesNotExist_PersistsNothing`). Do not mix in
  `Should_Have_Error_When_...` or `Name_ShouldFail_When...` — the raw pass used three conventions.
- **AAA**, with a blank line between Arrange / Act / Assert.
- **One reason to fail per test.** Never stack two arrange-act-assert cycles inside one `[Fact]`
  (the raw pass did this in `..._Empty_Or_Invalid` and `Capacity_..._ZeroOrNegative`). Split them,
  or use `[Theory]`.
- Use **`[Theory]` + `[InlineData]`** for equivalence partitions (`""`, `"   "`, `null`; `0`, `-5`).
- Use a **TestBuilder**: one known-valid factory (e.g. `CreateValidSessionDTO()`), with exactly one
  focused mutation per test on top of it.
- **Expected values are explicit literals**, not read back from the input object
  (`.Should().Be("Inception")`, not `.Should().Be(dto.MovieTitle)`).

---

## 2. The definition of "unit" — one isolation strategy per layer

The raw pass silently used four different isolation strategies for the same layer. Fix that:

| Layer under test | Strategy (mandatory) |
|---|---|
| **Services that use `IUnitOfWork`** (`SessionService`, `MovieService`, `HallService`, `TicketService`) | Mock `IUnitOfWork` + each `IGenericRepository<T>` it returns. Use the **real** AutoMapper via `MapperFixture` (mapping is config, not logic) — but do this *consistently* and be aware a broken profile fails these tests. |
| **Services that take `ApplicationDbContext` directly** (`GenreService`, `PaymentService`, `RoleService`) | You **cannot** mock `IUnitOfWork` here. Use **EF Core InMemory** as a documented *fake*, a fresh **GUID-named database per test**, and mark the class `[Trait("Category","Integration")]`. State in a comment that this is a narrow integration test, not a pure unit — do not pretend otherwise. |
| **API & UI controllers** | Mock **only the service interface** (`ISessionService`, …). **Never** instantiate a real service inside a controller test (the raw pass built a real `HallService` inside `HallsControllerTests`). |
| **Validators** | No mocks; `FluentValidation.TestHelper` + a fixed clock (see §5) for time rules. |

---

## 3. Structural coverage obligations (this lab is graded on branch coverage)

For every method under test, cover:

- **Both sides of every branch**, including the *surprising* one. Example: in
  `SessionsUIController.Index`, the combination *date set + movie name empty* falls into the `else`
  and silently returns **all** sessions — the raw pass skipped exactly this branch. Cover it.
- **Condition coverage of compound booleans.** For `!IsNullOrEmpty(name) && date.HasValue`, exercise
  all meaningful combinations (TT / TF / FT / FF), not just the happy TT.
- **Empty collections / zero-iteration loops.** Test `GetAll…` with an **empty** list, not only with
  2 items.
- **Boundary values *on* the boundary**, not just around it: `Date == DateTime.Today` (not only
  yesterday/tomorrow), minimal valid price `0.01m` (not only `0` and `-10`).
- **Forced-error / negative paths:** `null` body, id-mismatch → `BadRequest`, `null` from `GetById`
  → `NotFound`, invalid `ModelState` (which you must **arrange** — a fresh controller's `ModelState`
  is valid by default, so a test named `..._WithValidModel` that does nothing special is misnamed).
- Fallback branches like `movie?.Title ?? "Unknown"` must each be hit.

---

## 4. Assertion strength — kill realistic mutations

A test that survives an obvious mutation is worthless. Enforce:

- **Assert *what*, not just *that*.** On `Insert`/`Create`, use argument matchers verifying field
  values: `Verify(r => r.Insert(It.Is<Session>(s => s.MovieId == 7 && s.HallId == 3)), Times.Once)`.
  `It.IsAny<Session>()` is not acceptable for the persisted entity.
- **Distinguish specifications.** When a method builds an Ardalis specification, assert the **exact
  specification type/params** passed to `GetListBySpec`, not `It.IsAny<ISpecification<T>>()` —
  otherwise `GetAllSessions`, `GetSessionsByMovie`, `GetSessionsByMovieAndDate` become the same test.
  Prefer `MockBehavior.Strict` for repositories so an unexpected call fails loudly.
- **Exact interaction counts** (`Times.Once`) **plus `VerifyNoOtherCalls()`** where the contract is
  "only this happened".
- **Assert error messages**, not only `IsValid == false` / property name — the raw pass missed the
  validator typo *"End time must be after **end** time"* because messages were never checked.
- **Strong creation assertions.** For `Register`, verify the actual username/email/`RoleId`/password
  passed — not `CreateAsync(It.IsAny<User>(), It.IsAny<string>())`, which a swap of username↔email
  survives.
- **No tautological interaction tests.** A test that merely mirrors the implementation
  (`DeleteSession` calls `Delete` then `Save`) cannot catch a real bug and breaks on any refactor.
  Only write an interaction test when the interaction *is* the contract.

---

## 5. Judgment — question suspicious behavior, never enshrine bugs (most important)

The raw pass's worst outcome was **green tests protecting bugs and security holes**. Default rule:

> When production behavior looks wrong, write the test for the **intended** behavior and mark it
> `Skip = "documents bug #… — see reflection"` (a red/pending test), **or** add an explicit
> `// SMELL:` comment and surface it. **Do not** write a passing test that pins the current behavior
> as if it were the spec.

Known suspects in this codebase (do **not** silently codify these as correct):
- **Silent failure:** `SessionService.CreateSession` does `Console.WriteLine + return` on a missing
  movie/hall instead of throwing/returning a result.
- **Case-sensitive title match** in `CreateSession` (`m.Title == dto.MovieTitle`).
- **Hall-name regex** `^[A-Za-z\s]+$` rejects `"Hall 1"` — almost certainly a bug; a test asserting
  `"Hall 1"` is invalid protects the bug.
- **Stale `DateTime.Today`** captured at validator construction (breaks if the validator is a DI
  singleton) — inject a fixed clock in tests to expose it.
- **Security (never guard with a green test):** the hardcoded admin backdoor
  (`admin` / `passwordPassword123@@@`), username enumeration (`"User not found!"` vs
  `"Invalid credentials!"`), and raw EF entities (incl. Identity `User` with password hash) returned
  from `Users/Genres/Payments` controllers. Flag every one.
  - **Single deliberate exception:** the lab's Phase 4 asks for one *characterization* integration
    test that confirms the backdoor works. That test must be explicitly named and commented as
    "documents vulnerability", and lives only in the integration suite — it is not licence to pin
    security behavior anywhere else.
- **Sync-over-async:** `AuthenticationService` blocks on `.Result`; note it, don't just test it green.

**Value filter.** Don't re-test the framework. A test for a bare `NotEmpty()` / `GreaterThan(0)`
rule only verifies FluentValidation itself — low value; keep at most one. Spend the effort on rules
that encode **our** domain decisions: cross-field `StartTime < EndTime`, the "no past sessions"
boundary, the hall-name regex — that configuration is our code and is where the bugs live.

---

## 6. Allocate effort by risk, not by class size

The raw pass gave a low-risk CRUD screen 16 tests and the **booking flow** (the one real cinema
invariant — *no double-booking a seat*) only 2 happy-path tests. Invert this: the deepest coverage
goes to booking (`TicketService`), payments, and auth; big-but-trivial CRUD gets proportionate,
not maximal, coverage. If a domain invariant is neither implemented nor tested (seat-conflict),
add a pending/`Skip` test that *asks the question*.

---

## Definition of Done (checklist for every generated file)

- [ ] Names follow `Method_State_Expected`; file-scoped namespace; no duplicate `using Xunit;`.
- [ ] One isolation strategy for the layer (§2); DbContext-based services marked Integration.
- [ ] `IUnitOfWork` + `IGenericRepository<T>` mocked for UoW services; controllers mock the service only.
- [ ] Every branch + compound-condition combo covered; empty-collection and on-boundary cases present.
- [ ] Negative paths present: null body, id-mismatch, `null` from `GetById`, invalid (arranged) `ModelState`.
- [ ] Persisted entities asserted field-by-field with `It.Is<T>`; specification type asserted, not `IsAny`.
- [ ] Error **messages** asserted for domain validator rules; framework-only rules not over-tested.
- [ ] No tautological interaction tests.
- [ ] Every known smell/security issue is flagged or `Skip`-documented, **not** pinned green.
- [ ] Test depth reflects risk (booking/auth/payments deepest).
