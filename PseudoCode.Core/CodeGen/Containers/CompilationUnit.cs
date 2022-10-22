using LLVMSharp.Interop;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen.Containers;

public class CompilationUnit : Statement
{
    public readonly List<Function> Functions = new();
    public Function MainFunction;
    public Namespace Namespace;

    public Function MakeFunction(string name, List<Symbol> arguments, Symbol retType, bool isExtern = false,
        bool addToList = true)
    {
        var func = new Function
        {
            Name = name,
            Arguments = arguments,
            ReturnType = retType,
            CompilationUnit = this,
            ParentNamespace = Namespace,
            IsExtern = isExtern
        };
        if (addToList) Functions.Add(func);
        return func;
    }

    public void MakeMainFunction(CodeGenContext ctx)
    {
        MainFunction = MakeFunction(ReservedNames.Main, new List<Symbol>(), BuiltinTypes.Void);
        MainFunction.GeneratePrototype(ctx);
        MainFunction.LLVMFunction.Linkage = LLVMLinkage.LLVMExternalLinkage;
    }

    public override void Format(PseudoFormatter formatter)
    {
        foreach (var function in Functions) function.Format(formatter);
    }

    public override void CodeGen(CodeGenContext ctx, Function _)
    {
        foreach (var function in Functions) function.CodeGen(ctx, _);
    }
}