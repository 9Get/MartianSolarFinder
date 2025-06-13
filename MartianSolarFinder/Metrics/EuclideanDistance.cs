namespace MartianSolarFinder.Metrics;

public class EuclideanDistance : IDistanceMetric
{
    public string Name => "Euclidean";
    public double Calculate(IEnumerable<double> p1, IEnumerable<double> p2)
    {
        return Math.Sqrt(p1.Zip(p2, (a, b) => (a - b) * (a - b)).Sum());
    }
}
