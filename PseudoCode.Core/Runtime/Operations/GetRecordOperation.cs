using PseudoCode.Core.Analyzing;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Runtime.Operations;

public class GetRecordOperation : FileOperation
{
    public GetRecordOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var instance = Program.RuntimeStack.Pop();
        var path = PopPathAtRuntime();
        var got = Program.OpenFiles[path].Get();
        instance.Type.Assign(instance, got);
    }


    public override void MetaOperate()
    {
        base.Operate();
        var type = Program.TypeCheckStack.Pop();
        if (type is PlaceholderType)
        {
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = "PUTRECORD must not read value to a variable whose type is not specified",
                Severity = Feedback.SeverityType.Error,
                SourceRange = SourceRange
            });
        }
        PopAndCheckPath();
    }
}