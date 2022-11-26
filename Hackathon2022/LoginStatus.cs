using Hackathon2022.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon2022;

static class LoginStatus
{
    public static User User { get; set; }

    public static bool IsSignIn => User != null;

    public static void SignOut() => User = null;

    public static async Task<bool> ExitUser(string UserNameOrEmail)
    {
        try
        {
            using var Client = new HttpClient();
            using HttpResponseMessage Response = await Client.GetAsync(
                $"https://hackathonbackenddestiny.azurewebsites.net/api/Users/Find/{UserNameOrEmail}");
            Response.EnsureSuccessStatusCode();
            string ResponseBody = await Response.Content.ReadAsStringAsync();
            System.Console.WriteLine(ResponseBody);

            return true;
        }
        catch (Exception Ex)
        {
            return false;
        }
    }
}
