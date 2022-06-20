using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;
using PseudoCode.Core.Runtime.Types;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Reflection;

public class FunctionBinder
{
    public delegate Instance BuiltinFunction(Scope parentScope, PseudoProgram program, Instance[] arguments);

    public static Definition MakeDefinition(System.Type type, Scope parentScope, PseudoProgram program)
    {
        var methods = type.GetMethods();
        foreach (var methodInfo in methods)
        {
            if (methodInfo.GetCustomAttributes(typeof(BuiltinFunctionAttribute), true) is BuiltinFunctionAttribute[]
                    attributes && attributes.Length != 0)
            {
                var attr = attributes[0];
                return new Definition(parentScope, program)
                {
                    Name = methodInfo.Name,
                    References = new List<SourceRange>(),
                    SourceRange = SourceRange.Identity,
                    Type = new BuiltinFunctionType(parentScope, program)
                    {
                        ParameterInfos = attr.ParameterInfos,
                        ReturnType = attr.ReturnType
                    }
                };
            }
            else
            {
            }
        }

        return null;
    }
}