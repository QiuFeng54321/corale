using System.Reflection;
using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types;

public class BuiltinFunctionType : FunctionType
{
    public BuiltinFunctionType(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override Instance Instance(object value = null, Scope scope = null)
    {
        return DefaultInstance<BuiltinFunctionInstance>(value, scope);
    }

    public override Instance Call(FunctionInstance functionInstance, Instance[] args)
    {
        CheckArguments(args);
        try
        {
            return ((BuiltinFunctionInstance)functionInstance).Func(ParentScope, Program,
                args.Zip(ParameterInfos).Select((p, _) => p.Second.Type.CastFrom(p.First)).ToArray());
        }
        catch (TargetInvocationException e)
        {
            if (e.InnerException != null) throw e.InnerException;
            throw;
        }
    }
}