# CAIE PseudoCode Runner

This is a project set up to run pseudocode with standards defined by CAIE.

## Usage

```sh
Copyright (C) 2022 PseudoCode.Cli

  -c, --print-operations              Prints compiled operations

  -C, --print-executing-operations    Prints operation being executed currently

  -S, --strict-variables              Requires every variable to be declared
                                      before use / assignment.

  -D, --debug-representation          Outputs debug representation for values

  -l, --locale                        (Default: en) Locale of runtime

  --help                              Display this help screen.

  --version                           Display version information.

  File Path (pos. 0)                  Required. File to run.

```

For example:

```sh
PseudoCode -SvcC run.pseudo
```



## Roadmap

- [x] Declaration & Assignment
- [x] Basic Types
- [x] Array
- [x] Variable autodeclare
- [x] If-else
- [x] While / Repeat
- [x] For
- [x] Function / Procedure
- [x] Case
- [x] Struct
- [x] Enum
- [x] Pointer
- [ ] Class
- [x] Built-in functions

## Differences to / Behaviors not specified in the standard

### File Operations

Binary (`RANDOM`) files are stored in [BSON](https://bsonspec.org/) using [Json.NET](http://json.net/). Every address corresponds to an instance, which has variable size, unlike in implementations in other languages, whose address corresponds to one byte.

### Arrays

#### Multidimensional arrays are always flattened

This allows you to assign `[1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]` to an`ARRAY[1:3, 1:4] OF INTEGER`, for example.

#### Immediate arrays accept elements with different types but converts all to the type of the first element

This is an example:

```pseudocode
arbitraryArray <- [1, "123", TRUE] // ARRAY[1:3] OF INTEGER
arbitraryArray2 <- [1, "aspfijafpj", TRUE] // throws an error because the string cannot be converted to INTEGER.
```

#### Arrays can be assigned without declaration if not run with `-S` option.

I don't think this code will be allowed in standard but it's ok here:

```pseudocode
// No declaration
b <- [[1, 2, 3, 4],[5, 6, 7, 8], [9, 10, 11, 12]]
```

**However**, note that b is of type `ARRAY[1:12] OF INTEGER`, not `ARRAY[1:3, 1:4] OF INTEGER` because of array flattening.

**Another effect** of not declaring before assigning an array is the different behavior from the previous subsection:

```pseudocode
// We add a declaration statement, specifying the element type STRING
DECLARE arbitraryArray : ARRAY[1:3] OF STRING
// Every element is converted into INTEGER, then STRING
// since an immediate array converts all elements into the type of the first element
arbitraryArray <- [1, "2", FALSE] // arbitraryArray = ["1", "2", "0"]
OUTPUT arbitraryArray[3] & " Yes" // 0 Yes
```

### Functions / Procedures

#### Procedures = Functions with a return type of null

They're basically the same thing, just one with a return value and one without. This program treats them the same, so you can use `CALL`  and `BYREF` parameters on functions. (I mean why not lol)

### Errors

There are various types of errors that can be thrown:

#### InvalidAccessError

This is thrown when access operation is not valid (pretty literal):

+ Accessing arrays with non-integer(s)
+ Accessing arrays with more dimensions than the array's
+ Assigning an array to another with different total number of elements
+ Variable / Type member cannot be found in current scope
+ Unary / Binary operation not supported

#### InvalidTypeError

This is thrown when type check fails

+ Trying to call something that is not a function / procedure
+ Assigning non-array to an array
+ Passing a non-reference value to a function argument marked `BYREF`
+ Passing a value to a function argument marked `BYREF` with a different type

#### InvalidArgumentsError

This is thrown when calling a function with at least one argument that is not valid.

#### OutOfBoundsError

This is thrown when accessing an array with index greater than the upper bound or smaller than the lower bound.

#### UnsupportedCastError

This is thrown when a value cannot be casted to a specified type.

#### ReturnBreak

This is thrown when not using return inside a function.

#### Unhandled exception

This can be thrown when the PseudoCode runtime makes an error on itself, or something unexpected happens that breaks the runtime.

### Others

#### Values assigned to a variable is always casted, and values used as right operand is casted to the type of left operand except `INTEGER`

```pseudocode
DECLARE a : INTEGER
a <- TRUE // Allowed
a <- 1 + TRUE // Allowed, TRUE casted into INTEGER 1
a <- TRUE + 1 // UnsupportedCastError
a <- 1 + 1.2 // Allowed, 1 casted into REAL, Value 2.2 is casted into 2 and assigned to a
```

#### For loops accept expressions for variable increase

The following code is accepted

```pseudocode
DECLARE ForArray : ARRAY[1:10] OF INTEGER
FOR i <- 1 TO 10
    DECLARE Num : INTEGER
    OUTPUT i, ":"
    INPUT Num
    FOR ForArray[i] <- 1 TO Num
        OUTPUT ForArray[i]
    NEXT ForArray[i]
NEXT i
OUTPUT ForArray
```

In this example, `i` and `ForArray[i]` are used as variables for comparation. After the for-loop, their values will be the first value that is `Greater` than the target after incrementing by step, which defaults to 1(In this example the targets are `10` and `Num`).

#### Condition expression in Repeat-Until happens in inner scope

Declared variables inside repeat body can be used in `UNTIL`

The following code will be allowed (CAIE uses it anyways):

```pseudocode
REPEAT
    INPUT something
    OUTPUT something
UNTIL something = "YES" // Allowed
```



## Underlying Details

### Execution

This program parses source file using [ANTLR4](https://www.antlr.org/), and generates "Operations" by visiting the ASTs.

Then, the operations undergo a process which is named `MetaOperate` in code, which basically settles down the definitions of each instances and types, checks the validity of the code, and emits all errors, warnings and informations found.

Finally, the whole program is run.

### Built-in functions

[This folder](PseudoCode.Core/Runtime/Reflection) handles reflections for C# functions. The [FunctionBinder](PseudoCode.Core/Runtime/Reflection/FunctionBinder.cs) can bind functions of a type in C# and add them to PseudoCode runtime. The default built-in functions are specified [here](PseudoCode.Core/Runtime/Reflection/BuiltinFunctions.cs).

In the future this feature might be expanded to allow custom function binding and modules and other kinds of stuff.

Range comparison in a `CASE` statement calls function `__in_range` for comparison. The following code:

```pseudocode
a <- 7
CASE a OF
    6: OUTPUT 6
    1 TO 7: OUTPUT "1 to 7"
            OUTPUT "Yay"
    OTHERWISE: OUTPUT "No"
ENDCASE
```

Translates into the following operations:

```
Case:    {
    Duplicate # (4:4)
    Push immediate 6 # (4:4)
    Binary 79 # (4:4)
}
THEN:
{
    Push immediate 6 # (4:14)
    Output 1 # (4:7)
}
{
    Duplicate # (5:6)
    Push ref __in_range # (5:6)
    Swap # (5:6)
    Push immediate 1 # (5:4)
    Push immediate 7 # (5:9)
    Call 3 # (5:6)
}
THEN:
{
    Push immediate 1 to 7 # (5:19)
    Output 1 # (5:12)
    Push immediate Yay # (6:19)
    Output 1 # (6:12)
}
OTHERWISE:     {
    Push immediate No # (7:22)
    Output 1 # (7:15)
}
```

