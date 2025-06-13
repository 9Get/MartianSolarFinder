namespace MartianSolarFinder.Metrics;

public class ChebyshevDistance : IDistanceMetric
{
    public string Name => "Chebyshev";
    public double Calculate(IEnumerable<double> p1, IEnumerable<double> p2)
    {
        return p1.Zip(p2, (a, b) => Math.Abs(a - b)).Max();
    }
}