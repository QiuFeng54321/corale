using PseudoCode.Core.CodeGen;
using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Runtime.Errors;
using Type = System.Type;

namespace PseudoCode.Core.Runtime.Reflection;

public static class TypeBinder
{
    public static readonly Dictionary<Type, Symbol> TypeMap = new()
    {
        [typeof(int)] = BuiltinTypes.Integer,
        [typeof(double)] = BuiltinTypes.Real,
        [typeof(string)] = BuiltinTypes.CharPtr,
        [typeof(char)] = BuiltinTypes.Char,
        [typeof(byte)] = BuiltinTypes.Char,
        [typeof(BlittableChar)] = BuiltinTypes.Char,
        [typeof(bool)] = BuiltinTypes.Boolean,
        [typeof(BlittableBoolean)] = BuiltinTypes.Boolean,
        // [typeof(DateOnly)] = BuiltinTypes.,
        [typeof(void)] = BuiltinTypes.Void
    };

    public static Symbol GetTypeSymbolFromSystemType(CodeGenContext ctx, Type infoParameterType)
    {
        if (TypeMap.ContainsKey(infoParameterType)) return TypeMap[infoParameterType];
        if (infoParameterType.IsPointer)
            return GetTypeSymbolFromSystemType(ctx, infoParameterType.GetElementType()).MakePointerType();

        if (infoParameterType.IsValueType) return MakeTypeFromStruct(ctx, infoParameterType);

        throw new InvalidTypeError(infoParameterType.ToString());
    }

    public static Symbol MakeTypeFromStruct(CodeGenContext ctx, Type structType)
    {
        var typeAttribute = ReflectionHelper.GetAttribute<BuiltinTypeAttribute>(structType);
        var typeName = typeAttribute?.Name ?? structType.Name;
        var members = new List<Symbol>();
        foreach (var member in structType.GetFields())
        {
            var memberTypeSym = GetTypeSymbolFromSystemType(ctx, member.FieldType);
            members.Add(memberTypeSym);
        }

        var record = new Record
        {
            TypeName = typeName,
            Members = members
        };
        record.CodeGen(ctx, ctx.CompilationUnit.MainFunction);
        TypeMap.Add(structType, record.TypeSymbol);
        return record.TypeSymbol;
    }
}