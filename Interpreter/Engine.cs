namespace Interpreter;

public class Engine
{
    public bool IsDebugMode { get; set; } = false;
    public int maxInstructions { get; set; } = 10000;

    public Dictionary<string, int> Registers { get; } = new Dictionary<string, int>();

    private readonly Dictionary<string, Action<string[]>> commandHandlers = [];

    public Engine(bool isDebugMode = false, int maxInstructions = 10000)
    {
        this.IsDebugMode = isDebugMode;
        this.maxInstructions = maxInstructions;
        commandHandlers["mov"] = HandleMov;
        commandHandlers["inc"] = HandleInc;
        commandHandlers["dec"] = HandleDec;
        commandHandlers["add"] = HandleAdd;
        commandHandlers["sub"] = HandleSub;
        commandHandlers["mul"] = HandleMul;
        commandHandlers["div"] = HandleDiv;
    }

    public Engine Interpret(string program)
    {
        if (string.IsNullOrWhiteSpace(program))
        {
            throw new ArgumentException("Program cannot be empty or whitespace.", nameof(program));
        }

        string[] instructions = program.Split(';');

        int i = 0;
        int executedInstructions = 0;
        int maxInstructions = 10000; // Threshold to detect infinite loops

        while (i < instructions.Length)
        {
            if (executedInstructions++ > maxInstructions)
            {
                throw new InvalidOperationException("Infinite loop detected in the program.");
            }

            string instruction = instructions[i].Trim();
            if (!string.IsNullOrEmpty(instruction))
            {
                string[] parts = instruction.Split(' ');
                string command = parts[0];
                string[] args = [.. parts.Skip(1)];
                args = [.. args.Select(arg => arg.Trim().TrimEnd(','))];

                switch (command)
                {
                    case "mov":
                    case "inc":
                    case "dec":
                    case "add":
                    case "sub":
                    case "mul":
                    case "div":
                        ExecuteCommand(command, args);
                        i++;
                        break;
                    case "jnz":
                        i = HandleJnz(args, i);
                        break;
                    default:
                        throw new InvalidOperationException($"Unknown instruction: {command}");
                }
            }
            if (IsDebugMode)
            {
                PrintStep(instruction, i);
            }
        }

        return this;
    }

    public void PrintRegisters()
    {
        Console.WriteLine("Current State:");
        Console.WriteLine("{");
        foreach (var kvp in Registers)
        {
            Console.WriteLine($"  \"{kvp.Key}\": {kvp.Value},");
        }
        Console.WriteLine("}");
    }

    public bool ExecuteCommand(string command, string[] args)
    {
        if (commandHandlers.TryGetValue(command, out var handler))
        {
            handler(args);
            return true;
        }

        return false;
    }

    private void HandleMov(string[] args)
    {
        if (args.Length == 2)
        {
            string register = args[0];
            string valueOrReference = args[1];
            if (!Registers.ContainsKey(register))
            {
                Registers.Add(register, 0);
            }
            if (int.TryParse(valueOrReference, out int value))
            {
                Registers[register] = value;
            }
            else
            {
                string reference = valueOrReference;
                if (!Registers.ContainsKey(reference))
                {
                    Registers.Add(reference, 0);
                }
                Registers[register] = Registers[reference];
            }
        }
        else
        {
            throw new InvalidOperationException("Invalid MOV instruction");
        }
    }

    private void HandleInc(string[] args)
    {
        if (args.Length == 1)
        {
            string register = args[0];
            if (!Registers.ContainsKey(register))
            {
                Registers[register] = 0;
            }
            Registers[register]++;
        }
        else
        {
            throw new InvalidOperationException("Invalid INC instruction");
        }
    }

    private void HandleDec(string[] args)
    {
        if (args.Length == 1)
        {
            string register = args[0];
            if (!Registers.ContainsKey(register))
            {
                Registers[register] = 0;
            }
            Registers[register]--;
        }
        else
        {
            throw new InvalidOperationException("Invalid DEC instruction");
        }
    }

    private int HandleJnz(string[] args, int i)
    {
        if (args.Length == 2)
        {
            string register = args[0];
            string offset = args[1];
            if (Registers.ContainsKey(register) && Registers[register] != 0)
            {
                i += int.Parse(offset);
                return i;
            }
            else
            {
                i++;
                return i;
            }
        }
        else
        {
            throw new InvalidOperationException("Invalid JNZ instruction");
        }
    }

    private void HandleAdd(string[] args)
    {
        if (args.Length == 2)
        {
            string register = args[0];
            string value = args[1];
            if (!Registers.ContainsKey(register))
            {
                Registers.Add(register, int.Parse(value));
            }
            else
            {
                int currentValue = Registers[register];
                Registers[register] = currentValue + int.Parse(value);
            }
        }
        else
        {
            throw new InvalidOperationException("Invalid ADD instruction");
        }
    }

    private void HandleSub(string[] args)
    {
        if (args.Length == 2)
        {
            string register = args[0];
            string value = args[1];
            if (!Registers.ContainsKey(register))
            {
                Registers.Add(register, -int.Parse(value));
            }
            else
            {
                int currentValue = Registers[register];
                Registers[register] = currentValue - int.Parse(value);
            }
        }
        else
        {
            throw new InvalidOperationException("Invalid SUB instruction");
        }
    }

    private void HandleMul(string[] args)
    {
        if (args.Length == 2)
        {
            string register = args[0];
            string value = args[1];
            if (!Registers.ContainsKey(register))
            {
                Registers.Add(register, 0);
            }
            else
            {
                int currentValue = Registers[register];
                Registers[register] = currentValue * int.Parse(value);
            }
        }
        else
        {
            throw new InvalidOperationException("Invalid MUL instruction");
        }
    }

    private void HandleDiv(string[] args)
    {
        if (args.Length == 2)
        {
            string register = args[0];
            string value = args[1];
            if (!Registers.ContainsKey(register))
            {
                Registers.Add(register, 0);
            }
            else
            {
                int currentValue = Registers[register];
                Registers[register] = currentValue / int.Parse(value);
            }
        }
        else
        {
            throw new InvalidOperationException("Invalid DIV instruction");
        }
    }

    private void PrintStep(string currentInstruction, int nextPointer)
    {
        Console.WriteLine($"Executing: {currentInstruction}");
        PrintRegisters();
        Console.WriteLine($"Next Pointer: {nextPointer}");
        Console.WriteLine("Press [Enter] to continue or [Ctrl+C] to exit...");
        Console.ReadLine();
    }
}