using System.Runtime.InteropServices;
using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Runtime;

namespace PseudoCode.Core.CodeGen;

public static class Extensions
{
    public static unsafe sbyte* ToSByte(this string str)
    {
        return (sbyte*)Marshal.StringToHGlobalAnsi(str);
    }

    public static T AddDebugInformation<T>(this T astNode, DebugInformation di) where T : WithDebugInformation
    {
        astNode.DebugInformation = di;
        return astNode;
    }

    public static T AddDebugInformation<T>(this T astNode, CompilationUnit cu, SourceRange full,
        SourceRange partial = default) where T : WithDebugInformation
    {
        return astNode.AddDebugInformation(new DebugInformation(cu, full, partial ?? full));
    }
}