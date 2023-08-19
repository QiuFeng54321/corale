using PseudoCode.Core.Runtime.Errors;
using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;
using PseudoCode.Core.Runtime.Types;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Reflection;

public static class BuiltinFunctions
{
    public static Random Random = new();

    [Documentation("Checks if a file is read to the end")]
    [BuiltinFunction("EOF")]
    [ParamType("path", "STRING")]
    [ReturnType("BOOLEAN")]
    public static Instance Eof(Scope parentScope, PseudoProgram program, Instance[] arguments)
    {
        var path = arguments[0].Get<string>();
        return parentScope.FindDefinition(Type.BooleanId).Type.Instance(program.OpenFiles[path].Eof());
    }

    [Documentation("Returns a string containing `x` characters from the right of the source string")]
    [BuiltinNativeFunction("RIGHT")]
    public static string Right(string thisString, int x)
    {
        if (x < 1 || x > thisString.Length)
            throw new OutOfBoundsError(
                $"Cannot take substring [^{x}..] of \"{thisString}\": Length of string is {thisString.Length}", null);

        return thisString[^x..];
    }

    [Documentation("Returns a string containing `x` characters from the start of the source string")]
    [BuiltinNativeFunction("LEFT")]
    public static string Left(string thisString, int x)
    {
        if (x < 1 || x > thisString.Length)
            throw new OutOfBoundsError(
                $"Cannot take substring [..{x}] of \"{thisString}\": Length of string is {thisString.Length}",
                null);

        return thisString[..x];
    }

    [Documentation("Returns the length of the string")]
    [BuiltinNativeFunction("LENGTH")]
    public static int Length(string thisString)
    {
        return thisString.Length;
    }

    [Documentation("Returns if `target` falls between the bounds inclusively.\n**You shouldn't use this**")]
    [BuiltinNativeFunction("__in_range")]
    public static bool InRange(RealNumberType target, RealNumberType from, RealNumberType to)
    {
        return from <= target && target <= to;
    }

    [Documentation("Returns the substring of `length` of the source string starting from `index`")]
    [BuiltinNativeFunction("MID")]
    public static string Mid(string thisString, int index, int length)
    {
        if (index < 1 || index > thisString.Length || length < 1 || index + length - 1 > thisString.Length)
            throw new OutOfBoundsError(
                $"Cannot take substring [{index}..{index + length}] of \"{thisString}\": Length of string is {thisString.Length}",
                null);
        return thisString.Substring(index - 1, length);
    }

    [Documentation("Turns the character into lower case.\n`'C' -> 'c'`\n`'c' -> 'c'`")]
    [BuiltinNativeFunction("LCASE")]
    public static char LowerCase(char thisChar)
    {
        return char.ToLower(thisChar);
    }

    [Documentation("Turns the character into upper case.\n`'C' -> 'C'`\n`'c' -> 'C'`")]
    [BuiltinNativeFunction("UCASE")]
    public static char UpperCase(char thisChar)
    {
        return char.ToUpper(thisChar);
    }

    [Documentation("Turns all the characters in the string into upper case.\n`\"aBcD\" -> \"ABCD\"`")]
    [BuiltinNativeFunction("TO_UPPER")]
    public static string ToUpper(string str) => str.ToUpper();


    [Documentation("Turns all the characters in the string into lower case.\n`\"aBcD\" -> \"abcd\"`")]
    [BuiltinNativeFunction("TO_LOWER")]
    public static string ToLower(string str) => str.ToLower();

    [Documentation("Converts a number into a string")]
    [BuiltinNativeFunction("NUM_TO_STR")]
    public static string NumToStr(RealNumberType num) => num.ToString();

    [Documentation("Converts a string into a number")]
    [BuiltinNativeFunction("STR_TO_NUM")]
    public static RealNumberType StrToNum(string str) => RealNumberType.Parse(str);

    [Documentation("Returns if the given string can be parsed as a number")]
    [BuiltinNativeFunction("IS_NUM")]
    public static bool IsNum(string str) => RealNumberType.TryParse(str, out _);

