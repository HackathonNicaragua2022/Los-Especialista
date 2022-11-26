namespace Hackathon2022.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Route
    {
        public string Cooperativa { get; set; }

        public string TipoTransporte { get; set; } = "Autobús";

        public string NombreRuta { get; set; }

        public string Referencia { get; set; }
    }

    public static class RoutesData
    {
        public static IList<Route> Routes { get; private set; }

        private static async void LoadRoutes()
        {
            Routes = new List<Route>();

            var Stream = await FileSystem.OpenAppPackageFileAsync("NicaraguaManagua.geojson");
            var JsonDoc = System.Text.Json.JsonDocument.Parse(Stream);
            var Features = JsonDoc.RootElement.GetProperty("features").EnumerateArray();

            foreach (var Feature in Features)
            {
                var Properties = Feature.GetProperty("properties");

                Routes.Add(new Route
                {
                    NombreRuta = Properties.GetProperty("name").GetString(),
                    Cooperativa = Properties.GetProperty("operator").GetString(),
                    Referencia = Properties.GetProperty("ref").GetString()
                });
            }
        }

        static RoutesData()
        {
            LoadRoutes();
        }
    }
}
