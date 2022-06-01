namespace PseudoCode.Core.Runtime.Operations;

public class ForOperation : Operation
{
    public Operation TargetValue;
    public Operation Step;
    public Operation Next;
    public Operation ForBody;

    public ForOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        // Initial Stack: [ref instance id]
        TargetValue.HandledOperate(); // [ref instance id, ref instance targ_val]
        var targetValue = Program.RuntimeStack.Pop();
        var incrementValue = Program.RuntimeStack.Pop();
        // Stack empty
        while (incrementValue.Type.SmallerEqual(incrementValue, targetValue).Get<bool>())
        {
            ForBody.HandledOperate();
            Next.HandledOperate(); // [ref instance next]
            Next.HandledOperate(); // [ref instance next, ref instance next]
            Step.HandledOperate(); // [ref instance next, ref instance next, instance step]

            new BinaryOperation(ParentScope, Program)
            {
                OperatorMethod = PseudoCodeLexer.Add,
                PoiLocation = Step.PoiLocation
            }.HandledOperate(); // [ref instance next, instance next+step]

            new AssignmentOperation(ParentScope, Program)
            {
                PoiLocation = TargetValue.PoiLocation
            }.HandledOperate(); // Stack empty
        }
    }

    public override string ToPlainString() => "For";

    public override string ToString(int depth)
    {
        return string.Format(strings.ForOperation_ToString, Indent(depth), Step.ToString(depth),
            Indent(depth), TargetValue.ToString(depth), Indent(depth), ForBody.ToString(depth));
    }
}