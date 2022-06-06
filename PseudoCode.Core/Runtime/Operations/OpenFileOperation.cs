namespace PseudoCode.Core.Runtime.Operations;

public class OpenFileOperation : FileOperation
{
    public FileAccess FileAccess;
    public FileMode FileMode;
    public bool IsRandom;

    public OpenFileOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var path = PopPathAtRuntime();
        var stream = new PseudoFileStream(path, FileMode, FileAccess, IsRandom);
        stream.Open();
        if (Program.OpenFiles.ContainsKey(path)) Program.OpenFiles.Remove(path);
        Program.OpenFiles.Add(path, stream);
    }

    public override void MetaOperate()
    {
        base.Operate();
        PopAndCheckPath();
    }
}