    [Documentation("Returns the ascii index of a character")]
    [BuiltinNativeFunction("ASC")]
    public static int Ascii(char chr) => chr;

    [Documentation("Returns the character from the given ascii index")]
    [BuiltinNativeFunction("CHR")]
    public static char Char(int ascii) => (char)ascii;

    [Documentation("Returns the day component of a date")]
    [BuiltinNativeFunction("DAY")]
    public static int Day(DateOnly date) => date.Day;

    [Documentation("Returns the month component of a date")]
    [BuiltinNativeFunction("MONTH")]
    public static int Month(DateOnly date) => date.Month;

    [Documentation("Returns the year component of a date")]
    [BuiltinNativeFunction("YEAR")]
    public static int Year(DateOnly date) => date.Year;

    [Documentation("Returns the day of a date (`Sunday` = 1, `Saturday` = 7)")]
    [BuiltinNativeFunction("DAYINDEX")]
    public static int DayIndex(DateOnly date) => (int)date.DayOfWeek + 1;

    [Documentation("Constructs a date using given `day`, `month`, and `year`")]
    [BuiltinNativeFunction("SETDATE")]
    public static DateOnly SetDate(int day, int month, int year)
    {
        return new DateOnly(year, month, day);
    }

    [Documentation("Returns the date of today")]
    [BuiltinNativeFunction("TODAY")]
    public static DateOnly Today() => DateOnly.FromDateTime(DateTime.Today);

    [Documentation("Returns the integer component of a `REAL` number")]
    [BuiltinNativeFunction("INT")]
    public static int Int(RealNumberType x)
    {
        return (int)x;
    }

    [Documentation("Returns the index of the first occasion of the character in the string")]
    [BuiltinNativeFunction("FIND")]
    public static int Find(string str, char chr)
    {
        return str.IndexOf(chr) + 1;
    }

    [Documentation("Returns the index of the first occasion of the character in the string starting from `index`")]
    [BuiltinNativeFunction("INSTR")]
    public static int InString(int index, string str, char chr)
    {
        return Find(str[(index - 1)..], chr) + index - 1;
    }

    [Documentation("Returns a random number [0..x)")]
    [BuiltinNativeFunction("RAND")]
    public static RealNumberType RandomReal(int x)
    {
        // ReSharper disable once RedundantCast
        return (RealNumberType)(Random.NextDouble() * x);
    }

    [Documentation("**NONSTANDARD** Inserts an item to the set")]
    [BuiltinFunction("SET_INSERT")]
    [ParamType("set", "ANY", isSet: true)]
    [ParamType("item", "ANY")]
    public static Instance SetInsert(Scope parentScope, PseudoProgram program, Instance[] arguments)
    {
        var setInstance = arguments[0];
        var set = setInstance.Get<HashSet<Instance>>();
        var item = SetCheckAndCast(setInstance, arguments[1]);
        set.Add(item);
        return Instance.Null;
    }

    [Documentation("**NONSTANDARD** Removes an item from the set")]
    [BuiltinFunction("SET_REMOVE")]
    [ParamType("set", "ANY", isSet: true)]
    [ParamType("item", "ANY")]
    public static Instance SetRemove(Scope parentScope, PseudoProgram program, Instance[] arguments)
    {
        var setInstance = arguments[0];
        var set = setInstance.Get<HashSet<Instance>>();
        var item = SetCheckAndCast(setInstance, arguments[1]);
        set.Remove(item);
        return Instance.Null;
    }

    [Documentation("**NONSTANDARD** Checks if the set contains the item")]
    [BuiltinFunction("SET_CONTAINS")]
    [ParamType("set", "ANY", isSet: true)]
    [ParamType("item", "ANY")]
    [ReturnType("BOOLEAN")]
    public static Instance SetContains(Scope parentScope, PseudoProgram program, Instance[] arguments)
    {
        var setInstance = arguments[0];
        var set = setInstance.Get<HashSet<Instance>>();
        var item = SetCheckAndCast(setInstance, arguments[1]);
        var res = set.Contains(item);
        return parentScope.FindDefinition(Type.BooleanId).Type.Instance(res);
    }

