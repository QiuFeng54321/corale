using PseudoCode.Core.CodeGen;
using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.CodeGen.SimpleStatements;
using PseudoCode.Core.Runtime.Reflection;
using PseudoCode.Core.Runtime.Reflection.Builtin;

namespace PseudoCode.Core.Parsing;

public class PseudoCodeCompiler
{
    public readonly string EntryPath;

    public CodeGenContext Context;

    public PseudoCodeCompiler(string entryPath)
    {
        EntryPath = entryPath;
        Initialize();
    }

    public void Initialize()
    {
        BuiltinTypes.Initialize();
        Context = new CodeGenContext();
        Context.PseudoCodeCompiler = this;
        Context.MakeMainCompilationUnit(EntryPath);
        BuiltinTypes.InitializeReflectedTypes(Context);
        // Context.Builder.PositionAtEnd(CurrentBlock);
        BuiltinTypes.AddBuiltinTypes(Context.GlobalNamespace);
        FunctionBinder.MakeFromType(Context, typeof(BuiltinFunctions));
        FunctionBinder.MakeFromType(Context, typeof(Printer));
        FunctionBinder.MakeFromType(Context, typeof(Scanner));
        OutputStatement.MakeConstants();
        InputStatement.MakeConstants();
    }

    public void Compile()
    {
        CompileFile(Context.MainCompilationUnit);
    }

    public void CompileFile(CompilationUnit compilationUnit)
    {
        var compiler = new PseudoFileCompiler
        {
            PseudoCodeCompiler = this,
            CompilationUnit = compilationUnit
        };
        compiler.Compile();
    }
}