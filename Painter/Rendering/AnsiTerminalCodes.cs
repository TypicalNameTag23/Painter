using System.Drawing;

namespace Painter.Rendering;

internal static class AnsiTerminalCodes
{
    private const string ControlSequenceIntroducer = "\x1b[";
    
    internal const string ResetForegroundColor = $"{ControlSequenceIntroducer}39";
    internal const string ResetBackgroundColor = $"{ControlSequenceIntroducer}49";
    internal const string Reset = $"{ControlSequenceIntroducer}0";
    
    internal const string ClearDisplay = $"{ControlSequenceIntroducer}2J";
    
    internal const string HideCursor = $"{ControlSequenceIntroducer}?25l";
    internal const string ShowCursor = $"{ControlSequenceIntroducer}?25h";
    
    internal const string RestoreScreen = $"{ControlSequenceIntroducer}?47l";
    internal const string SaveScreen = $"{ControlSequenceIntroducer}?47h";

    internal static string SetForegroundColor(Color? color)
    {
        return color is null ? 
            ResetForegroundColor : 
            $"{ControlSequenceIntroducer}38;2;{color.Value.R};{color.Value.G};{color.Value.B}m";
    }
    
    internal static string SetBackgroundColor(Color? color)
    {
        return color is null ? 
            ResetBackgroundColor : 
            $"{ControlSequenceIntroducer}48;2;{color.Value.R};{color.Value.G};{color.Value.B}m";
    }

    internal static string SetCursorPosition(int x, int y)
    {
        if (x < 0 || y < 0)
        {
            return "";
        }
        
        if (x == 0 && y == 0)
        {
            return $"{ControlSequenceIntroducer}H";
        }
        
        return $"{ControlSequenceIntroducer}{y};{x}H";
    }
    
}