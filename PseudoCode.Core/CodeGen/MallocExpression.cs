using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.CodeGen.TypeLookups;

namespace PseudoCode.Core.CodeGen;

public class MallocExpression : Expression
{
    private readonly DataType _dataType;
    private readonly Expression _sizeExpression;

    public MallocExpression(Expression sizeExpression, DataType dataType)
    {
        _sizeExpression = sizeExpression;
        _dataType = dataType;
    }

    public override Symbol CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        var sizeSym = _sizeExpression.CodeGen(ctx, cu, function);
        if (sizeSym.Type.Kind is not Types.Integer) throw new InvalidOperationException();
        var dataType = _dataType.Lookup(ctx, cu, function);
        var res = cu.Builder.BuildArrayMalloc(dataType.Type.GetLLVMType(), sizeSym.GetRealValueRef(ctx, cu),
            ctx.NameGenerator.Request(ReservedNames.Malloc));
        var resType = dataType.MakePointerType();
        return Symbol.MakeTemp(resType.Type, res);
    }

    public override string ToFormatString()
    {
        return $"MALLOC {_sizeExpression.ToFormatString()} FOR {_dataType}";
    }
}