// ReSharper disable InconsistentNaming
namespace Core.Dice;

public interface IRandomSource
{
    int ZeroTo(int exclusiveMaximum);
}

public interface IDice
{
    
}

public interface IDice<TSide> : IDice
{
    TSide Roll();
}

public sealed class d6
{
    private readonly IRandomSource _randomSource;

    public d6(IRandomSource randomSource)
    {
        _randomSource = randomSource;
    }
    
    
}