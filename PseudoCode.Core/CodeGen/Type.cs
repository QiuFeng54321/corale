using LLVMSharp.Interop;
using PseudoCode.Core.Runtime.Errors;

namespace PseudoCode.Core.CodeGen;

public class Type
{
    private LLVMTypeRef _llvmTypeRef;

    /// <summary>
    ///     If the type is a function, this stores the arguments.<br />
    ///     We need this to store symbols because we need to fill them, and we need to specify the attributes
    /// </summary>
    public List<Symbol> Arguments;

    /// <summary>
    ///     Stores where the type is declared
    /// </summary>
    public DebugInformation DebugInformation;

    /// <summary>
    ///     If the type is an array T[] or a pointer ^T, the ElementType is T.
    /// </summary>
    public Type ElementType;

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

    public void SetLLVMType(LLVMTypeRef type)
    {
        _llvmTypeRef = type;
    }

    public LLVMTypeRef GetLLVMType()
    {
        if (_llvmTypeRef != null) return _llvmTypeRef;
        if (Kind == Types.Type)
        {
            var i = 0;
            List<LLVMTypeRef> llvmTypeMembers = new();
            foreach (var (key, sym) in Members)
            {
                sym.TypeMemberIndex = i++;
                llvmTypeMembers.Add(sym.Type.GetLLVMType());
            }

            _llvmTypeRef = LLVMTypeRef.CreateStruct(llvmTypeMembers.ToArray(), true);
            return _llvmTypeRef;
        }

        if (Kind == Types.GenericPlaceholder) throw new InvalidAccessError($"Generic placeholder for {TypeName}");

        throw new NotImplementedException();
    }

    public static Type MakePrimitiveType(string name, System.Type type)
    {
        var resType = new Type
        {
            TypeName = name
        };
        if (type == typeof(int))
        {
            resType._llvmTypeRef = LLVMTypeRef.Int32;
            resType.Kind = Types.Integer;
        }

        if (type == typeof(string))
        {
            resType._llvmTypeRef = LLVMTypeRef.CreatePointer(LLVMTypeRef.Int8, 0);
            resType.Kind = Types.Pointer;
            resType.ElementType = BuiltinTypes.Char.Type;
        }

        if (type == typeof(double))
        {
            resType._llvmTypeRef = LLVMTypeRef.Double;
            resType.Kind = Types.Real;
        }

        if (type == typeof(bool))
        {
            resType._llvmTypeRef = LLVMTypeRef.Int1;
            resType.Kind = Types.Boolean;
        }

        if (type == typeof(char))
        {
            resType._llvmTypeRef = LLVMTypeRef.Int8;
            resType.Kind = Types.Character;
        }

        if (type == typeof(void))
        {
            resType._llvmTypeRef = LLVMTypeRef.Void;
            resType.Kind = Types.None;
        }

        return resType;
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
            Kind = Kind,
            Members = Members?.Clone(),
            ReturnType = ReturnType?.Clone(),
            TypeName = TypeName
        };
    }


    public static void FillGeneric(out Type genericPlaceholder, ref Type targetType)
    {
        genericPlaceholder = targetType;
    }

    public static string GenerateFilledGenericTypeName(string typeName, List<Symbol> genericArguments)
    {
        if (genericArguments == null || genericArguments.Count == 0) return typeName;
        return $"{typeName}<{string.Join(",", genericArguments.Select(a => a.Type.TypeName))}>";
    }

    public static string GenerateFunctionTypeName(List<Symbol> arguments, Type returnType)
    {
        return $"{returnType.TypeName}({string.Join(",", arguments.Select(a => a.Type.TypeName))})";
    }

    public override bool Equals(object obj)
    {
        if (obj is not Type t) return false;
        return t.TypeName == TypeName && t.Kind == Kind;
    }

    public static implicit operator LLVMTypeRef(Type type)
    {
        return type.GetLLVMType();
    }
}