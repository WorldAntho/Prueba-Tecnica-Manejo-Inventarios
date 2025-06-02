using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Persistence.Context
{
    public class IdentityContext : DbContext
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
        }
    }
}