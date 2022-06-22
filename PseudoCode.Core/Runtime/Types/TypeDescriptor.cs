using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types;

public record TypeDescriptor(string Name, int TypeId = -1, int Dimensions = 0, TypeDescriptor ElementType = null,
    TypeDescriptor ReturnType = null, Definition[] ParameterInfos = null)
{
    public Type GetType(Scope scope, PseudoProgram program)
    {
        return GetDefinition(scope, program)?.Type ?? new NullType(scope, program);
    }

    public Definition GetDefinition(Scope scope, PseudoProgram program)
    {
        if (TypeId != -1) return scope.FindDefinition((uint)TypeId);
        return Name switch
        {
            "ARRAY" => new Definition(scope, program)
            {
                Type = new ArrayType(scope, program)
                {
                    ElementType = ElementType.GetType(scope, program), DimensionCount = Dimensions
                },
                Attributes = Definition.Attribute.Type
            },
            "FUNCTION" => new Definition(scope, program)
            {
                Type = new FunctionType(scope, program)
                {
                    ReturnType = ReturnType.GetDefinition(scope, program),
                    ParameterInfos = ParameterInfos
                },
                Attributes = Definition.Attribute.Type
            },
            _ => scope.FindDefinition(Name)
        };
    }

    public override string ToString()
    {
        return Name switch
        {
            "ARRAY" => string.Format(strings.ArrayType_ToString, Dimensions, ElementType),
            "FUNCTION" => string.Format(strings.FunctionType_ToString,
                string.Join(", ",
                    ParameterInfos.Select(p =>
                        $"{(p.Attributes.HasFlag(Definition.Attribute.Reference) ? "BYREF " : "")}{p.Name}: {p.TypeDescriptor}")),
                ReturnType == null ? "" : $"RETURNS {ReturnType}"),
            _ => Name
        };
    }
};