#nullable enable

namespace Hackathon2022.Views;

using Hackathon2022.Controls;

using System.Text.Json.Nodes;

public partial class GoogleMapPage : ContentPage
{
    private IDispatcherTimer Timer;
    private CancellationTokenSource LocationCancelTokenSource;
    private bool IsCheckingLocation;
    private Dictionary<int, (string, List<Location>)> BusPinLocations;
    private List<IDispatcherTimer> BusLocationTimers;
    private ImageSource PersonImage;
    private ImageSource BusImage;

    public GoogleMapPage()
    {
        InitializeComponent();

        BusImage = ImageSource.FromFile("bus_24.png");
        PersonImage = ImageSource.FromFile("pedestrian_64.png");
        BusPinLocations = new Dictionary<int, (string, List<Location>)>();

        LoadLocations();
        ConfigureTimerForCurrentDeviceLocation();
        ConfigureTimerForBusLocation();
    }

    private async void ConfigureTimerForCurrentDeviceLocation()
    {
        var Coord = await GetCurrentLocation();
        if (Coord != null)
        {
            Mappy.MoveToRegion(
            Microsoft.Maui.Maps.MapSpan.FromCenterAndRadius(
                Coord, Microsoft.Maui.Maps.Distance.FromMiles(2)));
        }

        Timer = Dispatcher.CreateTimer();
        Timer.Interval = TimeSpan.FromMilliseconds(5000);
        Timer.Tick += async delegate
        {
            var Location = await GetCurrentLocation();

            if (Location != null)
            {
                var MapPin = new CustomPin
                {
                    Tag = "PersonDevicePin",
                    Label = LoginStatus.User?.UserName ?? "User",
                    Location = Location,
                    ImageSource = PersonImage
                };

                Mappy.Dispatcher.Dispatch(delegate
                {
                    Mappy.Pins.Remove(MapPin);
                    Mappy.Pins.Add(MapPin);
                });
            }
        };

        Timer.Start();
    }

    private void ConfigureTimerForBusLocation()
    {
        BusLocationTimers = new List<IDispatcherTimer>(BusPinLocations.Count);

        foreach (var Item in BusPinLocations.Take(5))
        {
            var Index = 0;
            var (Name, Locations) = Item.Value;

            var Timer = Dispatcher.CreateTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(1000);
            Timer.Tick += delegate
            {
                var Location = Locations[Index++ % Locations.Count];

                if (Index == Locations.Count)
                {
                    Index = 0;
                }

                var MapPin = new CustomPin
                {
                    Tag = Item.Key,
                    Label = Name,
                    Location = Location,
                    ImageSource = BusImage,
                };

                Mappy.Dispatcher.Dispatch(delegate
                {
                    Mappy.Pins.Remove(MapPin);
                    Mappy.Pins.Add(MapPin);
                });
            };

            Timer.Start();
            BusLocationTimers.Add(Timer);
        }
    }

    /// <summary>
    ///     https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/button?view=net-maui-7.0#press-and-release-the-button
    ///     https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/device/geolocation?tabs=windows&view=net-maui-7.0
    /// </summary>
    /// <returns></returns>
    public async Task<Location?> GetCurrentLocation()
    {
        try
        {
            IsCheckingLocation = true;

            GeolocationRequest Request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

            LocationCancelTokenSource = new CancellationTokenSource();

            Location? Location = await Geolocation.Default.GetLocationAsync(Request, LocationCancelTokenSource.Token);

            return Location;
        }
        catch (Exception Ex)
        {
            return null;
        }
        finally
        {
            IsCheckingLocation = false;
        }
    }

    public void CancelRequest()
    {
        if (IsCheckingLocation && LocationCancelTokenSource != null && LocationCancelTokenSource.IsCancellationRequested == false)
        {
            LocationCancelTokenSource.Cancel();
        }
    }

    [Obsolete("Esta implementación es muy lenta")]
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

                var CustomPin = new CustomPin
                {
                    Location = Location,
                    ImageSource = ImageSource.FromFile("pedestrian_64.png")
                };

                Mappy.Pins.Add(CustomPin);
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

            int Index = 0;

            foreach (var Feature in Features)
            {
                var Properties = Feature.GetProperty("properties");

                List<Location> BusLocations = new List<Location>();
                BusPinLocations.Add(++Index, (Properties.GetProperty("name")!.GetString(), BusLocations));

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

                    var Location = new Location(Latitude, Longitude);

                    BusLocations.Add(Location);
                    Polygon.Geopath.Add(Location);
                }

                Mappy.MapElements.Add(Polygon);
            }
        }
        catch (Exception Ex)
        {
        }
    }
}
