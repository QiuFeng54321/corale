using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Types;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Operations;

public class FormImmediateSetOperation : Operation
{
    public int Length;
    public SetType SetType;

    public FormImmediateSetOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        HashSet<Instance> elements = new(Instance.SetComparer);
        for (var i = 0; i < Length; i++)
        {
            var instance = Program.RuntimeStack.Pop();
            elements.Add(instance);
        }

        var formedInstance = SetType.Instance(elements);
        Program.RuntimeStack.Push(formedInstance);
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        Type arrayElementType = null;
        for (var i = 0; i < Length; i++)
        {
            var type = Program.TypeCheckStack.Pop();
            arrayElementType = type.Type;
        }

        Program.TypeCheckStack.Push(new Definition(ParentScope, Program)
        {
            Type = SetType = new SetType(ParentScope, Program)
            {
                ElementType = arrayElementType
            },
            SourceRange = SourceRange,
            Attributes = Definition.Attribute.Immutable
        });
    }

    public override string ToPlainString()
    {
        return string.Format(strings.FormImmediateArrayOperation_ToPlainString, Length);
    }
}