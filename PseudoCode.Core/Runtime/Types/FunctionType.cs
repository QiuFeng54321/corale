using PseudoCode.Core.Runtime.Errors;
using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types;

public class FunctionType : Type
{
    public ParameterInfo[] ParameterInfos;
    public Type ReturnType;

    public FunctionType(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override Instance Instance(object value = null, Scope scope = null)
    {
        var instance = new FunctionInstance(scope ?? ParentScope, Program)
        {
            Type = this,
            Members = new Dictionary<string, Instance>(),
            Value = value
        };
        foreach (var member in Members) instance.Members[member.Key] = member.Value.Instance(scope: ParentScope);

        return instance;
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
                    if (parameterInfo.IsReference)
                    {
                        if (passedInstance is not ReferenceInstance referenceInstance)
                            throw new InvalidTypeError(
                                $"Passed argument {i} of type {passedInstance.Type} is not reference", null);

                        if (passedInstance.RealInstance.Type != parameterInfo.Definition.Type)
                            throw new InvalidTypeError(
                                $"Passed argument {i} of type {passedInstance.Type} should not undergo implicit cast to {parameterInfo.Definition.Type}",
                                null);

                        s.InstanceAddresses.Add(parameterInfo.Name, referenceInstance.ReferenceAddress);
                    }
                    else
                    {
                        s.InstanceAddresses.Add(parameterInfo.Name,
                            Program.AllocateId(parameterInfo.Definition.Type.CastFrom(passedInstance)));
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
            args.Zip(ParameterInfos).Any(zip => !zip.Second.Definition.Type.IsConvertableFrom(zip.First.Type)))
            throw new InvalidArgumentsError(
                $"Calling {this} with arguments ({string.Join(", ", args.Select(arg => arg.Type))})", null);
    }

    public override string ToString()
    {
        return
            $"FUNCTION({string.Join(", ", ParameterInfos.Select(p => $"{(p.IsReference ? "BYREF " : "")}{p.Name}: {p.Definition.Type}"))}) {(ReturnType == null ? "" : $"RETURNS {ReturnType}")}";
    }

    public class ParameterInfo
    {
        public Definition Definition;
        public bool IsReference;
        public string Name;
    }
}