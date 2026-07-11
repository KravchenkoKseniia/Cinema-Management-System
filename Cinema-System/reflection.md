# Step 2a. Reflection on one-shot generated unit tests

## General pros
- AAA structure
- Naming conventions (MethodName_StateUnderTest_ExpectedBehavior), BUT NOT CONSISTENT
- Happy and negative pathes tested
- Fast - 573 ms ms of execution time (195 tests)
- Isolated - dependencies were mocked 
- No "ignored" tests
- Tests are independent - no dependency on execution order and each other 

## General cons
- Tests are just describe, what system do (just to be green as AI likes), not what system should do. So bugs are the part of these green tests
- Mutations will survive (we have 19 `IsAny`)
- No E from BICEP
- No consistent isolation starategy for one layer:

| File | Strategy |
|---|---|
| `SessionServiceTests`, `MovieServiceTests`, `HallServiceTests` | Mocked IUnitOfWork + real AutoMapper |
| `GenreServiceTests`, `PaymentServiceTests` | Real EF Core InMemory database |
| `HallsControllerTests` | Mocks IUnitOfWork + IMapper but instantiates a real `HallService` inside a controller test |
| All other controller tests | Mocked service interface (correct boundary) |

- In `AuthorizationUIController.Login` we have `if (model.UserName == "admin" && model.Password == "passwordPassword123@@@")` and AI wrote test for it
- The depth of the tests correlates with how big the class is, not how dangerous it is. (`MovieUIControllerTests` got 16 tests; the entire booking flow got 2 happy-path tests (`TicketsControllerTests`))