using LLVMSharp.Interop;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen.Containers;

public class CompilationUnit : Statement
{
    public readonly string FilePath;
    public readonly List<Function> Functions = new();
    public readonly List<CompilationUnit> Imports = new();
    public readonly string ModuleName;
    public LLVMBuilderRef Builder;
    public LLVMDIBuilderRef DIBuilder;
    public Function MainFunction;
    public LLVMModuleRef Module;
    public Namespace Namespace;
    public CompilationUnit Parent;

    public CompilationUnit(CodeGenContext ctx, string path)
    {
        FilePath = path;
        ModuleName = Path.GetFileNameWithoutExtension(path);
        Module = LLVMModuleRef.CreateWithName(ModuleName);
        DIBuilder = Module.CreateDIBuilder();
        Builder = Module.Context.CreateBuilder();
        MakeMainFunction(ctx, ModuleName);
    }

    public CompilationUnit MakeSubUnit(CodeGenContext ctx, string path)
    {
        return new CompilationUnit(ctx, path)
        {
            Parent = this,
            Namespace = Namespace
        };
    }

    public string FindSubUnitPath(string givenPath)
    {
        if (Path.IsPathRooted(givenPath)) return givenPath;
        var concatPath = Path.Combine(Path.GetDirectoryName(FilePath)!, givenPath!);
        if (!File.Exists(concatPath)) throw new FileNotFoundException(concatPath);
        return concatPath;
    }

    public CompilationUnit ImportUnit(CodeGenContext ctx, string path)
    {
        var subUnit = MakeSubUnit(ctx, FindSubUnitPath(path));
        ctx.PseudoCodeCompiler.CompileFile(subUnit);
        Imports.Add(subUnit);
        ctx.Engine.AddModule(subUnit.Module);
        return subUnit;
    }

    public Function MakeFunction(string name, List<Symbol> arguments, Symbol retType, Namespace ns = default,
        bool isExtern = false,
        bool addToList = true)
    {
        var func = new Function
        {
            Name = name,
            Arguments = arguments,
            ReturnType = retType,
            CompilationUnit = this,
            ParentNamespace = ns ?? Namespace,
            IsExtern = isExtern
        };
        if (addToList) Functions.Add(func);
        return func;
    }

    public void MakeMainFunction(CodeGenContext ctx, string name)
    {
        MainFunction = MakeFunction(name, new List<Symbol>(), BuiltinTypes.Void);
        MainFunction.ParentNamespace = ctx.GlobalNamespace;
        MainFunction.GeneratePrototype(ctx, this);
        MainFunction.LLVMFunction.Linkage = LLVMLinkage.LLVMExternalLinkage;
    }

    public override void Format(PseudoFormatter formatter)
    {
        foreach (var function in Functions) function.Format(formatter);
    }

    public override unsafe void CodeGen(CodeGenContext ctx, CompilationUnit cu, Function _)
    {
        foreach (var function in Functions) function.CodeGen(ctx, cu, _);
        foreach (var import in Imports) LLVM.LinkModules2(Module, import.Module);
    }
}