using PseudoCode.Core.Analyzing;

namespace PseudoCode.Core.Runtime.Operations;

public class PutRecordOperation : FileOperation
{
    public PutRecordOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var instance = Program.RuntimeStack.Pop();
        var path = PopPathAtRuntime();
        Program.OpenFiles[path].Put(instance);
    }

    public override void MetaOperate()
    {
        base.Operate();
        var type = Program.TypeCheckStack.Pop();
        if (!type.Serializable)
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = $"Cannot serialize {type}",
                Severity = Feedback.SeverityType.Error,
                SourceRange = SourceRange
            });
        PopAndCheckPath();
    }
}