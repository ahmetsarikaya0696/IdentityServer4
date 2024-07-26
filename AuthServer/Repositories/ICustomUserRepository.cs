using AuthServer.Models;

namespace AuthServer.Repositories
{
    public interface ICustomUserRepository
    {
        Task<bool> ValidateAsync(string email, string password);
        Task<CustomUser> FindByIdAsync(int id); 
        Task<CustomUser> FindByEmailAsync(string email); 
    }
}
