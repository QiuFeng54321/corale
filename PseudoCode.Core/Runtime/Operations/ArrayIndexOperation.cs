using PseudoCode.Core.Analyzing;

namespace PseudoCode.Core.Runtime.Operations;

public class ArrayIndexOperation : Operation
{
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
        if (accessed is not ArrayType arrayTypeInfo)
        {
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = $"Invalid type of array access",
                SourceRange = SourceRange
            });
            Program.TypeCheckStack.Push(new NullType(ParentScope, Program));
        }
        else
        {
            if (access.Dimensions.Count < arrayTypeInfo.Dimensions.Count)
                Program.TypeCheckStack.Push(new ArrayType(ParentScope, Program)
                {
                    Dimensions = arrayTypeInfo.Dimensions.Skip(access.Dimensions.Count).ToList(),
                    ElementType = arrayTypeInfo.ElementType
                });
            else
            {
                Program.TypeCheckStack.Push(arrayTypeInfo.ElementType);
            }
        }
    }

    public override string ToPlainString()
    {
        return strings.ArrayIndexOperation_ToPlainString;
    }
}