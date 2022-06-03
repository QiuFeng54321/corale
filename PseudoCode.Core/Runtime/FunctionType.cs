using PseudoCode.Core.Runtime.Errors;
using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime;

public class FunctionType : Type
{
    public Type ReturnType;
    public ParameterInfo[] ParameterInfos;

    public class ParameterInfo
    {
        public string Name;
        public Type Type;
        public bool IsReference;
    }

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
        if (args.Length != ParameterInfos.Length ||
            args.Zip(ParameterInfos).Any(zip => !zip.Second.Type.IsConvertableFrom(zip.First.Type)))
            throw new InvalidArgumentsError(
                $"Calling {this} with arguments ({string.Join(", ", args.Select(arg => arg.Type))})", null);
        try
        {
            functionInstance.FunctionBody.Operate(s =>
            {
                s.ResetTemporaryContent();
                foreach (var ((parameterInfo, passedInstance), i) in ParameterInfos.Zip(args).Select((v, i) => (v, i)))
                {
                    if (parameterInfo.IsReference)
                    {
                        if (passedInstance is not ReferenceInstance referenceInstance)
                        {
                            throw new InvalidTypeError(
                                $"Passed argument {i} of type {passedInstance.Type} is not reference", null);
                        }

                        if (passedInstance.RealInstance.Type != parameterInfo.Type)
                        {
                            throw new InvalidTypeError(
                                $"Passed argument {i} of type {passedInstance.Type} should not undergo implicit cast to {parameterInfo.Type}",
                                null);
                        }

                        s.InstanceAddresses.Add(parameterInfo.Name, referenceInstance.ReferenceAddress);
                    }
                    else
                    {
                        s.InstanceAddresses.Add(parameterInfo.Name,
                            Program.AllocateId(parameterInfo.Type.CastFrom(passedInstance)));
                    }
                }

                return s;
            });
        }
        catch (ReturnBreak)
        {
            return Program.RuntimeStack.Pop();
        }

        return Runtime.Instance.Null;
    }

    public override string ToString()
    {
        return
            $"FUNCTION({string.Join(", ", ParameterInfos.Select(p => $"{(p.IsReference ? "BYREF " : "")}{p.Type}"))}) RETURNS {ReturnType}";
    }
}