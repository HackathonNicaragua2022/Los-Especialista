using Microsoft.Extensions.Logging;

namespace Hackathon2022;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var Builder = MauiApp.CreateBuilder();
        Builder
            .UseMauiApp<App>()
            .ConfigureFonts(Fonts =>
            {
                Fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                Fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        Builder.Logging.AddDebug();
#endif

        return Builder.Build();
    }
}
