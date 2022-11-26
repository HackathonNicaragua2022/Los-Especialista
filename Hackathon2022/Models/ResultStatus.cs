namespace Hackathon2022.Models;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class ResultStatus
{
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonProperty("title")]
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonProperty("status")]
    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonProperty("traceId")]
    [JsonPropertyName("traceId")]
    public string TraceId { get; set; }
}