    [Documentation(
        "**NONSTANDARD** Checks if the set contains the item. If the item is found, assign the item to `out`")]
    [BuiltinFunction("SET_TRY_GET")]
    [ParamType("set", "ANY", isSet: true)]
    [ParamType("item", "ANY")]
    [ParamType("out", "ANY", isReference: true)]
    [ReturnType("BOOLEAN")]
    public static Instance SetTryGet(Scope parentScope, PseudoProgram program, Instance[] arguments)
    {
        var setInstance = arguments[0];
        var set = setInstance.Get<HashSet<Instance>>();
        var item = SetCheckAndCast(setInstance, arguments[1]);
        var res = set.TryGetValue(item, out var outItem);
        if (res) arguments[2].Type.Assign(arguments[2], outItem);
        return parentScope.FindDefinition(Type.BooleanId).Type.Instance(res);
    }

    [Documentation("**NONSTANDARD** Only include items that are present in only one of the sets but not both")]
    [BuiltinFunction("SET_SYMMETRIC_DIFFERENCE")]
    [ParamType("targetSet", "ANY", isSet: true)]
    [ParamType("anotherSet", "ANY", isSet: true)]
    public static Instance SetSymmetricDifference(Scope parentScope, PseudoProgram program, Instance[] arguments)
    {
        var setInstance = arguments[0];
        var set = setInstance.Get<HashSet<Instance>>();
        var setType = (SetType)setInstance.Type;
        var set2 = arguments[1];
        set.SymmetricExceptWith(setType.HandledCastFrom(set2).Get<HashSet<Instance>>());
        return Instance.Null;
    }

    [Documentation("**NONSTANDARD** Only include items that are present in both sets in target set")]
    [BuiltinFunction("SET_INTERSECT")]
    [ParamType("targetSet", "ANY", isSet: true)]
    [ParamType("anotherSet", "ANY", isSet: true)]
    public static Instance SetIntersect(Scope parentScope, PseudoProgram program, Instance[] arguments)
    {
        var setInstance = arguments[0];
        var set = setInstance.Get<HashSet<Instance>>();
        var setType = (SetType)setInstance.Type;
        var set2 = arguments[1];
        set.IntersectWith(setType.HandledCastFrom(set2).Get<HashSet<Instance>>());
        return Instance.Null;
    }

    [Documentation("**NONSTANDARD** Adds items that are not in the target set but in the other set")]
    [BuiltinFunction("SET_UNION")]
    [ParamType("targetSet", "ANY", isSet: true)]
    [ParamType("anotherSet", "ANY", isSet: true)]
    public static Instance SetUnion(Scope parentScope, PseudoProgram program, Instance[] arguments)
    {
        var setInstance = arguments[0];
        var set = setInstance.Get<HashSet<Instance>>();
        var setType = (SetType)setInstance.Type;
        var set2 = arguments[1];
        set.UnionWith(setType.HandledCastFrom(set2).Get<HashSet<Instance>>());
        return Instance.Null;
    }

    [Documentation("**NONSTANDARD** Excludes items that are present in both sets")]
    [BuiltinFunction("SET_DIFFERENCE")]
    [ParamType("targetSet", "ANY", isSet: true)]
    [ParamType("anotherSet", "ANY", isSet: true)]
    public static Instance SetDifference(Scope parentScope, PseudoProgram program, Instance[] arguments)
    {
        var setInstance = arguments[0];
        var set = setInstance.Get<HashSet<Instance>>();
        var setType = (SetType)setInstance.Type;
        var set2 = arguments[1];
        set.ExceptWith(setType.HandledCastFrom(set2).Get<HashSet<Instance>>());
        return Instance.Null;
    }

    private static Instance SetCheckAndCast(Instance setInstance, Instance item)
    {
        var setType = (SetType)setInstance.Type;
        if (setType.ElementType != item.Type)
        {
            if (setType.ElementType.IsConvertableFrom(item.Type))
                item = setType.ElementType.HandledCastFrom(item);
            else
                throw new UnsupportedCastError($"Cannot insert {item.Type} into {setType}", null);
        }

        return item;
    }
}