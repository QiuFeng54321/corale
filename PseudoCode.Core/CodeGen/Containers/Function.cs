using LLVMSharp.Interop;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen.Containers;

public class Function : Statement
{
    public readonly List<Block> Blocks = new();
    public List<Symbol> Arguments;
    public Namespace BodyNamespace;
    public CompilationUnit CompilationUnit;
    public bool IsExtern;
    public LLVMValueRef LLVMFunction;
    public string Name;
    public Namespace ParentNamespace;
    public Symbol ResultFunction;
    public Symbol ReturnType;

    public unsafe void GeneratePrototype(CodeGenContext ctx)
    {
        if (LLVMFunction != null) return;
        AddSymbol(ctx);

        if (IsExtern)
        {
            LLVMFunction.Linkage = LLVMLinkage.LLVMExternalLinkage;
        }
        else
        {
            BodyNamespace = ParentNamespace.AddNamespace(ResultFunction.Name);
            var entry = AddBlock("entry");
            ctx.Builder.PositionAtEnd(entry.BlockRef);
        }

        for (uint index = 0; index < Arguments.Count; index++)
        {
            var argument = Arguments[(int)index];
            LLVMValueRef paramValue = LLVM.GetParam(LLVMFunction, index);
            LLVM.SetValueName(paramValue, argument.Name.ToSByte());
            if (!IsExtern)
                ParentNamespace.AddSymbol(new Symbol(argument.Name, false, argument.Type)
                {
                    ValueRef = paramValue
                });
        }
    }

    public void LinkToFunctionPointer(CodeGenContext ctx, IntPtr ptr)
    {
        LLVMFunction.Linkage = LLVMLinkage.LLVMExternalLinkage;
        ctx.Engine.AddGlobalMapping(LLVMFunction, ptr);
        IsExtern = true;
    }

    private unsafe void AddSymbol(CodeGenContext ctx)
    {
        var functionType = LLVMTypeRef.CreateFunction(ReturnType.Type.GetLLVMType(),
            Arguments.Select(a => a.Type.GetLLVMType()).ToArray());
        LLVMFunction = LLVM.AddFunction(ctx.Module, ParentNamespace.GetFullQualifier(Name).ToSByte(),
            functionType);

        var pseudoFunctionType = new Type
        {
            Arguments = Arguments,
            ReturnType = ReturnType.Type,
            TypeName = Type.GenerateFunctionTypeName(Arguments, ReturnType.Type),
            Kind = Types.Function
        };
        pseudoFunctionType.SetLLVMType(functionType);
        ResultFunction = new Symbol(Name, false, pseudoFunctionType)
        {
            ValueRef = LLVMFunction
        };
        if (!ParentNamespace.TryGetSymbol(Name, out var functionOverloadsSymbol))
        {
            functionOverloadsSymbol = new Symbol(Name, false, pseudoFunctionType);
            ParentNamespace.AddSymbol(functionOverloadsSymbol);
        }

        functionOverloadsSymbol.FunctionOverloads.Add(ResultFunction);
    }

    public Block AddBlock(string name, Namespace ns = null)
    {
        var block = new Block
        {
            Name = name,
            Namespace = ns ?? BodyNamespace,
            ParentFunction = this
        };
        block.InitializeBlock();
        Blocks.Add(block);
        return block;
    }

    public override void Format(PseudoFormatter formatter)
    {
        formatter.WriteStatement(
            $"FUNCTION {Name}({string.Join(", ", Arguments.Select(a => $"{a.Name} : {a.Type.TypeName}"))}) RETURNS {ReturnType?.Type.TypeName}");

        foreach (var block in Blocks) block.Format(formatter);
        formatter.WriteStatement("ENDFUNCTION");
    }

    public override void CodeGen(CodeGenContext ctx, Block _)
    {
        if (LLVMFunction == null) GeneratePrototype(ctx);
        if (IsExtern) return;
        foreach (var block in Blocks) block.CodeGen(ctx, _);

        ctx.Builder.BuildRetVoid();
    }
}