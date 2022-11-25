namespace HackathonBackend.Models;

using Newtonsoft.Json;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

[Table(nameof(Routes))]
public class Routes
{
    [Key]
    public string Id { get; set; }

    public string Colour { get; set; }

    public string Direction { get; set; }

    public string Duration { get; set; }

    public string Fare { get; set; }

    public string FareCurrency { get; set; }

    public string From { get; set; }

    public string Name { get; set; }

    public string Network { get; set; }

    public string OpeningHours { get; set; }

    public string Operator { get; set; }

    public string PaymentCash { get; set; }

    public string PaymentTuc { get; set; }

    public string PublicTransportVersion { get; set; }

    public string Ref { get; set; }

    public string Route { get; set; }

    public string To { get; set; }

    public string Type { get; set; }

    public DateTime Timestamp { get; set; }

    public string Version { get; set; }

    public string Changeset { get; set; }

    public string User { get; set; }

    public string Uid { get; set; }

    public virtual ICollection<Coordinate> Coordinates { get; set; }
}

