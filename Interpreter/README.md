# Descipriton

We want to create an interpreter for a simple assembly progra. An assembly program is composed of instructions, which operate on registers and integers (positive or negative), separated by semicolons. All register names are alphabatical (letters only). The instruction set is:

- "mov x, y" - copies y (eeither a constant value or the content of a register) into register x.
- "inc x" - increments the content of register x by 1.
- "dec x" - decrements the content of register x by 1.
- "jnx x, y" - jumps to an instruction y steps away (postive means forward, negative means backward, y can be a register or a constant), but only if x (a constant or a register) is not zero. Note that the "jnz" instruction moves relative to itself. For example, an offset of -1 would continue at the previous intruction, while an offset of 2 would skip over the next intruction.

The intepreter will be a function which will take a string of the assembly program, it will execute the program, and return a dictionary with the content of the registers after it has finished exeucting the program.

All introduced registers will be a fuction which will take a strin gof the assembly program, it will execute the program, and return a dictionary with the contents of the registers after it has finished executing the program.

All introduced registers will be initialized with 0. Hence an instruciton like mov x, y when y hasn't been encountered yet will create y register initialized with 0 and move its value into x; same applies for x if it hasn't been encoutered yet, create x, initialized with 0, move 0 into x.

For example, for input:

## Example 1

"mov x, 5; inc x; dec x; dec x; jnz x, -1; inc x; inc y; mov z, y; inc z"

instruciton   |    state              |  next pointer
mov x, 5      |    {x: 5}             |    1
inc x         |    {x: 6}             |    2
dec x         |    {x: 5}             |    3
dec x         |    {x: 4}             |    4
jnz x, -1     |    {x: 4}             |    3
dec x         |    {x: 3}             |    4
jnx x, -1     |    {x: 3}             |    3
dec x         |    {x: 2}             |    4
jnx x, -1     |    {x: 2}             |    3
dec x         |    {x: 1}             |    4
jnx x, -1     |    {x: 1}             |    3
dec x         |    {x: 0}             |    4
jnx x, -1     |    {x: 0}             |    5
inc x         |    {x: 1}             |    6
inc y         |    {x: 1, y: 1}       |    7
mov z, y      |    {x: 1, y: 1, z: 1} |    8
inc z         |    {x: 1, y: 1, z: 2} |    null

## Example 2

"mov x, 5; inc x; dec x; dec x; jnz x, -1; inc x; inc y; mov y, z; inc z"

instruciton   |    state              |  next pointer
mov x, 5      |    {x: 5}             |    1
inc x         |    {x: 6}             |    2
dec x         |    {x: 5}             |    3
dec x         |    {x: 4}             |    4
jnz x, -1     |    {x: 4}             |    3
dec x         |    {x: 3}             |    4
jnx x, -1     |    {x: 3}             |    3
dec x         |    {x: 2}             |    4
jnx x, -1     |    {x: 2}             |    3
dec x         |    {x: 1}             |    4
jnx x, -1     |    {x: 1}             |    3
dec x         |    {x: 0}             |    4
jnx x, -1     |    {x: 0}             |    5
inc x         |    {x: 1}             |    6
inc y         |    {x: 1, y: 1}       |    7
mov y, z      |    {x: 1, y: 0, z: 0} |    8
inc z         |    {x: 1, y: 0, z: 1} |    null

Extra points:

1. Write a wells structured, easy to manage solution (for example, taking an object oriented approach or a funtional approach).
2. how would you device a solution allowing rapid addition of new instructions like add, sub, mul, div

# Instruction

to run the program

```bash
cd Interpreter

dotnet run "mov x, 5; inc x; dec x; dec x; jnz x, -1; inc x; inc y; mov z, y; inc z"

dotnet run "mov x, 5; inc x; dec x; dec x; jnz x, -1; inc x; inc y; mov y, z; inc z" --debug

```