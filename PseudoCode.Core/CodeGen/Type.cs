using LLVMSharp.Interop;

namespace PseudoCode.Core.CodeGen;

public class Type
{
    private LLVMTypeRef _llvmTypeRef;

    /// <summary>
    ///     If the type is a function, this stores the arguments.<br />
    ///     We need this to store symbols because we need to fill them, and we need to specify the attributes
    /// </summary>
    public Dictionary<string, Symbol> Arguments;

    /// <summary>
    ///     Stores where the type is declared
    /// </summary>
    public DebugInformation DebugInformation;

    /// <summary>
    ///     If the type is an array T[] or a pointer ^T, the ElementType is T.
    /// </summary>
    public Type ElementType;

    /// <summary>
    ///     If the type is a function FUNCTION <![CDATA[<A, B, C>]]> or type TYPE<![CDATA[<A, B, C>]]>
    ///     , then this stores the generic arguments.
    ///     The key is the name ("A", "B", "C"), each corresponding to a placeholder type (Types.GenericPlaceholder).
    ///     When a generic type is being un-generalized (e.g. using DECLARE abc : Type<![CDATA[<INTEGER>]]>),
    ///     The generic type will be copied, and this field will be filled with actual type specified (in that case
    ///     Types.Integer)
    /// </summary>
    public List<Symbol> GenericArguments;

    /// <summary>
    ///     If the type is being made from a generic type, this field stores which generic type it is made from
    /// </summary>
    public Type GenericFrom;

    /// <summary>
    ///     Lists of variables that relies on the generic arguments of this type
    /// </summary>
    public List<Symbol> GenericMembers;

    /// <summary>
    ///     When generating a type from template Type<![CDATA[<T>]]>
    ///     with a member of type T,
    ///     the member uses GenericParent.GenericArguments to search for type
    /// </summary>
    public Type GenericParent;

    /// <summary>
    ///     Specifies if the type is generic
    /// </summary>
    public bool IsGeneric;

    /// <summary>
    ///     Specifies what kind of type this is.
    ///     <seealso cref="Types" />
    /// </summary>
    public Types Kind;

    /// <summary>
    ///     If the type is a custom type, this stores the members in the type.<br />
    ///     We need this to store symbols because we need to fill them, and we need to specify the attributes
    /// </summary>
    public Dictionary<string, Symbol> Members;

    /// <summary>
    ///     Return type of the function
    /// </summary>
    public Type ReturnType;

    /// <summary>
    ///     The name of the type. NOT THE NAME OF THE VARIABLE/PARAMETER!!
    /// </summary>
    public string TypeName;

    public LLVMTypeRef GetLLVMType()
    {
        if (_llvmTypeRef != null) return _llvmTypeRef;
        throw new NotImplementedException();
    }

    public static Type MakePrimitiveType(string name, System.Type type)
    {
        LLVMTypeRef typeRef = null;
        if (type == typeof(int)) typeRef = LLVMTypeRef.Int32;
        if (type == typeof(string)) typeRef = LLVMTypeRef.CreatePointer(LLVMTypeRef.Int8, 0);

        return new Type
        {
            _llvmTypeRef = typeRef,
            TypeName = name
        };
    }

    public static Type MakeGenericPlaceholder(string name)
    {
        return new Type
        {
            TypeName = name,
            Kind = Types.GenericPlaceholder
        };
    }

    public Type Clone()
    {
        return new Type
        {
            _llvmTypeRef = _llvmTypeRef,
            Arguments = Arguments?.Clone(),
            DebugInformation = DebugInformation,
            ElementType = ElementType?.Clone(),
            GenericArguments = GenericArguments?.Clone(),
            GenericParent = GenericParent?.Clone(),
            Kind = Kind,
            Members = Members?.Clone(),
            ReturnType = ReturnType?.Clone(),
            TypeName = TypeName
        };
    }

    /// <summary>
    ///     Fill the generic arguments (kinda like making types from template)
    /// </summary>
    /// <param name="target">Type to fill (cloned)</param>
    /// <param name="genericParent">Type to fill from</param>
    /// <param name="genericArguments">The arguments to fill</param>
    /// <returns>The cloned type with generic types and fields filled in</returns>
    public static void FillGeneric(Type target, Type genericParent, List<Symbol> genericArguments)
    {
        foreach (var member in genericParent.GenericMembers)
            member.Type = genericArguments.FirstOrDefault(g => g.Name == member.Type.TypeName)?.Type;

        target.IsGeneric = false;
        target.GenericArguments = null;
        target.GenericMembers = null;
        target.GenericFrom = genericParent;
    }

    public static string GenerateFilledGenericTypeName(Type type, IEnumerable<Symbol> genericArguments)
    {
        return $"{type.TypeName}<{string.Join(",", genericArguments.Select(a => a.Type.TypeName))}>";
    }

    public override bool Equals(object obj)
    {
        if (obj is not Type t) return false;
        return t.TypeName == TypeName && t.Kind == Kind;
    }
}