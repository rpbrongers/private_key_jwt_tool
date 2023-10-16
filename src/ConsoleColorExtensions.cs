namespace private_key_jwt_tool;

internal static class ConsoleColorExtensions
{
    internal static void ConsoleColorWriteLine(this string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    internal static void ConsoleWriteLine(this string text, bool resetColor = false)
    {
        if (resetColor) Console.ResetColor();
        Console.WriteLine(text);
    }
}