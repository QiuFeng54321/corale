namespace PseudoCode.Runtime.Operations;

public class FormImmediateArrayOperation : Operation
{
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

        var formedInstance = ((ArrayType)ParentScope.FindType(Type.ArrayId)).Instance(
            new List<Range> { new() { Start = 1, End = elements.Count } }, elements.First().Type);
        formedInstance.InitialiseFromList(elements.Select(e => formedInstance.ElementType.HandledCastFrom(e)));
        Program.RuntimeStack.Push(formedInstance);
    }

    public override string ToPlainString()
    {
        return string.Format(strings.FormImmediateArrayOperation_ToPlainString, Length);
    }
}