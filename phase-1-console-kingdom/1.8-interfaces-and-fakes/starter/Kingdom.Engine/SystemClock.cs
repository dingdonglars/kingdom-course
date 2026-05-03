namespace Kingdom.Engine;

public class SystemClock : IClock
{
    public DateTime Now => DateTime.UtcNow;
}
