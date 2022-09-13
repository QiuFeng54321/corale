using LLVMSharp.Interop;
using PseudoCode.Core.Runtime.Errors;

namespace PseudoCode.Core.CodeGen;

public class Symbol
{
    /// <summary>
    ///     Variable value
    /// </summary>
    public LLVMValueRef Alloca;

    public DefinitionAttribute DefinitionAttribute;

    /// <summary>
    ///     If the symbol is a function, this stores the overloads of the function
    /// </summary>
    public List<Symbol> FunctionOverloads = new();

    /// <summary>
    ///     Indicates whether the symbol is type-only
    /// </summary>
    public bool IsType;

    /// <summary>
    ///     The name of the symbol
    /// </summary>
    public string Name;

    /// <summary>
    ///     Namespace of this symbol
    /// </summary>
    public Namespace Namespace;

    /// <summary>
    ///     Type of the symbol. If the symbol is a set of overloads of a function, this is null
    /// </summary>
    public Type Type;

    /// <summary>
    ///     The LLVM value reference of the symbol.
    /// </summary>
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

    public Symbol GetRealValue(CodeGenContext ctx)
    {
        if (ValueRef != null) return this;
        return MakeTemp(Type, ctx.Builder.BuildLoad(Alloca, ctx.NameGenerator.Request(ReservedNames.Temp)));
    }

    /// <summary>
    ///     Generates a temporary symbol
    /// </summary>
    /// <param name="nameTemplate">The name to be generated from <see cref="NameGenerator" /></param>
    /// <param name="type">The type of the symbol</param>
    /// <param name="ctx">The context of the symbol. This is used to access <see cref="NameGenerator" /></param>
    /// <param name="value">The value of the symbol</param>
    /// <returns>The symbol generated</returns>
    public static Symbol MakeTemp(string nameTemplate, Type type, CodeGenContext ctx, LLVMValueRef value)
    {
        var sym = new Symbol(ctx.NameGenerator.Request(nameTemplate), false, type)
        {
            ValueRef = value
        };
        return sym;
    }

    /// <summary>
    ///     Generates a temporary symbol
    /// </summary>
    /// <param name="type">The type of the symbol</param>
    /// <param name="value">The value of the symbol</param>
    /// <returns>The symbol generated</returns>
    public static Symbol MakeTemp(Type type, LLVMValueRef value)
    {
        var sym = new Symbol(value.Name, false, type)
        {
            ValueRef = value
        };
        return sym;
    }

    /// <summary>
    ///     <seealso
    ///         cref="MakeTemp(string,PseudoCode.Core.CodeGen.Type,PseudoCode.Core.CodeGen.CodeGenContext,LLVMSharp.Interop.LLVMValueRef)" />
    /// </summary>
    /// <param name="nameTemplate"></param>
    /// <param name="typeName">The name of the type to lookup</param>
    /// <param name="ctx"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidTypeError">Type cannot be found</exception>
    public static Symbol MakeTemp(string nameTemplate, string typeName, CodeGenContext ctx, LLVMValueRef value)
    {
        if (!ctx.Root.Namespace.TryGetSymbol(typeName, out var sym)) throw new InvalidTypeError(typeName);

        return MakeTemp(nameTemplate, sym.Type, ctx, value);
    }

    /// <summary>
    ///     Generates a symbol for a primitive type.<br />
    /// </summary>
    /// <seealso cref="PseudoCode.Core.CodeGen.Type.MakePrimitiveType" />
    /// <param name="typeName">The name presented in pseudocode</param>
    /// <param name="type">The c# type</param>
    /// <returns>The symbol made</returns>
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