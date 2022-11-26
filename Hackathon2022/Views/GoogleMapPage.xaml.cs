namespace Hackathon2022.Views;

using Newtonsoft.Json.Linq;

using System.Text.Json.Nodes;

public partial class GoogleMapPage : ContentPage
{
    public GoogleMapPage()
    {
        InitializeComponent();
        LoadLocations();
        Mappy.MoveToRegion(new Microsoft.Maui.Maps.MapSpan(
            new Location(12.865416, -85.207229), 12.8654, 85.2072));
    }

    private async void LoadLocations()
    {
        var Stream = await FileSystem.OpenAppPackageFileAsync("NicaraguaManagua.geojson");
        var Reader = new System.IO.StreamReader(Stream);
        var JsonString = await Reader.ReadToEndAsync();
        JObject Json = JObject.Parse(JsonString);

        var Features = Json["features"].AsEnumerable();
    }
}
