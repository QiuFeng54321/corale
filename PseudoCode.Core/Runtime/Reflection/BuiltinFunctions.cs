using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;
using PseudoCode.Core.Runtime.Types;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Reflection;

public static class BuiltinFunctions
{
    public static Random Random = new Random();
    [BuiltinFunction("EOF")]
    [ParamType("path", "STRING")]
    [ReturnType("BOOLEAN")]
    public static Instance Eof(Scope parentScope, PseudoProgram program, Instance[] arguments)
    {
        var path = arguments[0].Get<string>();
        return parentScope.FindDefinition(Type.BooleanId).Type.Instance(program.OpenFiles[path].Eof());
    }

    [BuiltinNativeFunction("RIGHT")]
    public static string Right(string thisString, int x)
    {
        return thisString[^x..];
    }

    [BuiltinNativeFunction("LENGTH")]
    public static int Length(string thisString)
    {
        return thisString.Length;
    }

    [BuiltinNativeFunction("__in_range")]
    public static bool InRange(RealNumberType target, RealNumberType from, RealNumberType to)
    {
        return from <= target && target <= to;
    }

    [BuiltinNativeFunction("MID")]
    public static string Mid(string thisString, int x, int y)
    {
        return thisString.Substring(x - 1, y);
    }

    [BuiltinNativeFunction("LCASE")]
    public static char LowerCase(char thisChar)
    {
        return char.ToLower(thisChar);
    }
    [BuiltinNativeFunction("UCASE")]
    public static char UpperCase(char thisChar)
    {
        return char.ToUpper(thisChar);
    }

    [BuiltinNativeFunction("INT")]
    public static int Int(RealNumberType x) => (int)x;

    [BuiltinNativeFunction("RAND")]
    public static RealNumberType RandomReal(int x)
    {
        // ReSharper disable once RedundantCast
        return (RealNumberType)(Random.NextDouble() * x);
    }
}