using EverybodyCodes.Application.Contracts.Helpers;

namespace EverybodyCodes.Application.Helpers;

public class SystemTimeProvider : ITimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;

    public Task Delay(TimeSpan delay, CancellationToken cancellationToken) => Task.Delay(delay, cancellationToken);
}
