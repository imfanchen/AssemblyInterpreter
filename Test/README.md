# Instruction

Run following in a separate terminal

```bash
cd Test

dotnet test --logger "console;verbosity=detailed"

dotnet test --filter "FullyQualifiedName~Test.InterpreterTests.TestExample1"

```