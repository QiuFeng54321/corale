using PseudoCode.Core.Analyzing;
using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;
using PseudoCode.Core.Runtime.Types;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime;

/// <summary>
/// Represents a whole program.
/// </summary>
public class PseudoProgram
{
    /// <summary>
    /// Stores feedbacks during parsing and typechecking (metaoperation)
    /// </summary>
    public List<Feedback> AnalyserFeedbacks = new();

    /// <summary>
    /// Front address uninitialiized
    /// </summary>
    public uint CurrentInstanceAddress;

    /// <summary>
    /// If true, the values being output will specify its type and members
    /// </summary>
    public bool DebugRepresentation;

    /// <summary>
    /// The outer-most scope used in this program
    /// </summary>
    public Scope GlobalScope;

    /// <summary>
    /// Simulates a memory. (uint address, Instance instance)
    /// </summary>
    public Dictionary<uint, Instance> Memory = new();

    /// <summary>
    /// Stores files opened in program (OPENFILE)
    /// </summary>
    public Dictionary<string, PseudoFileStream> OpenFiles = new();

    /// <summary>
    /// At runtime, this stack is used mainly for evaluation of an expression, kinda like postfix operations.<br/>
    /// <example>
    /// Load a -> [ref a]<br/>
    /// Push Immediate 1 -> [ref a, 1]<br/>
    /// Push Immediate 2 -> [ref a, 1, 2]<br/>
    /// Binary Add -> [ref a, 3] (pop 2 values, add them, and push the result back)<br/>
    /// Assign -> [] (Pops 2 values, assign 3 to ref a)
    /// </example>
    /// </summary>
    public Stack<Instance> RuntimeStack = new();

    /// <summary>
    /// After opcode generation, the program undergoes type check (meta-operate) to ensure types are right.<br/>
    /// <example>
    /// Load a -> [Type INTEGER]<br/>
    /// Push Immediate 1 -> [Type INTEGER, Type INTEGER]<br/>
    /// Push Immediate 2 -> [Type INTEGER, Type INTEGER, Type INTEGER]<br/>
    /// Binary Add -> [Type INTEGER, Type INTEGER] (pop 2 types, check return type, push return type)<br/>
    /// Assign -> [] (Pops 2 values, check if value is assignable with/without implicit casting)
    /// </example>
    /// </summary>
    public Stack<Definition> TypeCheckStack = new();

    public PseudoProgram()
    {
        GlobalScope = new Scope(null, this)
        {
            AllowStatements = true,
        };
        AddPrimitiveTypes();
        AddBuiltinFunctions();
    }

    /// <summary>
    /// Prints opcodes generated by compiler
    /// </summary>
    public bool DisplayOperationsAfterCompiled { get; set; }

    /// <summary>
    /// Prints the operation before running
    /// </summary>
    public bool DisplayOperationsAtRuntime { get; set; }

    /// <summary>
    /// Prohibits undeclared variables if false, disabling PLACEHOLDER type
    /// </summary>
    public bool AllowUndeclaredVariables { get; set; }


    public Definition FindDefinition(uint id)
    {
        return GlobalScope.FindDefinition(id);
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
        Instance.Null = GlobalScope.FindDefinition(Type.NullId).Type.Instance(scope: GlobalScope);
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
                        GlobalScope.FindDefinition(Type.StringId).Make("path", Definition.Attribute.Immutable)
                    },
                    ReturnType = GlobalScope.FindDefinition(Type.BooleanId)
                }
            },
            Func = (scope, program, args) =>
            {
                var path = args[0].Get<string>();
                return scope.FindDefinition(Type.BooleanId).Type.Instance(program.OpenFiles[path].Eof());
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
                        GlobalScope.FindDefinition(Type.RealId).Make("target", Definition.Attribute.Immutable),
                        GlobalScope.FindDefinition(Type.RealId).Make("from", Definition.Attribute.Immutable),
                        GlobalScope.FindDefinition(Type.RealId).Make("to", Definition.Attribute.Immutable)
                    },
                    ReturnType = GlobalScope.FindDefinition(Type.BooleanId)
                }
            },
            Func = (scope, program, args) =>
            {
                var target = args[0].Get<decimal>();
                var from = args[1].Get<decimal>();
                var to = args[2].Get<decimal>();
                return scope.FindDefinition(Type.BooleanId).Type.Instance(from <= target && target <= to);
            }
        });
    }

    public void PrintAnalyzerFeedbacks(TextWriter textWriter)
    {
        foreach (var feedback in AnalyserFeedbacks.OrderBy(f => f.Severity)) textWriter.WriteLine(feedback);
    }
}