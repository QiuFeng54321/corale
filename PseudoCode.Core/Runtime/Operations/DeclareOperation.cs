using PseudoCode.Core.Runtime.Errors;

namespace PseudoCode.Core.Runtime.Operations;

public class DeclareOperation : Operation
{
    public List<Range> Dimensions = new();
    public string Name;
    public Type Type;

    public DeclareOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        if (ParentScope.ScopeStates.InstanceAddresses.ContainsKey(Name))
            throw new InvalidAccessError(string.Format(strings.DeclareOperation_Operate_AlreadyDeclared, Name), null);
        if (Dimensions.Count == 0)
        {
            ParentScope.ScopeStates.InstanceAddresses.Add(Name, Program.AllocateId(Type.Instance(scope: ParentScope)));
        }
        else
        {
            var arrayInstance = ((ArrayType)ParentScope.FindType(Type.ArrayId)).Instance(Dimensions, Type);
            arrayInstance.InitialiseInMemory();
            ParentScope.ScopeStates.InstanceAddresses.Add(Name, Program.AllocateId(arrayInstance));
        }
    }

    public override string ToPlainString()
    {
        var typeStr = Dimensions.Count == 0 ? Type.ToString() : $"ARRAY[{string.Join(',', Dimensions)}] OF {Type}";
        return string.Format(strings.DeclareOperation_ToPlainString, Name, typeStr);
    }
}