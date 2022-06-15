using PseudoCode.Core.Analyzing;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Runtime.Operations;

public class MemberAccessOperation : Operation
{
    public string MemberName;
    public MemberAccessOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var accessed = Program.RuntimeStack.Pop();
        Program.RuntimeStack.Push(accessed.Type.MemberAccess(accessed, MemberName));
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        var accessed = Program.TypeCheckStack.Pop();
        var resultType = accessed.Type.MemberAccessResultType(MemberName);
        if (resultType is NullType)
        {
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = $"{accessed} does not have field '{MemberName}'",
                Severity = Feedback.SeverityType.Error,
                SourceRange = SourceRange
            });
        }
        Program.TypeCheckStack.Push(new TypeInfo {
            Type = resultType,
            IsReference = true,
            SourceRange = SourceRange
        });
    }

    public override string ToPlainString() => $"Access {MemberName}";
}