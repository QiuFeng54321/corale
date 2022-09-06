using LLVMSharp.Interop;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.CodeGen;

public class BinaryExpression : Expression
{
    public Expression Left, Right;
    public PseudoOperator Operator;

    public override LLVMValueRef CodeGen()
    {
        throw new NotImplementedException();
    }
}