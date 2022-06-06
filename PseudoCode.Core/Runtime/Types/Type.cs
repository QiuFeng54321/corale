using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using PseudoCode.Core.Runtime.Errors;
using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types;

public class Type
{
    public delegate Instance BinaryOperator(Instance i1, Instance i2);

    public delegate Instance UnaryOperator(Instance i);

    public const uint IntegerId = 0,
        RealId = 1,
        StringId = 2,
        BooleanId = 3,
        CharId = 4,
        DateId = 5,
        NullId = 6,
        PlaceholderId = 7,
        ArrayId = 8;

    private static uint _incrementId = ArrayId;

    [JsonIgnore] public Dictionary<int, BinaryOperator> BinaryOperators;
    [JsonIgnore] public Dictionary<int, UnaryOperator> UnaryOperators;
    [JsonIgnore] public PseudoProgram Program;
    [JsonIgnore] public Scope ParentScope;

    
    public virtual bool Serializable => false;

    public Type(Scope parentScope, PseudoProgram program)
    {
        Program = program;
        ParentScope = parentScope;
        BinaryOperators = new Dictionary<int, BinaryOperator>
        {
            { PseudoCodeLexer.Add, Add },
            { PseudoCodeLexer.Multiply, Multiply },
            { PseudoCodeLexer.Divide, Divide },
            { PseudoCodeLexer.Subtract, Subtract },
            { PseudoCodeLexer.Pow, Pow },
            { PseudoCodeLexer.IntDivide, IntDivide },
            { PseudoCodeLexer.Mod, Mod },
            { PseudoCodeLexer.Greater, Greater },
            { PseudoCodeLexer.GreaterEqual, GreaterEqual },
            { PseudoCodeLexer.SmallerEqual, SmallerEqual },
            { PseudoCodeLexer.Smaller, Smaller },
            { PseudoCodeLexer.Equal, Equal },
            { PseudoCodeLexer.NotEqual, NotEqual },
            { PseudoCodeLexer.And, And },
            { PseudoCodeLexer.BitAnd, BitAnd },
            { PseudoCodeLexer.Or, Or }
        };
        UnaryOperators = new Dictionary<int, UnaryOperator>
        {
            { PseudoCodeLexer.Subtract, Negative },
            { PseudoCodeLexer.Not, Not }
        };
    }

    public virtual uint Id { get; set; } = ++_incrementId;
    public virtual string Name { get; set; } = null!;
    public Dictionary<string, Type> Members { get; } = new();

    public virtual Instance Instance(object value = null, Scope scope = null)
    {
        var instance = new Instance(scope ?? ParentScope, Program)
        {
            Type = this,
            Members = new Dictionary<string, Instance>(),
            Value = value
        };
        foreach (var member in Members) instance.Members[member.Key] = member.Value.Instance(scope: ParentScope);

        return instance;
    }


    public virtual Instance Clone(Instance instance)
    {
        return Instance(instance.Value, ParentScope);
    }

    public virtual Error MakeUnsupported(Instance[] args, [CallerMemberName] string caller = "Unknown")
    {
        return new UnsupportedCastError($"Cannot call {this}", null);
    }

    public virtual Error MakeUnsupported(Instance i1, Instance i2 = null, [CallerMemberName] string caller = "Unknown")
    {
        return MakeUnsupported(i1.Type, i2?.Type, caller);
    }

    public virtual Error MakeUnsupported(Type i1, Type i2 = null, [CallerMemberName] string caller = "Unknown")
    {
        return new UnsupportedCastError(
            string.Format(strings.Type_ThrowUnsupported, caller, i1, (i2 != null ? strings.and + i2 : "")), null);
    }

    public virtual Type BinaryResultType(int type, Type right)
    {
        return new NullType(ParentScope, Program);
    }

    public virtual Type UnaryResultType(int type)
    {
        return new NullType(ParentScope, Program);
    }

    public virtual Instance Add(Instance i1, Instance i2)
    {
        throw MakeUnsupported(i1, i2);
    }

