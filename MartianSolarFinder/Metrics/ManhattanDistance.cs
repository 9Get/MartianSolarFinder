namespace MartianSolarFinder.Metrics;

public class ManhattanDistance : IDistanceMetric
{
    public string Name => "Manhattan";
    public double Calculate(IEnumerable<double> p1, IEnumerable<double> p2)
    {
        return p1.Zip(p2, (a, b) => Math.Abs(a - b)).Sum();
    }
}
