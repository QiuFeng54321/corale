using PseudoCode.Core.Analyzing;
using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;
using PseudoCode.Core.Runtime.Types;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime;

public class PseudoProgram
{
    public uint CurrentInstanceAddress;
    public bool DebugRepresentation;
    public bool DisplayOperationsAfterCompiled { get; set; }
    public bool DisplayOperationsAtRuntime { get; set; }
    public Scope GlobalScope;
    public Dictionary<uint, Instance> Memory = new();
    public Stack<Instance> RuntimeStack = new();
    public Stack<Type> TypeCheckStack = new();
    public List<Feedback> AnalyserFeedbacks = new();
    public Dictionary<string, PseudoFileStream> OpenFiles = new();
    public bool AllowUndeclaredVariables { get; set; }

    public PseudoProgram()
    {
        GlobalScope = new Scope(null, this);
        AddPrimitiveTypes();
        AddBuiltinFunctions();
    }

    public uint AllocateId(Instance i)
    {
        i.InstanceAddress = CurrentInstanceAddress++;
        i.Program = this;
        Memory.Add(i.InstanceAddress, i);
        return i.InstanceAddress;
    }

    public uint AllocateId(Func<Instance> generator)
    {
        return AllocateId(generator());
    }

    public uint Allocate(int length, Func<Instance> generator)
    {
        var startAddress = CurrentInstanceAddress;
        for (var i = 0; i < length; i++) AllocateId(generator);

        return startAddress;
    }

    public void SetMemory(Range segment, Func<Instance> value)
    {
        for (var i = (uint)segment.Start; i < segment.End; i++)
        {
            Memory[i] = value();
            Memory[i].InstanceAddress = i;
            Memory[i].Program = this;
        }
    }

    public void ReleaseMemory(Range segment)
    {
        for (var i = (uint)segment.Start; i < segment.End; i++)
            if (Memory.ContainsKey(i))
                Memory.Remove(i);
    }

    public void AddPrimitiveTypes()
    {
        GlobalScope.AddType(new BooleanType (GlobalScope, this));
        GlobalScope.AddType(new IntegerType (GlobalScope, this));
        GlobalScope.AddType(new RealType (GlobalScope, this));
        GlobalScope.AddType(new StringType (GlobalScope, this));
        GlobalScope.AddType(new CharacterType (GlobalScope, this));
        GlobalScope.AddType(new DateType (GlobalScope, this));
        GlobalScope.AddType(new NullType (GlobalScope, this));
        GlobalScope.AddType(new PlaceholderType (GlobalScope, this));
        Instance.Null = GlobalScope.FindTypeDefinition(Type.NullId).Type.Instance(scope: GlobalScope);
    }

    public void AddBuiltinFunctions()
    {
        GlobalScope.AddOperation(new MakeBuiltinFunctionOperation(GlobalScope, this)
        {
            Name = "EOF",
            Definition = new Definition
            {
                Name = "EOF",
                References = new(),
                SourceRange = SourceRange.Identity,
                Type = new BuiltinFunctionType(GlobalScope, this)
                {
                    ParameterInfos = new []
                    {
                        new FunctionType.ParameterInfo
                        {
                            Name = "path",
                            Definition = GlobalScope.FindTypeDefinition(Type.StringId)
                        }
                    },
                    ReturnType = GlobalScope.FindTypeDefinition(Type.BooleanId).Type
                }
            },
            Func = (scope, program, args) =>
            {
                var path = args[0].Get<string>();
                return scope.FindTypeDefinition(Type.BooleanId).Type.Instance(program.OpenFiles[path].Eof());
            }
        });
    }

    public void PrintAnalyzerFeedbacks(TextWriter textWriter)
    {
        foreach (var feedback in AnalyserFeedbacks)
        {
            textWriter.WriteLine(feedback);
        }
    }
}