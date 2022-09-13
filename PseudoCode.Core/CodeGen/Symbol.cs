using LLVMSharp.Interop;
using PseudoCode.Core.Runtime.Errors;

namespace PseudoCode.Core.CodeGen;

public class Symbol
{
    public DefinitionAttribute DefinitionAttribute;
    public bool IsType;
    public string Name;
    public Namespace Namespace;
    public Type Type;
    public LLVMValueRef ValueRef;

    public Symbol(string name, bool isType, Type type, Namespace ns = default,
        DefinitionAttribute definitionAttribute = DefinitionAttribute.None)
    {
        Name = name;
        IsType = isType;
        Type = type;
        DefinitionAttribute = definitionAttribute;
        Namespace = ns;
    }

    public static Symbol MakeTemp(string nameTemplate, Type type, CodeGenContext ctx, LLVMValueRef value)
    {
        var sym = new Symbol(ctx.NameGenerator.Request(nameTemplate), false, type)
        {
            ValueRef = value
        };
        return sym;
    }

    public static Symbol MakeTemp(string nameTemplate, string typeName, CodeGenContext ctx, LLVMValueRef value)
    {
        if (!ctx.Root.Namespace.TryGetSymbol(typeName, out var sym)) throw new InvalidTypeError(typeName);

        return MakeTemp(nameTemplate, sym.Type, ctx, value);
    }

    public static Symbol MakePrimitiveType(string typeName, System.Type type)
    {
        return new Symbol(typeName, true, Type.MakePrimitiveType(typeName, type));
    }

    /// <summary>
    ///     Fill the generic arguments (kinda like making types from template)
    /// </summary>
    /// <param name="genericArguments">The arguments to fill</param>
    /// <returns>The cloned type with generic types and fields filled in</returns>
    public Symbol FillGeneric(List<Symbol> genericArguments)
    {
        if (Type.GenericMembers == null) return this; // Nothing to fill
        var typeName = Type.GenerateFilledGenericTypeName(Type, genericArguments);
        if (Namespace.TryGetSymbol(typeName, out var existingSymbol)) return existingSymbol;
        var res = Clone();
        Type.FillGeneric(res.Type, Type, genericArguments);
        res.Type.TypeName = typeName;
        Namespace.AddSymbol(res);
        return res;
    }

    public Symbol Clone()
    {
        return new Symbol(Name, IsType, Type?.Clone(), Namespace, DefinitionAttribute);
    }

    protected bool Equals(Symbol other)
    {
        return DefinitionAttribute == other.DefinitionAttribute && IsType == other.IsType && Name == other.Name &&
               Equals(Type, other.Type) && Equals(Namespace, other.Namespace);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Symbol)obj);
    }

    public static bool operator ==(Symbol left, Symbol right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Symbol left, Symbol right)
    {
        return !Equals(left, right);
    }
}