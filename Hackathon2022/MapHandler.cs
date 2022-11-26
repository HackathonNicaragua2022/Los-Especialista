namespace Hackathon2022.Experimental.NativeMap;

using Android.OS;

using Microsoft.Maui.Handlers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// https://www.cayas.de/blog/dotnet-maui-custom-map-handler

public partial class MapHandler
    : ViewHandler<Microsoft.Maui.Controls.Maps.Map, Android.Gms.Maps.MapView>
{
    private MapHelper _MapHelper;

    internal static Bundle Bundle { get; set; }

    public MapHandler(IPropertyMapper Mapper, CommandMapper CommandMapper = null)
        : base(Mapper, CommandMapper)
    {
    }

    protected override Android.Gms.Maps.MapView CreatePlatformView()
    {
        return new Android.Gms.Maps.MapView(Context);
    }

    protected override void ConnectHandler(Android.Gms.Maps.MapView PlatformView)
    {
        base.ConnectHandler(PlatformView);

        _MapHelper = new MapHelper(Bundle, PlatformView);
        _MapHelper.MapIsReady += MapHelper_MapIsReady;
        _MapHelper.CallCreateMap();
    }

    private void MapHelper_MapIsReady(object Sender, EventArgs Args)
    {
        _MapHelper.Map.UiSettings.ZoomControlsEnabled = true;
        _MapHelper.Map.UiSettings.CompassEnabled = true;
    }
}

class MapHelper : Java.Lang.Object, Android.Gms.Maps.IOnMapReadyCallback
{
    private Bundle _Bundle;
    private Android.Gms.Maps.MapView _MapView;

    public event EventHandler MapIsReady;

    public Android.Gms.Maps.GoogleMap Map { get; set; }

    public MapHelper(Bundle Bundle, Android.Gms.Maps.MapView MapView)
    {
        _Bundle = Bundle;
        _MapView = MapView;
    }

    public void CallCreateMap()
    {
        _MapView.OnCreate(_Bundle);
        _MapView.OnResume();
        _MapView.GetMapAsync(this);
    }

    public void OnMapReady(Android.Gms.Maps.GoogleMap GoogleMap)
    {
        Map = GoogleMap;
        MapIsReady?.Invoke(this, EventArgs.Empty);
    }
}
