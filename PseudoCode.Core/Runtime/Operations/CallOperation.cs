using PseudoCode.Core.Analyzing;
using PseudoCode.Core.Runtime.Errors;
using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Types;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Operations;

public class CallOperation : Operation
{
    public int ArgumentCount;

    public CallOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var arguments = new List<Instance>();
        for (var i = 0; i < ArgumentCount; i++) arguments.Insert(0, Program.RuntimeStack.Pop());

        var called = Program.RuntimeStack.Pop().RealInstance;
        if (called is not FunctionInstance functionInstance)
            throw new InvalidTypeError($"Cannot call {called.Type}", this);
        var ret = functionInstance.Type.Call(functionInstance, arguments.ToArray());
        if (functionInstance.FunctionType.ReturnType != null)
            Program.RuntimeStack.Push(functionInstance.FunctionType.ReturnType.Type.CastFrom(ret));
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        var arguments = new List<Type>();
        for (var i = 0; i < ArgumentCount; i++) arguments.Insert(0, Program.TypeCheckStack.Pop().Type);
        var called = Program.TypeCheckStack.Pop();
        if (called.Type is not FunctionType functionType)
        {
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = $"The instance called is not a function but {called}",
                Severity = Feedback.SeverityType.Error,
                SourceRange = SourceRange
            });
            functionType = null;
        }
        else
        {
            if (arguments.Count != functionType.ParameterInfos.Length)
                Program.AnalyserFeedbacks.Add(new Feedback
                {
                    Message = $"Calling {functionType} with arguments: ({string.Join(", ", arguments)})",
                    Severity = Feedback.SeverityType.Error,
                    SourceRange = SourceRange
                });

            foreach (var ((parameterInfo, passedInstance), i) in functionType.ParameterInfos.Zip(arguments)
                         .Select((v, i) => (v, i)))
                if (!parameterInfo.Type.IsConvertableFrom(passedInstance))
                    Program.AnalyserFeedbacks.Add(new Feedback
                    {
                        Message =
                            $"Argument {i + 1} cannot be casted from {passedInstance} to {parameterInfo.TypeString()}",
                        Severity = Feedback.SeverityType.Error,
                        SourceRange = SourceRange
                    });
        }

        var ret = functionType?.ReturnType?.Type ?? new NullType(ParentScope, Program);
        if (ret is not NullType) Program.TypeCheckStack.Push(new Definition(ParentScope, Program) {
            Type = ret,
            SourceRange = SourceRange,
            Attributes = Definition.Attribute.Immutable
        });
    }

    public override string ToPlainString()
    {
        return $"Call {ArgumentCount}";
    }
}