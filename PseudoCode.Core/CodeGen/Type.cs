using LLVMSharp.Interop;

namespace PseudoCode.Core.CodeGen;

public class Type
{
    private LLVMTypeRef _llvmTypeRef;

    /// <summary>
    ///     If the type is a function, this stores the arguments.<br />
    ///     We need this to store symbols because we need to specify the attributes
    /// </summary>
    public List<Symbol> Arguments;

    /// <summary>
    ///     If the type is a <see cref="Types.CArray" />, this stores the number of elements in the array.
    /// </summary>
    public uint ArrayLength;

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
    public List<Symbol> Members;

    /// <summary>
    ///     Return type of the function. Use <see cref="Symbol"/> because we need attribute
    /// </summary>
    public Symbol ReturnType;

    /// <summary>
    ///     The name of the type. NOT THE NAME OF THE VARIABLE/PARAMETER!!
    /// </summary>
    public string TypeName;


    public void SetLLVMType(LLVMTypeRef type)
    {
        _llvmTypeRef = type;
    }

    public string GetSignature(Namespace ns)
    {
        return Kind switch
        {
            Types.Pointer => $"^{ElementType.GetSignature(ns)}",
            Types.Function => GenerateFunctionSignature(),
            _ => ns?.GetFullQualifier(TypeName) ?? TypeName
        };
    }

    public string GenerateFunctionSignature()
    {
        return GetFunctionSignature(Arguments, ReturnType);
    }

    public static string GetFunctionSignature(List<Symbol> arguments, Symbol returnType)
    {
        return $"{returnType.GetTypeString()}({string.Join(",", arguments.Select(a => a.GetTypeString()))})";
    }

    public LLVMTypeRef GetLLVMType()
    {
        if (_llvmTypeRef != null) return _llvmTypeRef;
        if (Kind == Types.Type)
        {
            List<LLVMTypeRef> llvmTypeMembers = new();
            foreach (var sym in Members)
            {
                llvmTypeMembers.Add(sym.Type.GetLLVMType());
            }

            _llvmTypeRef = LLVMTypeRef.CreateStruct(llvmTypeMembers.ToArray(), true);
            return _llvmTypeRef;
        }

        if (Kind == Types.Pointer)
        {
            _llvmTypeRef = LLVMTypeRef.CreatePointer(ElementType.GetLLVMType(), 0);
            return _llvmTypeRef;
        }

        if (Kind == Types.CArray)
        {
            _llvmTypeRef = LLVMTypeRef.CreateArray(ElementType.GetLLVMType(), ArrayLength);
            return _llvmTypeRef;
        }

        throw new NotImplementedException();
    }

    public static Type MakePrimitiveType(string name, System.Type type)
    {
        var resType = new Type
        {
            TypeName = name
        };
        if (type == typeof(long))
        {
            resType._llvmTypeRef = LLVMTypeRef.Int64;
            resType.Kind = Types.Integer;
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

        if (type == typeof(sbyte))
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

    public Type MakeArrayType(uint length)
    {
        return new Type
        {
            Kind = Types.CArray,
            ElementType = this,
            DebugInformation = DebugInformation,
            TypeName = $"{TypeName}[{length}]",
            ArrayLength = length
        };
    }

    public Type MakePointerTypeFromArray()
    {
        return ElementType.MakePointerType();
    }

    public Type MakePointerType()
    {
        return new Type
        {
            Kind = Types.Pointer,
            ElementType = this,
            TypeName = "^" + TypeName,
            DebugInformation = DebugInformation
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

    public static string GenerateFilledGenericTypeName(string typeName, List<Symbol> genericArguments)
    {
        if (genericArguments == null || genericArguments.Count == 0) return typeName;
        return $"{typeName}<{string.Join(",", genericArguments.Select(a => a.Type.TypeName))}>";
    }

    protected bool Equals(Type other)
    {
        if (other is not { }) return false;
        return _llvmTypeRef.Equals(other._llvmTypeRef) || (Kind == other.Kind && Kind switch
        {
            Types.Pointer => ElementType == other.ElementType,
            Types.Function => ReturnType == other.ReturnType && Arguments.SequenceEqual(other.Arguments),
            Types.Type => Members.SequenceEqual(other.Members),
            Types.CArray => ElementType == other.ElementType,
            _ => Kind == other.Kind
        });
    }


    public override int GetHashCode()
    {
        return HashCode.Combine(_llvmTypeRef, (int)Kind, TypeName);
    }

    public static bool operator ==(Type left, Type right)
    {
        return left?.Equals(right) ?? false;
    }

    public static bool operator !=(Type left, Type right)
    {
        return !(left?.Equals(right) ?? true);
    }

    public static implicit operator LLVMTypeRef(Type type)
    {
        return type.GetLLVMType();
    }
}