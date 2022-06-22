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
        var access = (ArrayType)Program.TypeCheckStack.Pop().Type;
        var accessed = Program.TypeCheckStack.Pop();
        if (accessed.Type is not ArrayType accessedArray)
        {
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = $"Invalid type of array access: {accessed}",
                SourceRange = SourceRange
            });
            Program.TypeCheckStack.Push(new Definition(ParentScope, Program)
            {
                Type = new NullType(ParentScope, Program),
                Attributes = Definition.Attribute.Reference,
                SourceRange = SourceRange
            });
        }
        else
        {
            var attributes = accessed.Attributes.HasFlag(Definition.Attribute.Reference)
                ? Definition.Attribute.Reference
                : Definition.Attribute.Immutable;
            if (IndexLength < accessedArray.DimensionCount)
            {
                Program.TypeCheckStack.Push(new Definition(ParentScope, Program)
                {
                    Type = new ArrayType(ParentScope, Program)
                    {
                        DimensionCount = accessedArray.DimensionCount - IndexLength,
                        ElementType = accessedArray.ElementType
                    },
                    Attributes = attributes,
                    SourceRange = SourceRange
                });
            }
            else
                Program.TypeCheckStack.Push(new Definition(ParentScope, Program)
                {
                    Type = accessedArray.ElementType,
                    Attributes = attributes,
                    SourceRange = SourceRange
                });
        }
    }

    public override string ToPlainString()
    {
        return strings.ArrayIndexOperation_ToPlainString;
    }
}