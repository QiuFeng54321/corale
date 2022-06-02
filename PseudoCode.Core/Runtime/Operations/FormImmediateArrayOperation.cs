namespace PseudoCode.Core.Runtime.Operations;

public class FormImmediateArrayOperation : Operation
{
    public int Length;
    public ArrayType ArrayType;

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
        formedInstance.InitialiseFromList(elements.Select(e => ArrayType.ElementType.HandledCastFrom(e)));
        Program.RuntimeStack.Push(formedInstance);
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        Type arrayElementType = null;
        var overallLength = 0;
        for (var i = 0; i < Length; i++)
        {
            var type = Program.TypeCheckStack.Pop();
            if (type is ArrayType subArrayType)
            {
                arrayElementType = subArrayType.ElementType;
                overallLength += subArrayType.TotalElements;
            }
            else
            {
                arrayElementType = type;
                overallLength++;
            }
        }

        Program.TypeCheckStack.Push(ArrayType = new ArrayType(ParentScope, Program)
        {
            Dimensions = new List<Range> { new() { Start = 1, End = overallLength } },
            ElementType = arrayElementType
        });
    }

    public override string ToPlainString()
    {
        return string.Format(strings.FormImmediateArrayOperation_ToPlainString, Length);
    }
}