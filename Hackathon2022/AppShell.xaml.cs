namespace Hackathon2022;

using Hackathon2022.Views;

using System.Windows.Input;

public partial class AppShell : Shell
{
    public Dictionary<string, Type> Routes { get; } = new Dictionary<string, Type>();

    public ICommand HelpCommand => new Command<string>(async (Url) => await Launcher.OpenAsync(Url));

    public AppShell()
    {
        InitializeComponent();
        RegisterRoutes();
        BindingContext = this;
    }

    void RegisterRoutes()
    {
        Routes.Add("Home", typeof(HomePage));
        Routes.Add("Login", typeof(LoginPage));
        Routes.Add("Favorite", typeof(FavoritePage));
        Routes.Add("GoogleMaps", typeof(GoogleMapPage));
        Routes.Add("About", typeof(AboutPage));

        foreach (var Item in Routes)
        {
            Routing.RegisterRoute(Item.Key, Item.Value);
        }
    }
}
