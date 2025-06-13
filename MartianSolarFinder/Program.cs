using MartianSolarFinder.Metrics;

namespace MartianSolarFinder;

public static class Program
{
    static void Main(string[] args)
    {
        var locations = GetLocations();

        var criteriaWithWeights = new List<(Criterion, double)>
        {
            (new Criterion("Insolation", true, l => l.Insolation), 0.3),
            (new Criterion("Dust Storm Freq.", false, l => l.DustStormFrequency), 0.2),
            (new Criterion("Surface Slope", false, l => l.SurfaceSlope), 0.1),
            (new Criterion("Temperature", true, l => l.Temperature), 0.2),
            (new Criterion("Soil Stability", true, l => l.SoilStability), 0.1),
            (new Criterion("Transport Energy", false, l => l.TransportEnergy), 0.1)
        };

        var metricsToTest = new List<IDistanceMetric>
        {
            new EuclideanDistance(),
            new ManhattanDistance(),
            new ChebyshevDistance()
        };

        ConsoleReporter.PrintInitialData(locations, criteriaWithWeights);

        var analyzer = new TopsisAnalyzer(locations, criteriaWithWeights);

        foreach (var metric in metricsToTest)
        {
            var results = analyzer.Analyze(metric);
            ConsoleReporter.PrintRankedResults(results, metric.Name);
        }
    }

    public static List<Location> GetLocations()
    {
        return new List<Location>
        {
            new Location { Name = "Elysium Planitia", Insolation = 8.5, DustStormFrequency = 3, SurfaceSlope = 1, Temperature = -60, SoilStability = 8, TransportEnergy = 4 },
            new Location { Name = "Tharsis Plateau", Insolation = 9.0, DustStormFrequency = 6, SurfaceSlope = 2, Temperature = -55, SoilStability = 7, TransportEnergy = 5 },
            new Location { Name = "Hellas Planitia", Insolation = 8.0, DustStormFrequency = 5, SurfaceSlope = 4, Temperature = -70, SoilStability = 6, TransportEnergy = 3 },
            new Location { Name = "Meridiani Planum", Insolation = 8.8, DustStormFrequency = 2, SurfaceSlope = 1, Temperature = -58, SoilStability = 8, TransportEnergy = 4 },
            new Location { Name = "Valles Marineris", Insolation = 7.5, DustStormFrequency = 4, SurfaceSlope = 3, Temperature = -65, SoilStability = 9, TransportEnergy = 6 }
        };
    }
}