using CookieAuthentication.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CookieAuthentication.Context
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
