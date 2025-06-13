namespace MartianSolarFinder;

public class TopsisResult
{
    public Location Location { get; }
    public double Score { get; }
    public int Rank { get; set; }

    public TopsisResult(Location location, double score)
    {
        Location = location;
        Score = score;
    }
}
