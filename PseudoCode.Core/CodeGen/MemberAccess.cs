using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Runtime.Errors;

namespace PseudoCode.Core.CodeGen;

public class MemberAccess : Expression
{
    public Expression Before;
    public string MemberName;

    public override Symbol CodeGen(CodeGenContext ctx, Function function)
    {
        var parentVal = Before.CodeGen(ctx, function);
        foreach (var member in parentVal.Type.Members)
            if (member.Name == MemberName)
            {
                var memberVal = ctx.Builder.BuildStructGEP2(parentVal.Type.GetLLVMType(),
                    parentVal.GetPointerValueRef(), (uint)member.TypeMemberIndex,
                    ctx.NameGenerator.RequestTemp(MemberName));
                return Symbol.MakeTemp(member.Type, memberVal, true);
            }

        throw new InvalidAccessError(ToFormatString());
    }

    public override string ToFormatString()
    {
        return $"{Before.ToFormatString()}.{MemberName}";
    }
}