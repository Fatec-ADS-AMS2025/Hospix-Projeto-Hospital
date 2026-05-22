using Hospital.Application.Interfaces;

namespace Hospital.Infrastructure.Services;

public class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}
