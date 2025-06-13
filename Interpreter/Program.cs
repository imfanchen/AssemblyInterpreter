namespace Interpreter;

class Program
{
    static void Main(string[] args)
    {
        bool isDebugMode = args.Contains("--debug");

        if (isDebugMode)
        {
            args = args.Where(arg => arg != "--debug").ToArray();
        }

        if (args.Length == 0 || string.IsNullOrWhiteSpace(args[0]))
        {
            Console.WriteLine("Please provide a valid program as a command-line argument.");
            return;
        }

        string program = args[0];
        Engine engine = new(isDebugMode);
        engine.Interpret(program);
        engine.PrintRegisters();
    }
}
