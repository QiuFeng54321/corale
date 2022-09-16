using LLVMSharp.Interop;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen;

public class ProgramRoot : Block
{
    public LLVMBasicBlockRef GetBlock(CodeGenContext ctx)
    {
        if (BlockRef != null) return BlockRef;
        var mainFunctionType = LLVMTypeRef.CreateFunction(LLVMTypeRef.Void, Array.Empty<LLVMTypeRef>());
        var mainFunction = ctx.Module.AddFunction(ReservedNames.Main, mainFunctionType);
        mainFunction.Linkage = LLVMLinkage.LLVMExternalLinkage;
        var bb = mainFunction.AppendBasicBlock("entry");
        ctx.Builder.PositionAtEnd(bb);
        Statements.ForEach(s => s.CodeGen(ctx, this));
        ctx.Builder.BuildRetVoid();
        BlockRef = bb;
        return bb;
    }

    public override void Format(PseudoFormatter formatter)
    {
        WriteStatements(formatter);
    }
}