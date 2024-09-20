using Spectre.Console;

namespace ShiftLoggerUI;

public class EmployeeMenuHandler
{
    public void Display()
    {
        MenuPresentation.MenuDisplayer<EmployeeMenuOptions>(() => "[blue]Employee Menu[/]", HandleMenuOptions);
    }

    private bool HandleMenuOptions(EmployeeMenuOptions option)
    {
        switch (option)
        {
            case EmployeeMenuOptions.Back:
                return false;
            case EmployeeMenuOptions.CreateEmployee:
                break;
            case EmployeeMenuOptions.UpdateEmployee:
                break;
            case EmployeeMenuOptions.DeleteEmployee:
                break;
            case EmployeeMenuOptions.ShowEmployees:
                break;
            default:
                AnsiConsole.WriteLine($"Unknow option: {option}");
                break;
        }

        return true;
    }
}