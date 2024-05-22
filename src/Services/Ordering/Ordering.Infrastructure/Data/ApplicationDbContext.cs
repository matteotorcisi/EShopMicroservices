using Ordering.Domain.Models;
using System.Reflection;

/*
 * MIGRATIONS:
 * Add-Migration InitialCreate -OutputDir Data/Migrations -Project Ordering.Infrastructure -StartupProject Ordering.API
 */

namespace Ordering.Infrastructure.Data;
public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
