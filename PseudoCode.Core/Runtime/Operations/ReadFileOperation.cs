using PseudoCode.Core.Analyzing;
using PseudoCode.Core.Runtime.Types;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Operations;

public class ReadFileOperation : FileOperation
{
    public ReadFileOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var instance = Program.RuntimeStack.Pop();
        var stringType = Program.FindDefinition(Type.StringId).Type;
        var path = PopPathAtRuntime();
        var str = Program.OpenFiles[path].ReadLine();
        var strInstance = stringType.Instance(str);
        instance.Type.Assign(instance, strInstance);
    }

    public override void MetaOperate()
    {
        base.Operate();
        var valDef = Program.TypeCheckStack.Pop();

        var stringDef = Program.FindDefinition(Type.StringId);
        if (valDef.Type is PlaceholderType placeholderType) placeholderType.MetaAssign(valDef, stringDef);

        if (!valDef.Type.IsConvertableFrom(stringDef.Type))
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = $"READFILE variable is of type {valDef} and is not convertable from {stringDef}",
                Severity = Feedback.SeverityType.Error,
                SourceRange = SourceRange
            });
        PopAndCheckPath();
    }

    public override string ToPlainString()
    {
        return "Read line";
    }
}