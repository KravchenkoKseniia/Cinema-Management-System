using Infrastructure.Entities;

namespace Cinema_System.Services.Interfaces
{
    public interface IRoleService
    {
        Task<List<Role>> GetRolesAsync();
    }
}