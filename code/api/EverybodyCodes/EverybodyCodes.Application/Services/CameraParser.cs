using EverybodyCodes.Application.Contracts;
using EverybodyCodes.Application.Models;
using System.Globalization;

namespace EverybodyCodes.Application.Services;

public class CameraParser : ICameraParser
{
    public List<CameraDto> Parse(string dataPath, char separator)
    {
        if (string.IsNullOrWhiteSpace(dataPath))
        {
            throw new ArgumentException("Data path cannot be empty");
        }

        if (!File.Exists(dataPath))
        {
            throw new FileNotFoundException($"File not found: {dataPath}");
        }

        var cameras = new List<CameraDto>();
        var lines = File.ReadAllLines(dataPath);

        if (lines.Length == 0)
        {
            return cameras;
        }

        // Starts from 1 to skip the header row Camera; Latitude; Longitude
        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i].Trim();

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            try
            {
                var camera = ParseLine(line, separator);

                if (camera != null)
                {
                    cameras.Add(camera);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing line {i + 1}: {ex.Message}");
            }
        }

        return cameras;
    }

    private CameraDto? ParseLine(string line, char separator)
    {
        int fieldIdx = 0;
        var fields = new List<string>();
        var currentField = string.Empty;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == separator)
            {
                // If it is the first field (Camera Name), extract only digits for the camera number
                if (fieldIdx == 0)
                {
                    string numberString = new string(currentField.Trim().Where(char.IsDigit).ToArray());
                    fields.Add(numberString);
                }

                fields.Add(currentField.Trim());
                currentField = string.Empty;
                fieldIdx++;
            }
            else
            {
                currentField += c;
            }
        }

        // Add the last field
        fields.Add(currentField);

        if (fields.Count < 4)
        {
            return null;
        }

        int number = GetCameraNumber(fields[0]);
        string name = fields[1];
        float lattitude = GetCoordinate(fields[2]);
        float longitude = GetCoordinate(fields[3]);

        var camera = new CameraDto()
        {
            Number = number,
            Name = name,
            Latitude = lattitude,
            Longitude = longitude
        };

        return camera;
    }

    private int GetCameraNumber(string numberString)
    {
        if (int.TryParse(numberString, out int cameraNumber))
        {
            return cameraNumber;
        }
        else
        {
            throw new FormatException($"Invalid camera number: {numberString}");
        }
    }

    private float GetCoordinate(string coordinateString)
    {
        if (float.TryParse(coordinateString, NumberStyles.Float, CultureInfo.InvariantCulture, out float coordinate))
        {
            return coordinate;
        }
        else
        {
            throw new FormatException($"Invalid coordinate value: {coordinateString}");
        }
    }
}
