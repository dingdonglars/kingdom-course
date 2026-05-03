namespace Kingdom.Engine.Infrastructure;

public class SystemClock : IClock
{
    public DateTime Now => DateTime.UtcNow;
}
