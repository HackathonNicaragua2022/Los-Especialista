using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;

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
                Fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                Fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMarkup();

#if DEBUG
        Builder.Logging.AddDebug();
#endif

        return Builder.Build();
    }
}
