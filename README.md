# CAIE PseudoCode Runner

This is a project set up to run pseudocode with standards defined by CAIE.

## Usage

```sh
PseudoCode <options> <file>
Available options:
  -v, --verbose                 Prints extra info

  -S, --strict-variables        Requires every variable to be declared before
                                use / assignment.

  -D, --debug-representation    Outputs debug representation for values

  -l, --locale                  (Default: en) Locale of runtime

  --help                        Display this help screen.

  --version                     Display version information.

```

For example:

```sh
PseudoCode -Sv run.pseudo
```



## Roadmap

- [x] Declaration & Assignment
- [x] Basic Types
- [x] Array
- [x] Variable autodeclare
- [x] If-else
- [x] While / Repeat
- [x] For
- [ ] Function / Procedure
- [ ] Case
- [ ] Type (Struct / Enum / Pointer)
- [ ] Class
- [ ] Built-in functions

## Differences to / Behaviors not specified in the standard

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



