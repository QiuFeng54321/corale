using LLVMSharp.Interop;
using PseudoCode.Core.Analyzing;
using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Runtime.Errors;

namespace PseudoCode.Core.CodeGen;

public class Symbol : WithDebugInformation
{
    public static readonly Symbol ErrorSymbol = new("!ERROR!", false, null);
    public readonly DefinitionAttribute DefinitionAttribute;

    /// <summary>
    ///     If the symbol is a function, this stores the overloads of the function
    /// </summary>
    public readonly List<Symbol> FunctionOverloads = new();

    /// <summary>
    ///     Indicates whether the symbol is type-only
    /// </summary>
    public readonly bool IsType;

    /// <summary>
    ///     Type of the symbol. If the symbol is a set of overloads of a function, this is null
    /// </summary>
    public readonly Type Type;

    /// <summary>
    ///     This stores an expression which can generate a symbol when supplied with generic parameters.
    /// </summary>
    public IGenericExpression GenericExpression;

    /// <summary>
    ///     Pointer to the variable in memory
    /// </summary>
    public LLVMValueRef MemoryPointer;

    /// <summary>
    ///     The name of the symbol
    /// </summary>
    public string Name;

    /// <summary>
    ///     Namespace of this symbol
    /// </summary>
    public Namespace Namespace;

    /// <summary>
    ///     Indicates the index of this symbol in a type, if it is in a type
    /// </summary>
    public int TypeMemberIndex;

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

    public bool IsReference => DefinitionAttribute.HasFlag(DefinitionAttribute.Reference);

    public Symbol GetRealValue(CodeGenContext ctx, CompilationUnit cu)
    {
        if (this == ErrorSymbol) return null;
        return ValueRef != null
            ? this
            : MakeTemp(Type, GetRealValueRef(ctx, cu));
    }

    public LLVMValueRef GetRealValueRef(CodeGenContext ctx, CompilationUnit cu)
    {
        if (this == ErrorSymbol) return null;
        return ValueRef != null
            ? ValueRef
            : cu.Builder.BuildLoad2(Type.GetLLVMType(), GetPointerValueRef(ctx),
                ctx.NameGenerator.RequestTemp(ReservedNames.Load));
    }

    public LLVMValueRef GetPointerValueRef(CodeGenContext ctx)
    {
        if (MemoryPointer != null) return MemoryPointer;
        if (this == ErrorSymbol) return null;
        ctx.Analysis.Feedbacks.Add(new Feedback
        {
            Message = $"Symbol not a reference: {Namespace?.GetFullQualifier(Name) ?? Name}",
            Severity = Feedback.SeverityType.Error,
            DebugInformation = DebugInformation
        });
        return null;
    }

    public Symbol MakePointer(CodeGenContext ctx)
    {
        if (this == ErrorSymbol) return ErrorSymbol;
        return MakeTemp(new Type
        {
            Kind = Types.Pointer,
            ElementType = Type,
            TypeName = "^" + Type.TypeName
        }, GetPointerValueRef(ctx));
    }

    /// <summary>
    ///     Finds the function overload using the given argument types and return type.<br />
    ///     The return type can be omitted when return type is not known or doesn't matter.<br />
    ///     The return type can be used for finding cast functions
    /// </summary>
    /// <param name="arguments">Specified argument types to look for</param>
    /// <param name="returnSym">The return type of the function overload</param>
    /// <returns>The function overload found. Null if not found.</returns>
    public Symbol FindFunctionOverload(List<Symbol> arguments, Symbol returnSym = default)
    {
        foreach (var functionOverload in FunctionOverloads)
        {
            if (functionOverload.Type.Arguments.Count != arguments.Count) continue;
            var found = true;
            foreach (var (argument1, argument2) in functionOverload.Type.Arguments.Zip(arguments))
            {
                if (Equals(argument1.Type, argument2.Type)) continue;
                found = false;
                break;
            }

            if (!found) continue;
            if (returnSym == default || Equals(returnSym.Type, functionOverload.Type.ReturnType))
                return functionOverload;
        }

        return null;
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

    public Symbol MakePointerType()
    {
        if (this == ErrorSymbol) return null;
        return MakeTypeSymbol(new Type
        {
            ElementType = Type,
            Kind = Types.Pointer,
            TypeName = "^" + Type.TypeName
        }, Namespace);
    }

    /// <summary>
    ///     Generates a temporary symbol
    /// </summary>
    /// <param name="type">The type of the symbol</param>
    /// <param name="value">The value of the symbol</param>
    /// <param name="isMemoryPointer">If true, value will be assigned to MemoryPointer</param>
    /// <returns>The symbol generated</returns>
    public static Symbol MakeTemp(Type type, LLVMValueRef value, bool isMemoryPointer = false)
    {
        var sym = new Symbol(value.Name, false, type);
        if (isMemoryPointer) sym.MemoryPointer = value;
        else sym.ValueRef = value;
        return sym;
    }

    /// <summary>
    ///     <seealso
    ///         cref="MakeTemp(string,PseudoCode.Core.CodeGen.Type,CodeGenContext,LLVMSharp.Interop.LLVMValueRef)" />
    /// </summary>
    /// <param name="nameTemplate"></param>
    /// <param name="typeName">The name of the type to lookup</param>
    /// <param name="ctx"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidTypeError">Type cannot be found</exception>
    public static Symbol MakeTemp(string nameTemplate, string typeName, CodeGenContext ctx, LLVMValueRef value)
    {
        if (!ctx.GlobalNamespace.TryGetSymbol(typeName, out var sym)) throw new InvalidTypeError(typeName);

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

    public static Symbol MakeTypeSymbol(Type type, Namespace ns)
    {
        return new Symbol(type.TypeName, true, type, ns);
    }

    public static Symbol MakeGenericSymbol(string name, IGenericExpression genericDecl)
    {
        return new Symbol(name, true, null)
        {
            GenericExpression = genericDecl
        };
    }

    public string GetTypeString()
    {
        return (IsReference ? "&" : "") + Type.GetSignature(Namespace);
    }

    /// <summary>
    ///     Fill the generic arguments (kinda like making types from template)
    /// </summary>
    /// <param name="ctx">Context of the symbol</param>
    /// <param name="function">The function this symbol is in</param>
    /// <param name="genericArguments">The arguments to fill</param>
    /// <returns>The cloned type with generic types and fields filled in</returns>
    public Symbol FillGeneric(CodeGenContext ctx, CompilationUnit cu, Function function, List<Symbol> genericArguments)
    {
        if (GenericExpression == null) return this; // Nothing to fill
        return GenericExpression.Generate(ctx, cu, function, genericArguments);
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

    public Symbol MakeStructMemberDeclSymbol(string name)
    {
        return new Symbol(name, false, Type);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(FunctionOverloads);
        hashCode.Add((int)DefinitionAttribute);
        hashCode.Add(GenericExpression);
        hashCode.Add(IsType);
        hashCode.Add(MemoryPointer);
        hashCode.Add(Name);
        hashCode.Add(Namespace);
        hashCode.Add(Type);
        hashCode.Add(TypeMemberIndex);
        hashCode.Add(ValueRef);
        return hashCode.ToHashCode();
    }

    public static Symbol MakeFunctionGroupSymbol(string name, Type pseudoFunctionType)
    {
        return new Symbol(name, false, pseudoFunctionType);
    }

    public bool AddOverload(Symbol overload)
    {
        if (FunctionOverloads.Any(s => s.Type == overload.Type)) return false;
        FunctionOverloads.Add(overload);
        return true;
    }
}