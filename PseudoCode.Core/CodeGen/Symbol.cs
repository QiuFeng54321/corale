using LLVMSharp.Interop;

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
        Namespace.AddSymbol(typeName, res);
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