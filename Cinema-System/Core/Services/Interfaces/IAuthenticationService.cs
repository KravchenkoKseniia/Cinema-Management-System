using Cinema_System.DTOs;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using System;

namespace Cinema_System.Services.Interfaces
{
    public interface IAuthenticationService
    {
        string Register(string userName, string password, string role);
        string Login(string userName, string password);
        bool IsAdmin(int userId);
    }
}
