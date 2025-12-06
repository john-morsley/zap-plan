
namespace Morsley.UK.ZAP.SwaggerSplitter.Console;

internal class Display
{
    internal static void Title(string value)
    {
        Blank();
        System.Console.ForegroundColor = ConsoleColor.DarkGray;
        System.Console.WriteLine(new string('=', value.Length));
        System.Console.ForegroundColor = ConsoleColor.DarkGreen;
        System.Console.WriteLine(value);
        System.Console.ForegroundColor = ConsoleColor.DarkGray;
        System.Console.WriteLine(new string('=', value.Length));
        System.Console.ResetColor();
    }

    internal static void Normal(string value)
    {
        System.Console.ForegroundColor = ConsoleColor.White;
        System.Console.WriteLine(value);
        System.Console.ResetColor();
    }

    internal static void Warning(string value)
    {
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine(value);
        System.Console.ResetColor();
    }

    internal static void Good(string value)
    {
        System.Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine(value);
        System.Console.ResetColor();
    }

    internal static void Bad(string value)
    {
        System.Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine(value);
        System.Console.ResetColor();
    }

    internal static void Blank()
    {
        System.Console.WriteLine();
    }

    internal static void Mute(string value)
    {
        System.Console.ForegroundColor = ConsoleColor.DarkGray;
        System.Console.WriteLine(value);
        System.Console.ResetColor();
    }
}