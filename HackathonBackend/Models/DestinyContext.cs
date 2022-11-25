namespace HackathonBackend;

using HackathonBackend.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public class DestinyContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Routes> Routes { get; set; }

    public DbSet<Coordinate> Coordinates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder OptionsBuilder)
        => OptionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Destiny;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder ModelBuilder)
    {
        base.OnModelCreating(ModelBuilder);

        ModelBuilder.Entity<Routes>().HasKey(R => R.Id);
        ModelBuilder.Entity<Routes>()
            .HasMany(R => R.Coordinates)
            .WithOne(C => C.Routes)
            .HasForeignKey(C => C.RouteId);
    }
}
