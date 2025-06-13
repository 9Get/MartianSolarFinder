using MartianSolarFinder.Metrics;

namespace MartianSolarFinder;

public class TopsisAnalyzer
{
    private readonly IReadOnlyList<Location> _locations;
    private readonly IReadOnlyList<(Criterion criterion, double weight)> _criteriaWithWeights;
    private readonly double[,] _decisionMatrix;

    public TopsisAnalyzer(IReadOnlyList<Location> locations, IReadOnlyList<(Criterion, double)> criteriaWithWeights)
    {
        _locations = locations;
        _criteriaWithWeights = criteriaWithWeights;
        _decisionMatrix = BuildDecisionMatrix();
    }

    public List<TopsisResult> Analyze(IDistanceMetric metric)
    {
        var normalizedMatrix = Normalize(_decisionMatrix);
        var weightedMatrix = ApplyWeights(normalizedMatrix);

        var (idealSolution, antiIdealSolution) = FindIdealAndAntiIdealSolutions(weightedMatrix);

        var distances = CalculateDistances(weightedMatrix, idealSolution, antiIdealSolution, metric);
        var scores = CalculateTopsisScores(distances);

        var results = _locations
            .Select((loc, i) => new TopsisResult(loc, scores[i]))
            .OrderByDescending(r => r.Score)
            .ToList();

        for (int i = 0; i < results.Count; i++)
        {
            results[i].Rank = i + 1;
        }

        return results;
    }

    private double[,] BuildDecisionMatrix()
    {
        var matrix = new double[_locations.Count, _criteriaWithWeights.Count];
        for (int i = 0; i < _locations.Count; i++)
        {
            for (int j = 0; j < _criteriaWithWeights.Count; j++)
            {
                matrix[i, j] = _criteriaWithWeights[j].criterion.GetValue(_locations[i]);
            }
        }
        return matrix;
    }

    private static double[,] Normalize(double[,] matrix)
    {
        var normalizedMatrix = new double[matrix.GetLength(0), matrix.GetLength(1)];
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
            var column = GetColumn(matrix, j);

            var norm = Math.Sqrt(column.Sum(val => val * val));

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                normalizedMatrix[i, j] = norm == 0 ? 0 : matrix[i, j] / norm;
            }
        }
        return normalizedMatrix;
    }

    private double[,] ApplyWeights(double[,] matrix)
    {
        var weightedMatrix = new double[matrix.GetLength(0), matrix.GetLength(1)];
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                weightedMatrix[i, j] = matrix[i, j] * _criteriaWithWeights[j].weight;
            }
        }
        return weightedMatrix;
    }

    private (double[] ideal, double[] antiIdeal) FindIdealAndAntiIdealSolutions(double[,] matrix)
    {
        int numCriteria = matrix.GetLength(1);
        var ideal = new double[numCriteria];
        var antiIdeal = new double[numCriteria];

        for (int j = 0; j < numCriteria; j++)
        {
            var column = GetColumn(matrix, j);
            var criterion = _criteriaWithWeights[j].criterion;
            if (criterion.IsBenefit)
            {
                ideal[j] = column.Max();
                antiIdeal[j] = column.Min();
            }
            else
            {
                ideal[j] = column.Min();
                antiIdeal[j] = column.Max();
            }
        }
        return (ideal, antiIdeal);
    }

    private static List<(double ideal, double antiIdeal)> CalculateDistances(double[,] matrix, double[] ideal, double[] antiIdeal, IDistanceMetric metric)
    {
        var distances = new List<(double, double)>();
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            var row = GetRow(matrix, i);
            var idealDistance = metric.Calculate(row, ideal);
            var antiIdealDistance = metric.Calculate(row, antiIdeal);
            distances.Add((idealDistance, antiIdealDistance));
        }
        return distances;
    }

    private static List<double> CalculateTopsisScores(List<(double ideal, double antiIdeal)> distances)
    {
        return distances
            .Select(d => d.antiIdeal / (d.ideal + d.antiIdeal))
            .ToList();
    }
    private static double[] GetColumn(double[,] matrix, int columnIndex)
    {
        return Enumerable.Range(0, matrix.GetLength(0))
                .Select(i => matrix[i, columnIndex])
                .ToArray();
    }

    private static double[] GetRow(double[,] matrix, int rowIndex)
    {
        return Enumerable.Range(0, matrix.GetLength(1))
                .Select(j => matrix[rowIndex, j])
                .ToArray();
    }
}
