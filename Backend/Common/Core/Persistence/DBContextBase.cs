using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Backend.Common.Core.Persistence
{
    public abstract class DBContextBase : DbContext
    {

        public DBContextBase(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}