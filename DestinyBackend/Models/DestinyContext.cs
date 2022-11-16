using Microsoft.EntityFrameworkCore;

namespace DestinyBackend.Models;

public class DestinyContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Coordinate> Coordinates { get; set; }
}

