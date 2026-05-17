using System.Text.Json.Serialization;

namespace WeatherAggregator.Models;

public record WeatherData(
    string Source, 
    double Temperature, 
    int Humidity, 
    string City
);

public class OpenMeteoResponse
{
    [JsonPropertyName("current")]
    public CurrentData Current { get; set; } = new();
}

public class CurrentData
{
    [JsonPropertyName("temperature_2m")]
    public double Temperature { get; set; }

    [JsonPropertyName("relative_humidity_2m")]
    public int Humidity { get; set; }
}