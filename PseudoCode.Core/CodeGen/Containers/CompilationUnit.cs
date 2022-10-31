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
    public LLVMMetadataRef CompileUnitMetadata;
    public LLVMDIBuilderRef DIBuilder;
    public LLVMMetadataRef FileMetadataRef;
    public Function MainFunction;
    public LLVMModuleRef Module;
    public Namespace Namespace;
    public CompilationUnit Parent;

    public CompilationUnit(CodeGenContext ctx, string path, CompilationUnit useParent = default)
    {
        FilePath = path;
        ModuleName = Path.GetFileNameWithoutExtension(path);
        if (useParent != null)
        {
            Module = useParent.Module;
            // DIBuilder = useParent.DIBuilder;
            Builder = useParent.Builder;
        }
        else
        {
            Module = LLVMModuleRef.CreateWithName(ModuleName);
            Builder = Module.Context.CreateBuilder();
        }

        DIBuilder = Module.CreateDIBuilder();
        FileMetadataRef = DIBuilder.CreateFile(FilePath, Path.GetDirectoryName(FilePath));
        CompileUnitMetadata = DIBuilder.CreateCompileUnit(LLVMDWARFSourceLanguage.LLVMDWARFSourceLanguageC,
            FileMetadataRef, "PseudoCode",
            0, "", 0, "", LLVMDWARFEmissionKind.LLVMDWARFEmissionFull, 0, 0, 0, "", "");

        MakeMainFunction(ctx, ModuleName);
    }

    public CompilationUnit MakeSubUnit(CodeGenContext ctx, string path, CompilationUnit useParent = default)
    {
        return new CompilationUnit(ctx, path, useParent)
        {
            Parent = this,
            Namespace = Namespace
        };
    }

    public string FindSubUnitPath(string givenPath)
    {
        if (Path.IsPathRooted(givenPath)) return givenPath;
        var concatPath = Path.Combine(Path.GetDirectoryName(FilePath)!, givenPath!);
        if (!File.Exists(concatPath)) return string.Empty;
        return concatPath;
    }

    public CompilationUnit ImportUnit(CodeGenContext ctx, string path)
    {
        // TODO: Make new modules
        if (Imports.FirstOrDefault(i => i.FilePath == path) is { } f) return f;
        var subUnitPath = FindSubUnitPath(path);
        if (string.IsNullOrEmpty(subUnitPath)) return null;
        var subUnit = MakeSubUnit(ctx, subUnitPath, this);
        ctx.PseudoCodeCompiler.CompileFile(subUnit);
        Imports.Add(subUnit);
        // ctx.Engine.AddModule(subUnit.Module);
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

    public LLVMMetadataRef MakeDIFunc(Function function)
    {
        if (function.DebugInformation == null) return null;
        var functionMetaType = DIBuilder.CreateSubroutineType(FileMetadataRef,
            ReadOnlySpan<LLVMMetadataRef>.Empty, LLVMDIFlags.LLVMDIFlagZero);
        var startLine = (uint)function.DebugInformation.FullSourceRange.Start.Line;
        var startColumn = (uint)function.DebugInformation.FullSourceRange.Start.Column;
        var func = DIBuilder.CreateFunction(FileMetadataRef, function.Name, function.FullQualifier, FileMetadataRef,
            startLine, functionMetaType, 1, 1,
            startLine, LLVMDIFlags.LLVMDIFlagZero, 0);
        var currentLine =
            Module.Context.CreateDebugLocation(startLine, startColumn, func, default);
        return currentLine;
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
        DIBuilder.DIBuilderFinalize();
        // foreach (var import in Imports) Console.WriteLine(LLVM.LinkModules2(Module, import.Module));
    }
}