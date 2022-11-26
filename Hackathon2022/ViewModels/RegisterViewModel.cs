namespace Hackathon2022.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Hackathon2022.Models;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[INotifyPropertyChanged]
public partial class RegisterViewModel
{
    [ObservableProperty]
    string _UserName;

    [ObservableProperty]
    string _Password;

    [ObservableProperty]
    string _Email;

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

    partial void OnEmailChanged(string value)
    {
        OnPropertyChanged(nameof(IsValid));
    }

    public bool IsValid => !string.IsNullOrWhiteSpace(_UserName)
                        && !string.IsNullOrWhiteSpace(_Password)
                        && !string.IsNullOrWhiteSpace(_Email);

    [RelayCommand(CanExecute = nameof(IsValid))]
    async Task RegisterUser()
    {
        try
        {
            IsVisible = true;

            if (!await LoginStatus.ExitUser(UserName) && !await LoginStatus.ExitUser(Email))
            {
                using var Client = new HttpClient();

                string Json = JsonConvert.SerializeObject(new User
                {
                    UserName = UserName,
                    Password = Password,
                    Email = Email
                });

                StringContent Content = new StringContent(Json, Encoding.UTF8, "application/json");

                using HttpResponseMessage Response = await Client.PostAsync(
                    $"https://hackathonbackenddestiny.azurewebsites.net/api/Users", Content);
                Response.EnsureSuccessStatusCode();
                string ResponseBody = await Response.Content.ReadAsStringAsync();
                System.Console.WriteLine(ResponseBody);

                var User = JsonConvert.DeserializeObject<User>(ResponseBody);
                IsVisible = false;
                LoginStatus.User = User;
                await Shell.Current.GoToAsync("//GoogleMaps");
            }
            else
            {
                IsVisible = false;
                await App.Current.MainPage.DisplayAlert("Destino",
                    "Usuario o correo ya existe ", "Aceptar");
            }
        }
        catch (Exception Ex)
        {
            IsVisible = false;
            await App.Current.MainPage.DisplayAlert("Destino",
                "Usuario o Contraseña Incorrecto", "Aceptar");
        }
    }
}
