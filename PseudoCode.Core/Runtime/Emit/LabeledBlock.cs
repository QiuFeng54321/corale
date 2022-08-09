using PseudoCode.Core.Runtime.Emit.Opcodes;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Runtime.Emit;

public record LabeledBlock(Label Label, ProgramBuilder ProgramBuilder, List<IOpcode> Opcodes = default)
{
    public void AddOpcode(IOpcode opcode)
    {
        Opcodes.Add(opcode);
    }

    public Identifier AddQuaternion(PseudoOperator pseudoOperator, Identifier left, Identifier right = default,
        Identifier store = default)
    {
        var storeId = store;
        if (store == null)
        {
            var resultType = right == null
                ? left.Definition.Type.UnaryResultType(pseudoOperator)
                : left.Definition.Type.BinaryResultType(pseudoOperator, right.Definition.Type);
            var resultDef = new Definition(ParentScope, Program)
            {
                Type = resultType,
                Attributes = DefinitionAttribute.Immutable
            };
            store = ProgramBuilder.CurrentScope.MakeTempIdentifier()
        }

        AddOpcode(new Quaternion
        {
            Left = left,
            Right = right,
            Operator = pseudoOperator,
            Store = store ??
        });
    }

    public IEnumerable<IOpcode> ExpandOpcodes()
    {
        if (Opcodes.Count == 0)
        {
            yield return new Nop
            {
                Label = Label
            };
        }

        for (var index = 0; index < Opcodes.Count; index++)
        {
            var opcode = Opcodes[index];
            if (index == 0) opcode.Label = Label;
            yield return opcode;
        }
    }
}