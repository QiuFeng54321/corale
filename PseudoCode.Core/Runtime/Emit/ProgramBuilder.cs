namespace PseudoCode.Core.Runtime.Emit;

public class ProgramBuilder
{
    public PseudoScope GlobalScope;
    public PseudoScope CurrentScope;
    public LabelManager LabelManager = new();
    public List<LabeledBlock> Blocks = new();
    public PseudoAssembly PseudoAssembly;
    public MemoryAddress CurrentMemoryAddress = 0;
    public Stack<Identifier> GenerationStack = new();
    public Dictionary<TypeDefinition, Identifier> ConstantMap = new();

    public ProgramBuilder()
    {
        CurrentScope = GlobalScope = new PseudoScope(this);
    }

    public LabeledBlock MakeBlock(Label label)
    {
        var block = new LabeledBlock(label, this);
        Blocks.Add(block);
        return block;
    }
    public PseudoAssembly Build()
    {
        var opcodes = Blocks.SelectMany(b => b.ExpandOpcodes()).ToArray();
        for (var index = 0; index < opcodes.Length; index++)
        {
            var opcode = opcodes[index];
            if (opcode.Label == null) continue;
            if (opcode.Label.OpcodeIndex == -1) opcode.Label.OpcodeIndex = index;
        }

        PseudoAssembly.Opcodes = opcodes;
        return PseudoAssembly;
    }

    public Identifier MakeIdentifier(string name, TypeDefinition definition, PseudoScope scope = default)
    {
        var address = new MemoryAddress(CurrentMemoryAddress);
        CurrentMemoryAddress += definition.Type.Size;
        var identifier = new Identifier(name, scope ?? GlobalScope, address, definition);
        identifier.Scope.AddIdentifier(name, identifier);
        return identifier;
    }

    public Identifier GetOrMakeConstant(TypeDefinition constantDefinition)
    {
        if (ConstantMap.ContainsKey(constantDefinition)) return ConstantMap[constantDefinition];
        var id = MakeIdentifier($"$$CONST{ConstantMap.Count}", constantDefinition);
        ConstantMap.Add(constantDefinition, id);
        return id;
    }
}