using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Persistence;

public class KinderGartenContext : DbContext
{
    public DbSet<Child> Children { get; set; }
    public DbSet<Toy> Toys { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source = KinderGarten.db");
    }
}