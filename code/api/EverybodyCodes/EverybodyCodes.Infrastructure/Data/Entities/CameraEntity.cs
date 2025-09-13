namespace EverybodyCodes.Infrastructure.Data.Entities;

public record CameraEntity : EntityBase
{
    public int Number { get; init; }
    public string Name { get; init; } = string.Empty;
    public float Latitude { get; init; }
    public float Longitude { get; init; }
}
