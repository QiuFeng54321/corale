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
        BlockRef = mainFunction.AppendBasicBlock("entry");
        ctx.Builder.PositionAtEnd(BlockRef);
        foreach (var s in Statements) s.CodeGen(ctx, this);


        ctx.Builder.BuildRetVoid();
        return BlockRef;
    }

    public override void Format(PseudoFormatter formatter)
    {
        WriteStatements(formatter);
    }
}