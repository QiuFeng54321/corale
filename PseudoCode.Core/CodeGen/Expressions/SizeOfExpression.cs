using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.CodeGen.TypeLookups;

namespace PseudoCode.Core.CodeGen.Expressions;

public class SizeOfExpression : Expression
{
    private readonly DataType _dataType;

    public SizeOfExpression(DataType dataType)
    {
        _dataType = dataType;
    }

    public override Symbol CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        var sizeVal = _dataType.Lookup(ctx, cu, function).Type.GetLLVMType().SizeOf;
        return Symbol.MakeTemp(BuiltinTypes.Integer.Type, sizeVal);
    }

    public override string ToFormatString()
    {
        return $"SIZEOF {_dataType}";
    }
}