using news_application.Models;
using news_application;
using news_application.Context;
using Microsoft.EntityFrameworkCore;
using news_aggregator.infrastructure.Repositories;
using news_aggregator.application.Interfaces.Repositories;

namespace news_aggregator.application.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly NewsDataContext _context;
        public UserRepository(NewsDataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email) =>
            await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
        
    }
}
