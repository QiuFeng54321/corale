using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types;

public class ObjectType : Type
{
    public Definition InheritTypeDef;

    public ObjectType(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public Instance New(IEnumerable<Instance> arguments)
    {
        var instance = Instance(scope: ParentScope);
        var constructor = (FunctionInstance)MemberAccess(instance, "NEW");
        constructor.Type.Call(constructor, arguments.ToArray());
        return instance;
    }
}