using Cinema_System.Services;
using Cinema_System.Services.Interfaces;
using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// (Optional) Set the environment name early
builder.Environment.EnvironmentName = "Development";

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the DbContext with a connection string from configuration.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

// Register Identity services BEFORE calling builder.Build()
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Register your custom services.
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>(); 
builder.Services.AddScoped<IHallService, HallService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IUserService, UserService>();

// Register the Unit of Work implementation.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Now build the app after all services have been registered.
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    await SeedRolesAsync(roleManager);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// seeding method
static async Task SeedRolesAsync(RoleManager<Role> roleManager)
{
    // For example:
    if (!await roleManager.RoleExistsAsync("User"))
    {
        await roleManager.CreateAsync(new Role { Name = "User", NormalizedName = "USER" });
    }
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new Role { Name = "Admin", NormalizedName = "ADMIN" });
    }
}

