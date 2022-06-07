using PseudoCode.Core.Runtime.Instances;

namespace PseudoCode.Core.Runtime.Types;

public class TypeInfo
{
    public Type Type;
    public bool IsConstant;
    public bool IsReference;
    public bool IsConstantEvaluated;
    public SourceRange SourceRange;
    public Instance ConstantInstance;

    public override string ToString()
    {
        return $"{(IsConstant ? "CONST " : "")}{(IsReference ? "REF " : "")}{Type}";
    }
}