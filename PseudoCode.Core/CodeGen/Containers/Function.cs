using LLVMSharp.Interop;
using PseudoCode.Core.Analyzing;
using PseudoCode.Core.CodeGen.Operator;
using PseudoCode.Core.Formatting;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.CodeGen.Containers;

public class Function : Statement
{
    public List<Symbol> Arguments;
    public Block Block;
    public Namespace BodyNamespace;
    public CompilationUnit CompilationUnit;
    public LLVMBasicBlockRef CurrentBlockRef;
    public string FullQualifier;
    public LLVMTypeRef FunctionType;
    public bool IsExtern;
    public LLVMValueRef LLVMFunction;
    public string Name;
    public PseudoOperator Operator = PseudoOperator.None;
    public Function ParentFunction;
    public Namespace ParentNamespace;
    public Symbol ResultFunction;
    public Symbol ResultFunctionGroup;
    public Symbol ReturnType;

    public void GeneratePrototype(CodeGenContext ctx, CompilationUnit cu)
    {
        if (LLVMFunction != null) return;
        AddSymbol(ctx, cu);

        if (IsExtern)
        {
            LLVMFunction.Linkage = LLVMLinkage.LLVMExternalLinkage;
        }
        else
        {
            LLVMFunction.Linkage = LLVMLinkage.LLVMWeakAnyLinkage;
            MakeFunctionBodyBlock(cu);
        }

        for (uint index = 0; index < Arguments.Count; index++)
        {
            var argument = Arguments[(int)index];
            var paramValue = SetArgumentName(index, argument);
            if (!IsExtern)
            {
                AddArgumentToBodyNs(argument, paramValue);
            }
        }
    }

    private void AddArgumentToBodyNs(Symbol argument, LLVMValueRef paramValue)
    {
        var symbol = new Symbol(argument.Name, false, argument.Type);
        if (argument.DefinitionAttribute.HasFlag(DefinitionAttribute.Reference))
            symbol.MemoryPointer = paramValue;
        else
            symbol.ValueRef = paramValue;
        BodyNamespace.AddSymbol(symbol);
    }

    private unsafe LLVMValueRef SetArgumentName(uint index, Symbol argument)
    {
        LLVMValueRef paramValue = LLVM.GetParam(LLVMFunction, index);
        LLVM.SetValueName(paramValue, argument.Name.ToSByte());
        return paramValue;
    }

    private void MakeFunctionBodyBlock(CompilationUnit cu)
    {
        BodyNamespace ??= ParentNamespace.AddNamespace(ResultFunction.Name);
        if (Block == null) CreateBlock("entry");
        CurrentBlockRef = LLVMFunction.AppendBasicBlock("entry");
        cu.Builder.PositionAtEnd(CurrentBlockRef);
    }

    public void LinkToFunctionPointer(CodeGenContext ctx, IntPtr ptr)
    {
        LLVMFunction.Linkage = LLVMLinkage.LLVMExternalLinkage;
        ctx.Engine.AddGlobalMapping(LLVMFunction, ptr);
        IsExtern = true;
    }

    private unsafe void AddSymbol(CodeGenContext ctx, CompilationUnit cu)
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
        var returnType = ReturnType.Type.GetLLVMType();
        if (ReturnType.DefinitionAttribute.HasFlag(DefinitionAttribute.Reference))
            returnType = LLVMTypeRef.CreatePointer(returnType, 0);
        FunctionType = LLVMTypeRef.CreateFunction(returnType,
            llvmParamTypes.ToArray());
        FullQualifier = ParentNamespace.GetFullQualifier(Name);
        cu.MakeDIFunc(this);
        LLVMFunction = LLVM.AddFunction(cu.Module, FullQualifier.ToSByte(),
            FunctionType);

        var pseudoFunctionType = new Type
        {
            Arguments = Arguments,
            ReturnType = ReturnType,
            TypeName = Type.GetFunctionSignature(Arguments, ReturnType),
            Kind = Types.Function
        };
        pseudoFunctionType.SetLLVMType(FunctionType);
        ResultFunction = new Symbol(Name, false, pseudoFunctionType)
        {
            ValueRef = LLVMFunction
        };
        if (Operator != PseudoOperator.None)
            AddFuncToOperator(ctx.OperatorResolverMap, ResultFunction);
        else
            AddFuncToFuncGroup(pseudoFunctionType, ResultFunction);
    }

    private bool AddFuncToOperator(OperatorResolverMap resolverMap, Symbol resultFunction)
    {
        if (Operator is PseudoOperator.Cast)
        {
            ResultFunctionGroup = resolverMap.TypeOperatorResolver.Casters;
            return resolverMap.TypeOperatorResolver.Casters.AddOverload(resultFunction);
        }

        return resolverMap.TypeOperatorResolver.TryAddOperator(Operator, resultFunction, out ResultFunctionGroup);
    }

    private void AddFuncToFuncGroup(Type pseudoFunctionType, Symbol resultFunction)
    {
        // Add to function group
        if (!ParentNamespace.TryGetSymbol(Name, out var functionOverloadsSymbol))
        {
            functionOverloadsSymbol = Symbol.MakeFunctionGroupSymbol(Name, pseudoFunctionType);
            ParentNamespace.AddSymbol(functionOverloadsSymbol);
        }

        functionOverloadsSymbol.AddOverload(resultFunction);
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

    public override void CodeGen(CodeGenContext ctx, CompilationUnit cu, Function parentBlock)
    {
        if (LLVMFunction == null) GeneratePrototype(ctx, cu);
        if (IsExtern) return;
        // cu.MakeDIFunc(this);
        Block.ParentFunction = this;
        cu.Builder.PositionAtEnd(CurrentBlockRef);
        Block.CodeGen(ctx, cu, this);
        cu.Builder.PositionAtEnd(CurrentBlockRef);
        if (ReturnType.Type.Kind is Types.None) cu.Builder.BuildRetVoid();
        if (ParentFunction != null) cu.Builder.PositionAtEnd(ParentFunction.CurrentBlockRef);
        var status = LLVMFunction.VerifyFunction(LLVMVerifierFailureAction.LLVMPrintMessageAction);
        if (!status)
        {
            LLVMFunction.Dump();
            // LLVMFunction.DeleteFunction();
            ctx.Analysis.Feedbacks.Add(new Feedback
            {
                Message = "Function is not valid as LLVM function",
                // Severity = Feedback.SeverityType.Error,
                DebugInformation = DebugInformation
            });
        }
    }
}