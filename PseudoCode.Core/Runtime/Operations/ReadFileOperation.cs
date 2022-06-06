using PseudoCode.Core.Analyzing;

namespace PseudoCode.Core.Runtime.Operations;

public class ReadFileOperation : Operation
{

    public ReadFileOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()                                                  
    {
        base.Operate();
        var instance = Program.RuntimeStack.Pop();
        var stringType = ParentScope.FindTypeDefinition(Type.StringId).Type;
        var pathInstance = stringType.CastFrom(Program.RuntimeStack.Pop());
        var path = pathInstance.Get<string>();                                          
        var str = Program.OpenFiles[path].ReadLine();
        var strInstance = stringType.Instance(str);
        instance.Type.Assign(instance, strInstance);
    }
    public override void MetaOperate()                                                  
    {
        base.Operate();
        var instance = Program.TypeCheckStack.Pop();
        var path = Program.TypeCheckStack.Pop();
        if (!ParentScope.FindTypeDefinition(Type.StringId).Type.IsConvertableFrom(path))
        {
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = $"File path cannot be {path}",
                Severity = Feedback.SeverityType.Error,
                SourceRange = SourceRange
            });
        }
    }
}