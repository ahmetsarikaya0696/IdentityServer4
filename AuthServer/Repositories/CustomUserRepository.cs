using AuthServer.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Repositories
{
    public class CustomUserRepository : ICustomUserRepository
    {
        private readonly CustomDbContext _dbContext;

        public CustomUserRepository(CustomDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CustomUser> FindByEmailAsync(string email)
        {
            return await _dbContext.CustomUsers.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<CustomUser> FindByIdAsync(int id)
        {
            return await _dbContext.CustomUsers.FindAsync(id);
        }

        public async Task<bool> ValidateAsync(string email, string password)
        {
            return await _dbContext.CustomUsers.AnyAsync(x => x.Email == email && x.Password == password);
        }
    }
}
