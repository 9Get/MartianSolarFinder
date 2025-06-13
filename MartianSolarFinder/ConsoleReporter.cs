namespace MartianSolarFinder;

public static class ConsoleReporter
{
    public static void PrintInitialData(IReadOnlyList<Location> locations, IReadOnlyList<(Criterion criterion, double weight)> criteria)
    {
        Console.WriteLine("1. Initial Data & Criteria Weights:");
        Console.WriteLine("-----------------------------------");
        foreach (var loc in locations)
        {
            Console.WriteLine($"{loc.Name,-18} | " +
                              $"Insol: {loc.Insolation:F1}, " +
                              $"Dust: {loc.DustStormFrequency:F1}, " +
                              $"Slope: {loc.SurfaceSlope:F1}, " +
                              $"Temp: {loc.Temperature:F1}, " +
                              $"Soil: {loc.SoilStability:F1}, " +
                              $"Energy: {loc.TransportEnergy:F1}");
        }

        Console.WriteLine("\nCriteria Weights:");
        Console.WriteLine(string.Join(", ", criteria.Select(c => $"{c.criterion.Name}: {c.weight}")));
        Console.WriteLine("-----------------------------------\n");
    }

    public static void PrintRankedResults(List<TopsisResult> results, string metricName)
    {
        Console.WriteLine($"--- Results using {metricName} metric ---");
        Console.WriteLine($"{"Rank",-5} {"Location",-18} {"Score",-10}");
        Console.WriteLine(new string('-', 35));

        foreach (var result in results)
        {
            Console.WriteLine($"{result.Rank,-5} {result.Location.Name,-18} {result.Score,-10:F4}");
        }

        var optimal = results.First();
        Console.WriteLine($"\nOptimal Location: {optimal.Location.Name} (Score: {optimal.Score:F4})\n");
    }
}
