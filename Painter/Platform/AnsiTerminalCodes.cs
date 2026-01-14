using Painter.Drawing;

namespace Painter.Platform;

internal static class AnsiTerminalCodes
{
    private const string ControlSequenceIntroducer = "\x1b[";
    internal const string FuckAssLinuxThingStolenFromTerminalDotGui = $"{ControlSequenceIntroducer}?1049h";
    
    internal static readonly string CSI_EnableAnyEventMouse = ControlSequenceIntroducer + "?1003h";
    internal static readonly string CSI_EnableSgrExtModeMouse = ControlSequenceIntroducer + "?1006h";
    
    internal static readonly string CSI_EnableUrxvtExtModeMouse = ControlSequenceIntroducer + "?1015h";
    
    internal const string EnableBoldMode = $"{ControlSequenceIntroducer}1m";
    internal const string EnableItalicMode = $"{ControlSequenceIntroducer}3m";
    internal const string EnableUnderlineMode = $"{ControlSequenceIntroducer}4m";
    internal const string EnableDoubleUnderlineMode = $"{ControlSequenceIntroducer}21m"; // linux only? found randomly, not in docs i am using?
    internal const string EnableBlinkingMode = $"{ControlSequenceIntroducer}5m";
    internal const string EnableInverseMode = $"{ControlSequenceIntroducer}7m";
    internal const string EnableStrikethroughMode = $"{ControlSequenceIntroducer}9m";
    
    internal const string DisableBoldMode = $"{ControlSequenceIntroducer}22m";
    internal const string DisableItalicMode = $"{ControlSequenceIntroducer}23m";
    internal const string DisableUnderlineMode = $"{ControlSequenceIntroducer}24m";
    internal const string DisableBlinkingMode = $"{ControlSequenceIntroducer}25m";
    internal const string DisableInverseMode = $"{ControlSequenceIntroducer}27m";
    internal const string DisableStrikethroughMode = $"{ControlSequenceIntroducer}29m";
    
    internal const string ResetForegroundColor = $"{ControlSequenceIntroducer}39m";
    internal const string ResetBackgroundColor = $"{ControlSequenceIntroducer}49m";
    internal const string ResetStyles = $"{ControlSequenceIntroducer}0m";

    internal const string HideCursor = $"{ControlSequenceIntroducer}?25l";
    internal const string ShowCursor = $"{ControlSequenceIntroducer}?25h";
    internal const string ResetCursorPosition = $"{ControlSequenceIntroducer}H";
    internal const string SaveCursorPositon = $"{ControlSequenceIntroducer}[s";
    internal const string RestoreCursorPositon = $"{ControlSequenceIntroducer}[u";

    internal const string SaveScreen = $"{ControlSequenceIntroducer}?47h";
    internal const string DisableLineWrapping = $"{ControlSequenceIntroducer}?7l";
    internal const string EnableLineWrapping = $"{ControlSequenceIntroducer}?7h";
    internal const string RestoreScreen = $"{ControlSequenceIntroducer}?47l";
    internal const string ClearScreen = $"{ControlSequenceIntroducer}3J{ResetCursorPosition}{ControlSequenceIntroducer}J";//$"{ResetCursorPosition}{ControlSequenceIntroducer}J"; // windows \:

    internal static string SetBoldMode(bool enabled)
    {
        return enabled ? EnableBoldMode : DisableBoldMode;
    }
    
    internal static string SetBlinkingMode(bool enabled)
    {
        return enabled ? EnableBlinkingMode : DisableBlinkingMode;
    }
    
    internal static string SetItalicsMode(bool enabled)
    {
        return enabled ? EnableItalicMode : DisableItalicMode;
    }
    
    internal static string SetUnderlineMode(bool enabled)
    {
        return enabled ? EnableUnderlineMode : DisableUnderlineMode;
    }

    internal static string SetStrikethroughMode(bool enabled)
    {
        return enabled ? EnableStrikethroughMode : DisableStrikethroughMode;
    }

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
        return x <= 0 && y <= 0 ? 
            ResetCursorPosition : 
            $"{ControlSequenceIntroducer}{y + 1};{x + 1}H";
    }
    
}