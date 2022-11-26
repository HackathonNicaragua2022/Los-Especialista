namespace Hackathon2022.Views;

public partial class GoogleMapPage : ContentPage
{
    public GoogleMapPage()
    {
        InitializeComponent();

        Mappy.MoveToRegion(new Microsoft.Maui.Maps.MapSpan(
            new Location(12.865416, -85.207229), 12.8654, 85.2072));
    }
}
