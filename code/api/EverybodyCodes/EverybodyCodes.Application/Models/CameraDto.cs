namespace EverybodyCodes.Application.Models;

public record CameraDto
{
    public int Number { get; init; }
    public string Name { get; init; } = string.Empty;
    public float Latitude { get; init; }
    public float Longitude { get; init; }
}
