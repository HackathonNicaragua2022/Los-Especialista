namespace HackathonBackend.Models;

using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(Coordinate))]
public class Coordinate
{
    public int Id { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string RouteId { get; set; }

    public virtual Routes Routes { get; set; }
}
