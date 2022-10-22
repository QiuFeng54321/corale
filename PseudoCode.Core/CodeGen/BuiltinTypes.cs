using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Runtime.Reflection;
using PseudoCode.Core.Runtime.Reflection.Builtin;

namespace PseudoCode.Core.CodeGen;

public static class BuiltinTypes
{
    public static Symbol Integer, Real, CharPtr, Boolean, Char, Void, String, Date;

    public static void Initialize()
    {
        Char = Symbol.MakePrimitiveType("CHAR", typeof(char));
        CharPtr = Symbol.MakePrimitiveType("__CHARPTR", typeof(string));
        Integer = Symbol.MakePrimitiveType("INTEGER", typeof(int));
        Real = Symbol.MakePrimitiveType("REAL", typeof(double));
        Boolean = Symbol.MakePrimitiveType("BOOLEAN", typeof(bool));
        Void = Symbol.MakePrimitiveType("VOID", typeof(void));
    }

    public static void InitializeReflectedTypes(CodeGenContext ctx)
    {
        String = TypeBinder.GetTypeSymbolFromSystemType(ctx, typeof(PseudoStringStruct));
        Date = TypeBinder.GetTypeSymbolFromSystemType(ctx, typeof(PseudoDateStruct));
    }

    public static void AddBuiltinTypes(Namespace ns)
    {
        ns.AddSymbol(CharPtr);
        ns.AddSymbol(Integer);
        ns.AddSymbol(Real);
        ns.AddSymbol(Char);
        ns.AddSymbol(Boolean);
        ns.AddSymbol(Void);
        ns.AddSymbol(String);
        ns.AddSymbol(Date);
    }
}