using PseudoCode.Core.Analyzing;
using PseudoCode.Core.Runtime.Types;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Operations;

public class FileOperation : Operation
{
    public FileOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public string PopPathAtRuntime()
    {
        var stringType = Program.FindTypeDefinition(Type.StringId).Type;
        var pathInstance = stringType.CastFrom(Program.RuntimeStack.Pop());
        var path = pathInstance.Get<string>();
        return path;
    }

    public TypeInfo PopAndCheckPath()
    {
        var path = Program.TypeCheckStack.Pop();
        if (!Program.FindTypeDefinition(Type.StringId).Type.IsConvertableFrom(path.Type))
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = $"File path cannot be {path}",
                Severity = Feedback.SeverityType.Error,
                SourceRange = SourceRange
            });

        return path;
    }
}