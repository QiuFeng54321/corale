using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Runtime.Errors;

namespace PseudoCode.Core.CodeGen;

public class MemberAccess : Expression
{
    public Expression Before;
    public string MemberName;

    public override Symbol CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        var parentVal = Before.CodeGen(ctx, cu, function);
        foreach (var member in parentVal.Type.Members)
            if (member.Name == MemberName)
            {
                var memberVal = cu.Builder.BuildStructGEP2(parentVal.Type.GetLLVMType(),
                    parentVal.GetPointerValueRef(ctx), (uint)member.TypeMemberIndex,
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