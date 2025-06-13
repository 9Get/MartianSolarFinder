namespace MartianSolarFinder.Metrics;

public interface IDistanceMetric
{
    string Name { get; }
    double Calculate(IEnumerable<double> p1, IEnumerable<double> p2);
}
