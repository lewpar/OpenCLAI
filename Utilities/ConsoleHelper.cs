namespace OpenCLAI.Utilities
{
    public class ConsoleHelper
    {
        public static void WriteLine(string message, ConsoleColor color)
        {
            ConsoleColor prevColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = prevColor;
        }
    }
}
