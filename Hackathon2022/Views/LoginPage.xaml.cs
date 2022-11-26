using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace Hackathon2022.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        ModifyEntry();
    }

    void ModifyEntry()
    {
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("MyCustomization", (Handler, View) =>
        {
            const int PaddingTopBottom = 5;
            const int PaddingLeftRight = 15;

            if (View is Entry)
            {
                Handler.PlatformView.SetPadding(PaddingLeftRight, PaddingTopBottom, PaddingLeftRight, PaddingTopBottom);
            }
        });
    }

    private async void LoginButton_Clicked(object sender, EventArgs e)
    {
        await App.Current.MainPage.DisplayAlert("Destino", "Se ha Iniciado sesión", "Aceptar");
    }
}