using WeatherAggregator.Interfaces;
using WeatherAggregator.Models;

namespace WeatherAggregator.Services;

public class WeatherManager
{
    private readonly IEnumerable<IWeatherProvider> _providers;

    public WeatherManager(IEnumerable<IWeatherProvider> providers)
    {
        _providers = providers;
    }

    public async Task<double> CalculateAverageTemperatureAsync(string city)
    {
        var tasks = _providers.Select(p => p.GetWeatherAsync(city)).ToList();
        var results = await Task.WhenAll(tasks);

        if (results.Length == 0) return 0;

        return results.Average(r => r.Temperature);
    }
}