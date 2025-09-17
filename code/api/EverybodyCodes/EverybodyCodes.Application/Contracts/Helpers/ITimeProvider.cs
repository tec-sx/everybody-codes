namespace EverybodyCodes.Application.Contracts.Helpers;

public interface ITimeProvider
{
    DateTime UtcNow { get; }
    Task Delay(TimeSpan delay, CancellationToken cancellationToken);
}
