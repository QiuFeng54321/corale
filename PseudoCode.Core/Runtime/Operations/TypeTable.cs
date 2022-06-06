using PseudoCode.Core.Analyzing;

namespace PseudoCode.Core.Runtime.Operations;

public class TypeTable
{
    public TypeInfo NullType = new() { Name = "NULL" };
    public VariableInfo NullVar = new() { Name = "Null", Type = new TypeInfo { Name = "NULL" } };
    public TypeTable ParentTable;
    public PseudoProgram Program;
    public Dictionary<string, TypeInfo> TypeInfos = new();
    public Dictionary<string, VariableInfo> VariableInfos = new();


    public TypeTable(TypeTable parentTable, PseudoProgram program)
    {
        ParentTable = parentTable;
        Program = program;
    }

    public Stack<Info> TypeChecker { get; set; } = new();

    public TypeInfo FindType(string name)
    {
        return TypeInfos.ContainsKey(name) ? TypeInfos[name] : ParentTable?.FindType(name) ?? NullType;
    }

    public VariableInfo FindVariable(string name, SourceLocation sourceLocation = null)
    {
        var res = FindVariableInner(name, sourceLocation);
        if (res != null) return res;
        var placeholder = new VariableInfo
        {
            Type = FindType("PLACEHOLDER"),
            Name = name,
            DeclarationLocation = sourceLocation
        };
        VariableInfos.Add(name, placeholder);
        return placeholder;
    }

    public VariableInfo FindVariableInner(string name, SourceLocation sourceLocation = null)
    {
        return VariableInfos.ContainsKey(name)
            ? VariableInfos[name]
            : ParentTable?.FindVariableInner(name, sourceLocation);
    }

    public void AddToStack(Info info)
    {
        TypeChecker.Push(info);
    }

    public void FormArray(int length, SourceLocation location)
    {
        List<TypeInfo> elements = new();
        for (var i = 0; i < length; i++)
        {
            var info = PopStack();
            if (info is ArrayTypeInfo arrayTypeInfo)
                elements.Insert(0, arrayTypeInfo.ElementTypeInfo);
            else
                elements.Insert(0, info.Type);
        }

        AddToStack(new ArrayTypeInfo
        {
            DeclarationLocation = location,
            Dimensions = 1,
            ElementTypeInfo = elements.First().Type
        });
    }

    public void ArrayAccess(SourceLocation location)
    {
        var access = (ArrayTypeInfo)PopStack();
        var accessed = PopStack();
        if (accessed.Type is not ArrayTypeInfo arrayTypeInfo)
        {
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = "Invalid type of array access",
                SourceRange = new SourceRange(location, location)
            });
            AddToStack(new TypeInfo
            {
                DeclarationLocation = location,
                Name = "Null"
            });
        }
        else
        {
            if (access.Dimensions < arrayTypeInfo.Dimensions)
                AddToStack(new ArrayTypeInfo
                {
                    DeclarationLocation = location,
                    Dimensions = arrayTypeInfo.Dimensions - access.Dimensions,
                    ElementTypeInfo = arrayTypeInfo.ElementTypeInfo
                });
            else
                AddToStack(arrayTypeInfo.ElementTypeInfo);
        }
    }

    public void MakeBinary(int op, SourceLocation location)
    {
        var right = PopStack();
        var left = PopStack();
        // switch (op)
        // {
        //     case PseudoCodeLexer.Divide:
        //         if (left.Type.Name == "INTEGER")
        //         {
        //             TypeChecker.Push(FindType("REAL"));
        //             break;
        //         }
        //         TypeChecker.Push(left.Type);
        //     case PseudoCodeLexer.Add:
        //     case PseudoCodeLexer.Subtract:
        //     case PseudoCodeLexer.Multiply:
        //         
        // }

        TypeChecker.Push(left.Type);
    }

    private Info PopStack()
    {
        try
        {
            return TypeChecker.Pop();
        }
        catch (InvalidOperationException e)
        {
            return NullVar;
        }
    }

    public void Assign(SourceLocation location)
    {
        var right = PopStack();
        var left = PopStack();
        if (left.Type.Name == "PLACEHOLDER") VariableInfos[left.Name].Type = right.Type;
    }

    public void MakeUnary(int op, SourceLocation location)
    {
    }

    public IEnumerable<VariableInfo> GetVariableCompletionBefore(SourceLocation location)
    {
        return VariableInfos.Where(x => x.Value.DeclarationLocation <= location)
            .Select(x => x.Value)
            .Concat(ParentTable?.GetVariableCompletionBefore(location) ?? Array.Empty<VariableInfo>());
    }

    public IEnumerable<TypeInfo> GetTypeCompletionBefore(SourceLocation location)
    {
        return TypeInfos.Where(x => x.Value.DeclarationLocation <= location)
            .Select(x => x.Value)
            .Concat(ParentTable?.GetTypeCompletionBefore(location) ?? Array.Empty<TypeInfo>());
    }

    public class Info
    {
        public SourceLocation DeclarationLocation;
        public virtual string Name { get; set; }

        public virtual TypeInfo Type { get; set; }

        public override string ToString()
        {
            return $"{DeclarationLocation} {Name} : {(Type == this ? "" : Type)}";
        }
    }

    public class VariableInfo : Info
    {
    }

    public class TypeInfo : Info
    {
        public Dictionary<string, TypeInfo> Members;
        public override TypeInfo Type => this;

        public override string ToString()
        {
            return Name;
        }
    }

    public class ArrayTypeInfo : TypeInfo
    {
        public uint Dimensions;
        public TypeInfo ElementTypeInfo;
        public override string Name => "ARRAY";

        public override string ToString()
        {
            return $"{Name} OF {ElementTypeInfo}";
        }
    }
}