using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;

using Hackathon2022.Controls;

using Microsoft.Extensions.Logging;

namespace Hackathon2022;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var Builder = MauiApp.CreateBuilder();
        Builder
            .UseMauiApp<App>()
            .UseMauiMaps()
            .ConfigureFonts(Fonts =>
            {
                Fonts.AddFont("fa_solid.ttf", "FontAwesome");
                Fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                Fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMarkup()
            .ConfigureMauiHandlers(Handlers =>
            {
#if ANDROID
                Handlers.AddHandler<Microsoft.Maui.Controls.Maps.Map, CustomMapHandler>();
#endif
            });

#if DEBUG
        Builder.Logging.AddDebug();
#endif

        return Builder.Build();
    }
}
