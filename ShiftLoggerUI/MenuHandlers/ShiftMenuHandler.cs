using Spectre.Console;

namespace ShiftLoggerUI;

public class ShiftMenuHandler
{
    public void Display()
    {
        MenuPresentation.MenuDisplayer<ShiftMenuOptions>(() => "[blue]Shift Menu[/]", HandleMenuOptions);
    }

    private bool HandleMenuOptions(ShiftMenuOptions option)
    {
        switch (option)
        {
            case ShiftMenuOptions.Quit:
                return false;
            case ShiftMenuOptions.CreateShift:
                break;
            case ShiftMenuOptions.UpdateShift:
                break;
            case ShiftMenuOptions.DeleteShift:
                break;
            case ShiftMenuOptions.ShowShifts:
                break;
            case ShiftMenuOptions.ShowShiftsByEmployee:
                break;
            case ShiftMenuOptions.ManageEmployees:
                break;
            default:
                AnsiConsole.WriteLine($"Unknow option: {option}");
                break;
        }

        return true;
    }
}