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

    public static void AddBuiltinTypes(Block block)
    {
        block.Namespace.AddSymbol(CharPtr);
        block.Namespace.AddSymbol(Integer);
        block.Namespace.AddSymbol(Real);
        block.Namespace.AddSymbol(Char);
        block.Namespace.AddSymbol(Boolean);
    }
}