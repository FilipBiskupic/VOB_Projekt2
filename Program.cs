using Microsoft.Extensions.DependencyInjection;
using WeatherAggregator.Interfaces;
using WeatherAggregator.Providers;
using WeatherAggregator.Services;

var services = new ServiceCollection();
services.AddHttpClient();
services.AddTransient<IWeatherProvider, OpenMeteoProvider>();
services.AddTransient<WeatherManager>();

var serviceProvider = services.BuildServiceProvider();
var weatherManager = serviceProvider.GetRequiredService<WeatherManager>();

Console.WriteLine("=== Weather Aggregator Dashboard ===");
Console.Write("Zadejte název města (Praha, Brno, Ostrava): ");
string? city = Console.ReadLine()?.Trim();

if (string.IsNullOrWhiteSpace(city))
{
    Console.WriteLine("Neplatný vstup.");
    return;
}

try 
{
    Console.WriteLine($"Získávám asynchronní živá data z internetu pro město: {city}...");
    double average = await weatherManager.CalculateAverageTemperatureAsync(city);
    
    Console.WriteLine("------------------------------------");
    Console.WriteLine($"Průměrná teplota: {average:F1}°C");
    Console.WriteLine("------------------------------------");
}
catch (KeyNotFoundException ex)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"Chyba validace: {ex.Message}");
    Console.ResetColor();
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Neočekávaná chyba aplikace: {ex.Message}");
    Console.ResetColor();
}