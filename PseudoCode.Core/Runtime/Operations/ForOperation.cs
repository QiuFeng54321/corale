using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Runtime.Operations;

public class ForOperation : Operation
{
    public Operation ForBody;
    public Operation Next;
    public Operation Step;
    public Operation TargetValue;

    public ForOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public delegate Instance CompareFunc(Instance i1, Instance i2);
    public override void Operate()
    {
        base.Operate();
        // Initial Stack: [ref instance id]
        TargetValue.HandledOperate(); // [ref instance id, ref instance targ_val]
        var targetValue = Program.RuntimeStack.Pop();
        var incrementValue = Program.RuntimeStack.Pop();
        var smaller = incrementValue.Type.SmallerEqual(incrementValue, targetValue).Get<bool>();
        CompareFunc func = smaller ? incrementValue.Type.SmallerEqual : incrementValue.Type.GreaterEqual;
        // Stack empty
        while (func(incrementValue, targetValue).Get<bool>())
        {
            ForBody.HandledOperate();
            Next.HandledOperate(); // [ref instance next]
            Next.HandledOperate(); // [ref instance next, ref instance next]
            Step.HandledOperate(); // [ref instance next, ref instance next, instance step]

            new BinaryOperation(ParentScope, Program)
            {
                OperatorMethod = PseudoOperator.Add,
                PoiLocation = Step.PoiLocation,
                SourceRange = Step.SourceRange
            }.HandledOperate(); // [ref instance next, instance next+step]

            new AssignmentOperation(ParentScope, Program)
            {
                PoiLocation = TargetValue.PoiLocation,
                SourceRange = TargetValue.SourceRange
            }.HandledOperate(); // Stack empty
        }
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        TargetValue.MetaOperate();
        var targetValueType = Program.TypeCheckStack.Pop();
        var incrementValueType = Program.TypeCheckStack.Pop();
        ForBody.MetaOperate();
        Next.MetaOperate();
        Next.MetaOperate();
        Step.MetaOperate();
        new BinaryOperation(ParentScope, Program)
        {
            OperatorMethod = PseudoOperator.Add,
            PoiLocation = Step.PoiLocation,
            SourceRange = Step.SourceRange
        }.MetaOperate(); // [ref instance next, instance next+step]

        new AssignmentOperation(ParentScope, Program)
        {
            PoiLocation = TargetValue.PoiLocation,
            SourceRange = TargetValue.SourceRange
        }.MetaOperate();
    }

    public override string ToPlainString()
    {
        return "For";
    }

    public override string ToString(int depth)
    {
        return string.Format(strings.ForOperation_ToString, Indent(depth), Step.ToString(depth),
            Indent(depth), TargetValue.ToString(depth), Indent(depth), ForBody.ToString(depth));
    }
}