using LLVMSharp.Interop;

namespace PseudoCode.Core.CodeGen;

public class ProgramRoot : Block
{
    public LLVMBasicBlockRef GetBlock(CodeGenContext ctx)
    {
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
}