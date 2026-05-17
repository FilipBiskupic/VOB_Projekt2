using WeatherAggregator.Models;

namespace WeatherAggregator.Interfaces;

public interface IWeatherProvider
{
    Task<WeatherData> GetWeatherAsync(string city);
}