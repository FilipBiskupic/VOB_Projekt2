using System.Globalization;
using System.Net.Http.Json;
using WeatherAggregator.Interfaces;
using WeatherAggregator.Models;

namespace WeatherAggregator.Providers;

public class OpenMeteoProvider : IWeatherProvider
{
    private readonly HttpClient _httpClient;

    public OpenMeteoProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherData> GetWeatherAsync(string city)
    {
        double latitude = 50.0755; // Praha
        double longitude = 14.4378;
        string formattedCity = "Praha";

        if (city.Equals("Brno", StringComparison.OrdinalIgnoreCase))
        {
            latitude = 49.1951;
            longitude = 16.6068;
            formattedCity = "Brno";
        }
        else if (city.Equals("Ostrava", StringComparison.OrdinalIgnoreCase))
        {
            latitude = 49.8209;
            longitude = 18.2625;
            formattedCity = "Ostrava";
        }
        else if (!city.Equals("Praha", StringComparison.OrdinalIgnoreCase))
        {
            throw new KeyNotFoundException($"Město '{city}' není v našem seznamu souřadnic (podporujeme Praha, Brno, Ostrava).");
        }

        try
        {
            // CultureInfo.InvariantCulture zajistí, že double bude mít v URL vždy TEČKU, i když máte Windows v češtině
            string latStr = latitude.ToString(CultureInfo.InvariantCulture);
            string lonStr = longitude.ToString(CultureInfo.InvariantCulture);

            string url = $"https://api.open-meteo.com/v1/forecast?latitude={latStr}&longitude={lonStr}&current=temperature_2m,relative_humidity_2m";

            var response = await _httpClient.GetFromJsonAsync<OpenMeteoResponse>(url);

            if (response == null)
            {
                throw new Exception("Nepodařilo se načíst data z Open-Meteo API.");
            }

            return new WeatherData(
                Source: "Open-Meteo API (Živá data)",
                Temperature: response.Current.Temperature,
                Humidity: response.Current.Humidity,
                City: formattedCity
            );
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Chyba sítě při komunikaci s Open-Meteo: {ex.Message}");
        }
    }
}