    public virtual Instance Subtract(Instance i1, Instance i2)
    {
        throw MakeUnsupported(i1, i2);
    }

    public virtual Instance Multiply(Instance i1, Instance i2)
    {
        throw MakeUnsupported(i1, i2);
    }

    public virtual Instance Divide(Instance i1, Instance i2)
    {
        throw MakeUnsupported(i1, i2);
    }

    public virtual Instance IntDivide(Instance i1, Instance i2)
    {
        throw MakeUnsupported(i1, i2);
    }

    public virtual Instance Mod(Instance i1, Instance i2)
    {
        throw MakeUnsupported(i1, i2);
    }

    public virtual Instance Pow(Instance i1, Instance i2)
    {
        throw MakeUnsupported(i1, i2);
    }

    public virtual Instance Negative(Instance i)
    {
        throw MakeUnsupported(i);
    }

    public virtual Instance Index(Instance i1, Instance i2)
    {
        throw MakeUnsupported(i1, i2);
    }

    public virtual Instance Not(Instance i)
    {
        throw MakeUnsupported(i);
    }

    public virtual Instance And(Instance i1, Instance i2)
    {
        throw MakeUnsupported(i1, i2);
    }

    public virtual Instance BitAnd(Instance i1, Instance i2)
    {
        throw MakeUnsupported(i1, i2);
    }

    public virtual Instance Or(Instance i1, Instance i2)
    {
        throw MakeUnsupported(i1, i2);
    }

    public virtual Instance Equal(Instance i1, Instance i2)
    {
        throw MakeUnsupported(i1, i2);
    }

    public virtual Instance NotEqual(Instance i1, Instance i2)
    {
        throw MakeUnsupported(i1, i2);
    }

    public virtual Instance Greater(Instance i1, Instance i2)
    {
        throw MakeUnsupported(i1, i2);
    }

    public virtual Instance Smaller(Instance i1, Instance i2)
    {
        throw MakeUnsupported(i1, i2);
    }

    public virtual Instance GreaterEqual(Instance i1, Instance i2)
    {
        throw MakeUnsupported(i1, i2);
    }

    public virtual Instance SmallerEqual(Instance i1, Instance i2)
    {
        throw MakeUnsupported(i1, i2);
    }

    public virtual bool IsConvertableFrom(Type type)
    {
        return false;
    }

    public virtual Instance Call(FunctionInstance function, Instance[] args)
    {
        throw MakeUnsupported(args);
    }

    public virtual Instance CastFrom(Instance i)
    {
        throw MakeUnsupportedCastError(i);
    }


    private UnsupportedCastError MakeUnsupportedCastError(Instance i, string systemMsg = "")
    {
        return new UnsupportedCastError(
            string.Format(strings.Type_MakeUnsupportedCastError, i.Type, this, systemMsg.Length != 0 ? ": " : "",
                systemMsg), null);
    }

    public Instance HandledCastFrom(Instance i)
    {
        try
        {
            return CastFrom(i);
        }
        catch (FormatException e)
        {
            throw MakeUnsupportedCastError(i, e.Message);
        }
        catch (InvalidCastException e)
        {
            throw MakeUnsupportedCastError(i, e.Message);
        }
    }

    public virtual Instance Assign(Instance to, Instance value)
    {
        if (to.Type.Id != value.Type.Id)
        {
            var casted = HandledCastFrom(value);
            to.Value = casted.Value;
            to.Type = casted.Type;
        }
        else
        {
            to.Value = value.Value;
        }

        to.Members.Clear();
        foreach (var p in value.Members) to.Members.Add(p.Key, p.Value);
        return to;
    }


    public virtual void WriteBinary(Instance i, BinaryWriter writer)
    {
        throw new FileError($"Cannot write {this}", null);
    }

    public virtual Instance ReadBinary(BinaryReader reader)
    {
        throw new FileError($"Cannot read {this}", null);
    }

    public override string ToString()
    {
        return Name;
    }
}