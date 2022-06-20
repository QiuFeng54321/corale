using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;
using PseudoCode.Core.Runtime.Types;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Reflection;

public static class BuiltinFunctions
{
    public static Instance EOF(Scope parentScope, PseudoProgram program, Instance[] arguments)
    {
        var path = arguments[0].Get<string>();
        return parentScope.FindDefinition(Type.BooleanId).Type.Instance(program.OpenFiles[path].Eof());
    }

    public static bool InRange(decimal target, decimal from, decimal to)
    {
        return from <= target && target <= to;
    }
}