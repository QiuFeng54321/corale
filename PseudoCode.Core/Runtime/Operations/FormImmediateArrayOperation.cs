using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Types;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Operations;

public class FormImmediateArrayOperation : Operation
{
    public ArrayType ArrayType;
    public int Length;

    public FormImmediateArrayOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        List<Instance> elements = new();
        for (var i = 0; i < Length; i++)
        {
            var instance = Program.RuntimeStack.Pop();
            if (instance is ArrayInstance arrayInstance)
                elements.InsertRange(0, arrayInstance.Array);
            else
                elements.Insert(0, instance);
        }

        var formedInstance = (ArrayInstance)ArrayType.Instance();
        formedInstance.Dimensions = new List<Range> { new() { Start = 1, End = elements.Count } };
        formedInstance.InitialiseFromList(elements.Select(e => ArrayType.ElementType.HandledCastFrom(e)));
        Program.RuntimeStack.Push(formedInstance);
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        Type arrayElementType = null;
        for (var i = 0; i < Length; i++)
        {
            var type = Program.TypeCheckStack.Pop();
            if (type.Type is ArrayType subArrayType)
                arrayElementType = subArrayType.ElementType;
            else
                arrayElementType = type.Type;
        }

        Program.TypeCheckStack.Push(new TypeInfo
        {
            Type = ArrayType = new ArrayType(ParentScope, Program)
            {
                DimensionCount = 1,
                ElementType = arrayElementType
            },
            SourceRange = SourceRange
        });
    }

    public override string ToPlainString()
    {
        return string.Format(strings.FormImmediateArrayOperation_ToPlainString, Length);
    }
}