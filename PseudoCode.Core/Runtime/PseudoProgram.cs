using PseudoCode.Core.Analyzing;
using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;
using PseudoCode.Core.Runtime.Types;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime;

public class PseudoProgram
{
    public List<Feedback> AnalyserFeedbacks = new();
    public uint CurrentInstanceAddress;
    public bool DebugRepresentation;
    public Scope GlobalScope;
    public Dictionary<uint, Instance> Memory = new();
    public Dictionary<string, PseudoFileStream> OpenFiles = new();
    public Dictionary<string, Definition> TypeDefinitions = new();
    public Stack<Instance> RuntimeStack = new();
    public Stack<TypeInfo> TypeCheckStack = new();

    public PseudoProgram()
    {
        GlobalScope = new Scope(null, this)
        {
            AllowStatements = true
        };
        AddPrimitiveTypes();
        AddBuiltinFunctions();
    }

    public bool DisplayOperationsAfterCompiled { get; set; }
    public bool DisplayOperationsAtRuntime { get; set; }
    public bool AllowUndeclaredVariables { get; set; }

    
    public Definition FindTypeDefinition(uint id)
    {
        return TypeDefinitions.First(p => p.Value.Type.Id == id).Value;
    }
    public Definition FindTypeDefinition(string name)
    {
        return TypeDefinitions.GetValueOrDefault(name, null);
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

    public IEnumerable<uint> Allocate(int length, Func<Instance> generator)
    {
        var startAddress = CurrentInstanceAddress;
        for (var i = 0; i < length; i++) yield return AllocateId(generator);
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
        GlobalScope.AddType(new BooleanType(GlobalScope, this));
        GlobalScope.AddType(new IntegerType(GlobalScope, this));
        GlobalScope.AddType(new RealType(GlobalScope, this));
        GlobalScope.AddType(new StringType(GlobalScope, this));
        GlobalScope.AddType(new CharacterType(GlobalScope, this));
        GlobalScope.AddType(new DateType(GlobalScope, this));
        GlobalScope.AddType(new NullType(GlobalScope, this));
        GlobalScope.AddType(new PlaceholderType(GlobalScope, this));
        GlobalScope.AddType(new ModuleType(GlobalScope, this));
        Instance.Null = GlobalScope.FindTypeDefinition(Type.NullId).Type.Instance(scope: GlobalScope);
    }

    public void AddBuiltinFunctions()
    {
        GlobalScope.AddOperation(new MakeBuiltinFunctionOperation(GlobalScope, this)
        {
            Name = "EOF",
            Definition = new Definition(GlobalScope, this)
            {
                Name = "EOF",
                References = new List<SourceRange>(),
                SourceRange = SourceRange.Identity,
                Type = new BuiltinFunctionType(GlobalScope, this)
                {
                    ParameterInfos = new[]
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
        GlobalScope.AddOperation(new MakeBuiltinFunctionOperation(GlobalScope, this)
        {
            Name = "__in_range",
            Definition = new Definition(GlobalScope, this)
            {
                Name = "__in_range",
                References = new List<SourceRange>(),
                SourceRange = SourceRange.Identity,
                Type = new BuiltinFunctionType(GlobalScope, this)
                {
                    ParameterInfos = new[]
                    {
                        new FunctionType.ParameterInfo
                        {
                            Name = "target",
                            Definition = GlobalScope.FindTypeDefinition(Type.RealId)
                        },new FunctionType.ParameterInfo
                        {
                            Name = "from",
                            Definition = GlobalScope.FindTypeDefinition(Type.RealId)
                        },new FunctionType.ParameterInfo
                        {
                            Name = "to",
                            Definition = GlobalScope.FindTypeDefinition(Type.RealId)
                        },
                    },
                    ReturnType = GlobalScope.FindTypeDefinition(Type.BooleanId).Type
                }
            },
            Func = (scope, program, args) =>
            {
                var target = args[0].Get<decimal>();
                var from = args[1].Get<decimal>();
                var to = args[2].Get<decimal>();
                return scope.FindTypeDefinition(Type.BooleanId).Type.Instance(from <= target && target <= to);
            }
        });
    }

    public void PrintAnalyzerFeedbacks(TextWriter textWriter)
    {
        foreach (var feedback in AnalyserFeedbacks.OrderBy(f => f.Severity)) textWriter.WriteLine(feedback);
    }
}