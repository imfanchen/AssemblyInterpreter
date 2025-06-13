namespace Test;

using Interpreter;

[TestClass]
public sealed class InterpreterTests
{
    [TestMethod]
    public void TestExample1()
    {
        string program = "mov x, 5; inc x; dec x; dec x; jnz x, -1; inc x; inc y; mov z, y; inc z";
        Engine engine = new();
        engine.Interpret(program);
        var result = engine.Registers;
        var expected = new Dictionary<string, int> { { "x", 1 }, { "y", 1 }, { "z", 2 } };
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    public void TestExample2()
    {
        string program = "mov x, 5; inc x; dec x; dec x; jnz x, -1; inc x; inc y; mov y, z; inc z";
        Engine engine = new();
        engine.Interpret(program);
        var result = engine.Registers;
        var expected = new Dictionary<string, int> { { "x", 1 }, { "y", 0 }, { "z", 1 } };
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    public void TestEmptyProgram()
    {
        string program = "";
        Engine engine = new();
        Assert.ThrowsException<ArgumentException>(() => engine.Interpret(program));
    }

    [TestMethod]
    public void TestUndefinedRegisters()
    {
        string program = "inc a; dec b; mov c, d";
        Engine engine = new();
        engine.Interpret(program);
        var result = engine.Registers;
        var expected = new Dictionary<string, int> { { "a", 1 }, { "b", -1 }, { "c", 0 }, { "d", 0 } };
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    public void TestInvalidSyntax()
    {
        string program = "mov x 5; inc x; jnz x -1"; // Missing commas
        Engine engine = new();
        Assert.ThrowsException<InvalidOperationException>(() => engine.Interpret(program));
    }

    [TestMethod]
    public void TestLargeJump()
    {
        string program = "mov x, 1; jnz x, 100; inc y";
        Engine engine = new();
        engine.Interpret(program);
        var result = engine.Registers;
        var expected = new Dictionary<string, int> { { "x", 1 } }; // y is never incremented
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    public void TestFakeJump()
    {
        string program = "mov x, 1; jnz x, 0; inc y";
        Engine engine = new();
        Assert.ThrowsException<InvalidOperationException>(() => engine.Interpret(program));
    }

    [TestMethod]
    public void TestCircularJump()
    {
        // infinite loop
        string program = "mov x, 1; jnz x, -1; inc y";
        Engine engine = new();
        Assert.ThrowsException<InvalidOperationException>(() => engine.Interpret(program));
    }
}