using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;
using PseudoCode.Core.Runtime.Types;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Reflection;

public static class BuiltinFunctions
{
    [BuiltinFunction("EOF")]
    [ParamType("path", "STRING")]
    [ReturnType("BOOLEAN")]
    public static Instance Eof(Scope parentScope, PseudoProgram program, Instance[] arguments)
    {
        var path = arguments[0].Get<string>();
        return parentScope.FindDefinition(Type.BooleanId).Type.Instance(program.OpenFiles[path].Eof());
    }

    [BuiltinFunction("RIGHT")]
    [ParamType("ThisString", "STRING")]
    [ParamType("x", "INTEGER")]
    [ReturnType("STRING")]
    public static Instance Right(Scope parentScope, PseudoProgram program, Instance[] arguments)
    {
        var thisString = arguments[0].Get<string>();
        var x = arguments[1].Get<int>();
        return parentScope.FindDefinition(Type.StringId).Type.Instance(thisString[^x..]);
    }
    [BuiltinFunction("LENGTH")]
    [ParamType("ThisString", "STRING")]
    [ReturnType("INTEGER")]
    public static Instance Length(Scope parentScope, PseudoProgram program, Instance[] arguments)
    {
        var thisString = arguments[0].Get<string>();
        return parentScope.FindDefinition(Type.IntegerId).Type.Instance(thisString.Length);
    }

    [BuiltinFunction("__in_range")]
    [ParamType("target", "INTEGER")]
    [ParamType("from", "INTEGER")]
    [ParamType("to", "INTEGER")]
    [ReturnType("BOOLEAN")]
    public static Instance InRange(Scope parentScope, PseudoProgram program, Instance[] arguments)
    {
        var target = arguments[0].Get<decimal>();
        var from = arguments[1].Get<decimal>();
        var to = arguments[2].Get<decimal>();
        return parentScope.FindDefinition(Type.BooleanId).Type.Instance(from <= target && target <= to);
    }

    public static bool InRange(decimal target, decimal from, decimal to)
    {
        return from <= target && target <= to;
    }
}