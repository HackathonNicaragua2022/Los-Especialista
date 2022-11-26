#nullable enable

#if ANDROID

namespace Hackathon2022.Controls;

using Android.Gms.Maps.Model;
using Android.Gms.Maps;

using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps.Handlers;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Platform;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Xamarin.Google.Crypto.Tink.Proto;

public class CustomPin : Pin
{
    public static readonly BindableProperty ImageSourceProperty =
        BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(CustomPin),
                                propertyChanged: OnImageSourceChanged);

    public ImageSource? ImageSource
    {
        get => (ImageSource?)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    public object? Tag { get; set; }

    public Microsoft.Maui.Maps.IMap? Map { get; set; }

    public override bool Equals(object? Pin)
    {
        return Pin is CustomPin P ? P.Tag == Tag : false;
    }

    public override int GetHashCode()
    {
        return Tag?.GetHashCode() ?? 0;
    }

    static async void OnImageSourceChanged(BindableObject Bindable, object OldValue, object NewValue)
    {
        var Control = (CustomPin)Bindable;

        if (Control.Handler?.PlatformView is null)
        {
            // Workaround for when this executes the Handler and PlatformView is null
            Control.HandlerChanged += OnHandlerChanged;
            return;
        }

#if IOS || MACCATALYST
		await Control.AddAnnotation();
#else
        await Task.CompletedTask;
#endif

        void OnHandlerChanged(object? Sender, EventArgs Args)
        {
            OnImageSourceChanged(Control, OldValue, NewValue);
            Control.HandlerChanged -= OnHandlerChanged;
        }
    }
}

public class CustomMapHandler : MapHandler
{
    public static readonly IPropertyMapper<IMap, IMapHandler> CustomMapper =
        new PropertyMapper<IMap, IMapHandler>(Mapper)
        {
            [nameof(IMap.Pins)] = MapPins,
        };

    public CustomMapHandler() : base(CustomMapper, CommandMapper)
    {
    }

    public CustomMapHandler(IPropertyMapper? Mapper = null, CommandMapper? CommandMapper = null)
        : base(Mapper ?? CustomMapper, CommandMapper ?? MapHandler.CommandMapper)
    {
    }

    public List<Marker>? Markers { get; private set; }

    protected override void ConnectHandler(MapView PlatformView)
    {
        base.ConnectHandler(PlatformView);
        var MapReady = new MapCallbackHandler(this);
        base.PlatformView.GetMapAsync(MapReady);
    }

    private static new async void MapPins(IMapHandler Handler, IMap Map)
    {
        if (Handler is CustomMapHandler MapHandler)
        {
            if (MapHandler.Markers != null)
            {
                foreach (var Marker in MapHandler.Markers)
                {
                    Marker.Remove();
                }

                MapHandler.Markers = null;
            }

            await MapHandler.AddPins(Map.Pins);
        }
    }

    private async Task AddPins(IEnumerable<IMapPin> MapPins)
    {
        if (Map is null || MauiContext is null)
        {
            return;
        }

        Markers ??= new List<Marker>();

        foreach (var Pin in MapPins)
        {
            var PinHandler = Pin.ToHandler(MauiContext);

            if (PinHandler is IMapPinHandler MapPinHandler)
            {
                var MarkerOption = MapPinHandler.PlatformView;

                if (Pin is CustomPin _CustomPin)
                {
                    IImageSourceHandler ImageSourceHandler = _CustomPin.ImageSource switch
                    {
                        FileImageSource => new FileImageSourceHandler(),
                        StreamImageSource => new StreamImagesourceHandler(),
                        UriImageSource => new ImageLoaderSourceHandler(),
                        _ => throw new NotImplementedException()
                    };

                    var Bitmap = await ImageSourceHandler.LoadImageAsync(_CustomPin.ImageSource, Android.App.Application.Context);
                    MarkerOption?.SetIcon(Bitmap is null
                        ? BitmapDescriptorFactory.DefaultMarker()
                        : BitmapDescriptorFactory.FromBitmap(Bitmap));
                }

                var Marker = Map.AddMarker(MarkerOption);
                Pin.MarkerId = Marker.Id;
                Markers.Add(Marker);
            }
        }
    }
}

class MapCallbackHandler : Java.Lang.Object, IOnMapReadyCallback
{
    private readonly IMapHandler MapHandler;

    public MapCallbackHandler(IMapHandler MapHandler)
    {
        this.MapHandler = MapHandler;
    }

    public void OnMapReady(GoogleMap GoogleMap)
    {
        MapHandler.UpdateValue(nameof(IMap.Pins));
    }
}

#endif