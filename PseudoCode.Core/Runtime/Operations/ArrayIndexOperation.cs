using PseudoCode.Core.Analyzing;
using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Runtime.Operations;

public class ArrayIndexOperation : Operation
{
    public int IndexLength;
    public ArrayIndexOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var index = (ArrayInstance)Program.RuntimeStack.Pop();
        var instance = Program.RuntimeStack.Pop();
        // TODO: Merge with MetaOperate type
        Program.RuntimeStack.Push(instance.Type.Index(instance, index));
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        var access = (ArrayType)Program.TypeCheckStack.Pop();
        var accessed = Program.TypeCheckStack.Pop();
        if (accessed is not ArrayType accessedArray)
        {
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = $"Invalid type of array access: {accessed}",
                SourceRange = SourceRange
            });
            Program.TypeCheckStack.Push(new NullType(ParentScope, Program));
        }
        else
        {
            if (IndexLength < accessedArray.DimensionCount)
                Program.TypeCheckStack.Push(new ArrayType(ParentScope, Program)
                {
                    DimensionCount = accessedArray.DimensionCount - IndexLength,
                    ElementType = accessedArray.ElementType
                });
            else
            {
                Program.TypeCheckStack.Push(accessedArray.ElementType);
            }
        }
    }

    public override string ToPlainString()
    {
        return strings.ArrayIndexOperation_ToPlainString;
    }
}