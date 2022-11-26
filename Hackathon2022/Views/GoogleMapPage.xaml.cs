namespace Hackathon2022.Views;

using System.Text.Json.Nodes;

public partial class GoogleMapPage : ContentPage
{
    public GoogleMapPage()
    {
        InitializeComponent();
        LoadLocations();
        SetCurrentLocation();
    }

    private async void SetCurrentLocation()
    {
        try
        {
            var Location = await Geolocation.GetLastKnownLocationAsync();

            if (Location != null)
            {
                // Mappy.MoveToRegion(
                //     Microsoft.Maui.Maps.MapSpan.FromCenterAndRadius(
                //         new Location(12.1491726, -86.2727568),
                //         Microsoft.Maui.Maps.Distance.FromMiles(2)));

                Mappy.MoveToRegion(
                    Microsoft.Maui.Maps.MapSpan.FromCenterAndRadius(
                        Location, Microsoft.Maui.Maps.Distance.FromMiles(2)));
            }

        }
        catch (FeatureNotSupportedException Ex)
        {
            // Handle not supported on device exception
        }
        catch (FeatureNotEnabledException EX)
        {
            // Handle not enabled on device exception
        }
        catch (PermissionException Ex)
        {
            // Handle permission exception
        }
        catch (Exception Ex)
        {
            // Unable to get location
        }
    }

    private async void LoadLocations()
    {
        try
        {
            var Stream = await FileSystem.OpenAppPackageFileAsync("NicaraguaManagua.geojson");
            var JsonDoc = System.Text.Json.JsonDocument.Parse(Stream);
            var Features = JsonDoc.RootElement.GetProperty("features").EnumerateArray();

            foreach (var Feature in Features)
            {
                var Properties = Feature.GetProperty("properties");

                var Polygon = new Microsoft.Maui.Controls.Maps.Polygon();

                var Colour = Properties.GetProperty("colour").GetString();
                var PolygonColor = Color.FromArgb(Colour);

                Polygon.StrokeWidth = 12;
                Polygon.StrokeColor = PolygonColor;
                // Polygon.FillColor = PolygonColor;

                var Geometry = Feature.GetProperty("geometry");
                var Coordinates = Geometry.GetProperty("coordinates").EnumerateArray();

                foreach (var Coordinate in Coordinates)
                {
                    static IEnumerable<double> GetCoordinates(System.Text.Json.JsonElement Element)
                    {
                        if (Element.ValueKind == System.Text.Json.JsonValueKind.Number)
                        {
                            yield return Element.GetDouble();
                        }
                        else if (Element.ValueKind == System.Text.Json.JsonValueKind.Array)
                        {
                            foreach (var C in Element.EnumerateArray())
                            {
                                foreach (var R in GetCoordinates(C))
                                {
                                    yield return R;
                                }
                            }
                        }
                    }

                    var C = GetCoordinates(Coordinate).ToArray();

                    double Latitude = C[1];
                    double Longitude = C[0];

                    Polygon.Geopath.Add(new Location(Latitude, Longitude));

                }

                Mappy.MapElements.Add(Polygon);
            }


        }
        catch (Exception Ex)
        {
        }
    }
}
