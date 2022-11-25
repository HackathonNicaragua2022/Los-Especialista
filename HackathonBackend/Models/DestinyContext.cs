namespace HackathonBackend;

using HackathonBackend.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using Microsoft.Azure; // Namespace for CloudConfigurationManager

public class DestinyContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Routes> Routes { get; set; }

    public DbSet<Coordinate> Coordinates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder OptionsBuilder)
    {
        // OptionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Destiny;Trusted_Connection=True;");
        OptionsBuilder.UseSqlServer("Server=tcp:hackathonbackenddbserver.database.windows.net,1433;Initial Catalog=HackathonBackend_db;Persist Security Info=False;User ID=Destino;Password=#@$Destiny$#&;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
    }

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
