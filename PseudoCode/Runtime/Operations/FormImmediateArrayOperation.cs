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
            var instance = ParentScope.RuntimeStack.Pop();
            if (instance is ArrayInstance arrayInstance)
            {
                elements.InsertRange(0, arrayInstance.Array);
            }
            else
            {
                elements.Insert(0, instance);
            }
        }

        var formedInstance = ((ArrayType)ParentScope.FindType("ARRAY")).Instance(
            new List<Range> { new() { Start = 0, End = elements.Count - 1 } }, elements.First().Type);
        formedInstance.InitialiseFromList(elements);
        ParentScope.RuntimeStack.Push(formedInstance);
    }

    public override string ToPlainString()
    {
        return $"Immediate array {Length}";
    }
}