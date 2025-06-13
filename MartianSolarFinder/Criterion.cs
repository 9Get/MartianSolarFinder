namespace MartianSolarFinder;

public class Criterion
{
    public string Name { get; }
    public bool IsBenefit { get; }
    public Func<Location, double> GetValue { get; }

    public Criterion(string name, bool isBenefit, Func<Location, double> getValue)
    {
        Name = name;
        IsBenefit = isBenefit;
        GetValue = getValue;
    }
}
