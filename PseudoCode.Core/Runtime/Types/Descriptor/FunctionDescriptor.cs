using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types.Descriptor;

public record FunctionDescriptor
    (ITypeDescriptor ReturnType = null, Definition[] ParameterInfos = null) : ITypeDescriptor
{
    public Definition GetDefinition(Scope scope, PseudoProgram program)
    {
        return new Definition(scope, program)
        {
            Type = new FunctionType(scope, program)
            {
                ReturnType = ReturnType?.GetDefinition(scope, program),
                ParameterInfos = ParameterInfos
            },
            Attributes = Definition.Attribute.Type
        };
    }

    public string SelfName => "FUNCTION";

    public override string ToString() => string.Format(strings.FunctionType_ToString,
        string.Join(", ",
            ParameterInfos.Select(p =>
                p.ToString())),
        ReturnType == null ? "" : $"RETURNS {ReturnType}");
}