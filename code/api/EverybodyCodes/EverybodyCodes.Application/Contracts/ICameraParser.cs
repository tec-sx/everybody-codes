using EverybodyCodes.Application.Models;

namespace EverybodyCodes.Application.Contracts;

public interface ICameraParser
{
    List<CameraDto> Parse(string dataPath, char separator);
}
