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
- [ ] For
- [ ] Function / Procedure
- [ ] Case
- [ ] Type (Struct / Enum / Pointer)
- [ ] Class
- [ ] Built-in functions

## Differences to / Behaviors not specified in the standard

#### Arrays can be assigned without declaration if not run with `-S` option.

I don't think this code will be allowed in standard but it's ok here:

```pseudocode
// No declaration
b <- [[1, 2, 3, 4],[5, 6, 7, 8], [9, 10, 11, 12]]
```

#### Multidimensional arrays are always flattened

This allows you to assign `[1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]` to an`ARRAY[1:3, 1:4] OF INTEGER`, for example.

#### Values assigned to a variable is always casted, and values used as right operand is casted to the type of left operand except `INTEGER`

```pseudocode
DECLARE a : INTEGER
a <- TRUE // Allowed
a <- 1 + TRUE // Allowed, TRUE casted into INTEGER 1
a <- TRUE + 1 // UnsupportedCastError
a <- 1 + 1.2 // Allowed, 1 casted into REAL, Value 2.2 is casted into 2 and assigned to a
```





