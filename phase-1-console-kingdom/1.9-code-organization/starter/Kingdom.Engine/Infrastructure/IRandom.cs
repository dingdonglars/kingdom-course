namespace Kingdom.Engine.Infrastructure;

public interface IRandom
{
    int Next(int minInclusive, int maxExclusive);
    double NextDouble();
}
