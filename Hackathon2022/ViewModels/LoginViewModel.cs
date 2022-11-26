namespace Hackathon2022.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Hackathon2022.Models;

using Microsoft.AspNetCore.Identity;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[INotifyPropertyChanged]
public partial class LoginViewModel
{
    [ObservableProperty]
    string _UserName;

    [ObservableProperty]
    string _Password;

    [ObservableProperty]
    bool _IsVisible;

    partial void OnUserNameChanged(string value)
    {
        OnPropertyChanged(nameof(IsValid));
    }

    partial void OnPasswordChanged(string value)
    {
        OnPropertyChanged(nameof(IsValid));
    }

    public bool IsValid => !string.IsNullOrWhiteSpace(_UserName)
                        && !string.IsNullOrWhiteSpace(_Password);

    [RelayCommand]
    async Task SignIn()
    {
        try
        {
            IsVisible = true;

            using var Client = new HttpClient();
            using HttpResponseMessage Response = await Client.GetAsync(
                $"https://hackathonbackenddestiny.azurewebsites.net/api/Users/Find/{UserName}");
            Response.EnsureSuccessStatusCode();

            string ResponseBody = await Response.Content.ReadAsStringAsync();
            var User = JsonConvert.DeserializeObject<User>(ResponseBody);

            var HashPassword = new PasswordHasher<object>().HashPassword(null, Password ?? string.Empty);
            var PasswordVerificationResult = new PasswordHasher<object>().VerifyHashedPassword(null, User.Password, HashPassword);

            switch (PasswordVerificationResult)
            {
                case PasswordVerificationResult.Success:
                case PasswordVerificationResult.SuccessRehashNeeded:
                    IsVisible = false;
                    LoginStatus.User = User;
                    await Shell.Current.GoToAsync("//GoogleMaps");
                    break;
            }

            throw new InvalidOperationException();
        }
        catch (Exception Ex)
        {
            IsVisible = false;
            await App.Current.MainPage.DisplayAlert("Destino", "Usuario o Contraseña Incorrecto", "Aceptar");
        }
    }
}
