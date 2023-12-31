namespace PseudoCode.Core.Runtime.Operations;

public class CloseFileOperation : FileOperation
{
    public CloseFileOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var path = PopPathAtRuntime();
        Program.OpenFiles[path].Close();
    }

    public override void MetaOperate()
    {
        base.Operate();
        PopAndCheckPath();
    }

    public override string ToPlainString()
    {
        return "Close file";
    }
}