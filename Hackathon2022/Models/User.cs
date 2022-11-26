namespace Hackathon2022.Models;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class User
{
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonProperty("userName")]
    [JsonPropertyName("userName")]
    public string UserName { get; set; }

    [JsonProperty("password")]
    [JsonPropertyName("password")]
    public string Password { get; set; }

    [JsonProperty("email")]
    [JsonPropertyName("email")]
    public string Email { get; set; }
}
