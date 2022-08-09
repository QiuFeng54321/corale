using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Runtime.Emit.Opcodes;

public class Quaternion : IOpcode
{
    public Label Label { get; set; }
    public Identifier Store, Left, Right;
    public PseudoOperator Operator;

    public void Execute(PseudoRuntime runtime)
    {
        var storeInstance = runtime.GetInstanceInMemory(Store.Address);
        var left = runtime.GetInstanceInMemory(Store.Address);
        Instance result;
        if (Right == null)
        {
            result = left.Type.UnaryOperators[Operator](left);
        }
        else
        {
            var right = runtime.GetInstanceInMemory(Store.Address);
            result = left.Type.BinaryOperators[Operator](left, right);
        }

        storeInstance.Type.Assign(storeInstance, result);
    }

    public string Represent()
    {
        return Right == null ? $"{Operator} {Left}, {Store}" : $"{Operator} {Left}, {Right}, {Store}";
    }
}