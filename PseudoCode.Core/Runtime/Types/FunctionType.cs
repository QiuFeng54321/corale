using PseudoCode.Core.Runtime.Errors;
using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types;

public class FunctionType : Type
{
    public Definition[] ParameterInfos;
    public Definition ReturnType;

    public FunctionType(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override Instance Instance(object value = null, Scope scope = null)
    {
        return DefaultInstance<FunctionInstance>(value, scope);
    }

    public override Instance Call(FunctionInstance functionInstance, Instance[] args)
    {
        CheckArguments(args);
        try
        {
            functionInstance.FunctionBody.Operate(s =>
            {
                s.ResetTemporaryContent();
                foreach (var ((parameterInfo, passedInstance), i) in ParameterInfos.Zip(args).Select((v, i) => (v, i)))
                    if (parameterInfo.Attributes.HasFlag(Definition.Attribute.Reference))
                    {
                        if (passedInstance is not ReferenceInstance referenceInstance)
                            throw new InvalidTypeError(
                                $"Passed argument {i} of type {passedInstance.Type} is not reference", null);

                        if (passedInstance.RealInstance.Type != parameterInfo.Type)
                            throw new InvalidTypeError(
                                $"Passed argument {i} of type {passedInstance.Type} should not undergo implicit cast to {parameterInfo.TypeString()}",
                                null);

                        s.InstanceAddresses.Add(parameterInfo.Name, referenceInstance.ReferenceAddress);
                    }
                    else
                    {
                        s.InstanceAddresses.Add(parameterInfo.Name,
                            Program.AllocateId(parameterInfo.Type.CastFrom(passedInstance)));
                    }

                return s;
            });
        }
        catch (ReturnBreak)
        {
            return Program.RuntimeStack.Pop();
        }

        return Instances.Instance.Null;
    }

    public void CheckArguments(Instance[] args)
    {
        if (args.Length != ParameterInfos.Length ||
            args.Zip(ParameterInfos).Any(zip => !zip.Second.Type.IsConvertableFrom(zip.First.Type)))
            throw new InvalidArgumentsError(
                $"Calling {this} with arguments ({string.Join(", ", args.Select(arg => arg.Type))})", null);
    }

    public override string ToString()
    {
        return
            string.Format(strings.FunctionType_ToString,
                string.Join(", ",
                    ParameterInfos.Select(p =>
                        $"{(p.Attributes.HasFlag(Definition.Attribute.Reference) ? "BYREF " : "")}{p.Name}: {p.TypeString()}")),
                ReturnType == null ? "" : $"RETURNS {ReturnType}");
    }
}