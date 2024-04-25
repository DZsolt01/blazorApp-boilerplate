using Microsoft.EntityFrameworkCore;

namespace fitnessAPI.Entities
{
    public class UsersContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UsersContext(DbContextOptions<UsersContext> options) : base(options)
        {
            
        }
    }
}
