using EverybodyCodes.Application.Models;

namespace EverybodyCodes.Application.Contracts.Services;

public interface ICameraParser
{
    List<CameraDto> Parse(string dataPath, char separator);
}
