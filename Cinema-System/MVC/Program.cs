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

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register IAuthenticationService
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>(); 
builder.Services.AddScoped<IHallService, HallService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<ITicketService, TicketService>();

builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();
builder.Environment.EnvironmentName = "Development"; 


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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