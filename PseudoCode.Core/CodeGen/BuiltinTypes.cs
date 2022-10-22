namespace PseudoCode.Core.CodeGen;

public static class BuiltinTypes
{
    public static Symbol Integer, Real, CharPtr, Boolean, Char, Void;

    public static void Initialize()
    {
        Char = Symbol.MakePrimitiveType("CHAR", typeof(char));
        CharPtr = Symbol.MakePrimitiveType("__CHARPTR", typeof(string));
        Integer = Symbol.MakePrimitiveType("INTEGER", typeof(int));
        Real = Symbol.MakePrimitiveType("REAL", typeof(double));
        Boolean = Symbol.MakePrimitiveType("BOOLEAN", typeof(bool));
        Void = Symbol.MakePrimitiveType("VOID", typeof(void));
    }

    public static void AddBuiltinTypes(Namespace ns)
    {
        ns.AddSymbol(CharPtr);
        ns.AddSymbol(Integer);
        ns.AddSymbol(Real);
        ns.AddSymbol(Char);
        ns.AddSymbol(Boolean);
        ns.AddSymbol(Void);
    }
}