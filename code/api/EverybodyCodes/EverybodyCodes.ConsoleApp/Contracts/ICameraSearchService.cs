namespace EverybodyCodes.ConsoleApp.Contracts;

public interface ICameraSearchService
{
    Task SearchCamerasAsync(string searchTerm);
}
