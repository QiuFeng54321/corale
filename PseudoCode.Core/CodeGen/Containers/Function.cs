using LLVMSharp.Interop;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen.Containers;

public class Function : Statement
{
    public List<Symbol> Arguments;
    public Block Block;
    public Namespace BodyNamespace;
    public CompilationUnit CompilationUnit;
    public LLVMBasicBlockRef CurrentBlockRef;
    public bool IsExtern;
    public LLVMValueRef LLVMFunction;
    public string Name;
    public Function ParentFunction;
    public Namespace ParentNamespace;
    public Symbol ResultFunction;
    public Symbol ResultFunctionGroup;
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
            BodyNamespace ??= ParentNamespace.AddNamespace(ResultFunction.Name);
            if (Block == null) CreateBlock("entry");
            CurrentBlockRef = LLVMFunction.AppendBasicBlock("entry");
            ctx.Builder.PositionAtEnd(CurrentBlockRef);
        }

        for (uint index = 0; index < Arguments.Count; index++)
        {
            var argument = Arguments[(int)index];
            LLVMValueRef paramValue = LLVM.GetParam(LLVMFunction, index);
            LLVM.SetValueName(paramValue, argument.Name.ToSByte());
            if (!IsExtern)
            {
                var symbol = new Symbol(argument.Name, false, argument.Type);
                if (argument.DefinitionAttribute.HasFlag(DefinitionAttribute.Reference))
                    symbol.MemoryPointer = paramValue;
                else
                    symbol.ValueRef = paramValue;
                BodyNamespace.AddSymbol(symbol);
            }
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
        // Make parameter list
        var llvmParamTypes = new List<LLVMTypeRef>();
        foreach (var argument in Arguments)
        {
            var llvmTypeRef = argument.Type.GetLLVMType();
            if (argument.DefinitionAttribute.HasFlag(DefinitionAttribute.Reference))
                llvmTypeRef = LLVMTypeRef.CreatePointer(llvmTypeRef, 0);
            llvmParamTypes.Add(llvmTypeRef);
        }

        // Make function type
        var functionType = LLVMTypeRef.CreateFunction(ReturnType.Type.GetLLVMType(),
            llvmParamTypes.ToArray());
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
        // Add to function group
        if (!ParentNamespace.TryGetSymbol(Name, out var functionOverloadsSymbol))
        {
            functionOverloadsSymbol = new Symbol(Name, false, pseudoFunctionType);
            ParentNamespace.AddSymbol(functionOverloadsSymbol);
        }

        functionOverloadsSymbol.FunctionOverloads.Add(ResultFunction);
        ResultFunctionGroup = functionOverloadsSymbol;
    }

    public Block CreateBlock(string name, Namespace ns = null)
    {
        Block = new Block
        {
            Name = name,
            Namespace = ns ?? BodyNamespace,
            ParentFunction = this
        };
        return Block;
    }

    public override void Format(PseudoFormatter formatter)
    {
        formatter.WriteStatement(
            $"FUNCTION {Name}({string.Join(", ", Arguments.Select(a => $"{a.Name} : {a.Type.TypeName}"))}) RETURNS {ReturnType?.Type.TypeName}");

        Block.Format(formatter);
        formatter.WriteStatement("ENDFUNCTION");
    }

    public override void CodeGen(CodeGenContext ctx, Function parentBlock)
    {
        if (LLVMFunction == null) GeneratePrototype(ctx);
        if (IsExtern) return;
        Block.ParentFunction = this;
        ctx.Builder.PositionAtEnd(CurrentBlockRef);
        Block.CodeGen(ctx, this);
        ctx.Builder.PositionAtEnd(CurrentBlockRef);
        ctx.Builder.BuildRetVoid();
        if (ParentFunction != null) ctx.Builder.PositionAtEnd(ParentFunction.CurrentBlockRef);
    }
}