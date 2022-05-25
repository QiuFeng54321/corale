using System.Runtime.CompilerServices;
using PseudoCode.Runtime.Operations;

namespace PseudoCode.Runtime;

public class Type
{
    public const uint IntegerId = 0,
        RealId = 1,
        StringId = 2,
        BooleanId = 3,
        CharId = 4,
        DateId = 5,
        ArrayId = 6;

    private static uint _incrementId = ArrayId;
    public Scope ParentScope;
    public virtual uint Id { get; set; } = ++_incrementId;
    public virtual string Name { get; set; } = null!;
    public Dictionary<string, Type> Members { get; } = new();

    public virtual Instance Instance(object value = null)
    {
        var instance = new Instance
        {
            Type = this,
            Members = new Dictionary<string, Instance>()
        };
        foreach (var member in Members) instance.Members[member.Key] = member.Value.Instance();

        return instance;
    }

    public Type()
    {
        BinaryOperators = new()
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
            { PseudoCodeLexer.Or, Or },
        };
        UnaryOperators = new()
        {
            { PseudoCodeLexer.Subtract, Negative },
            { PseudoCodeLexer.Not, Not },
        };
    }

    public delegate Instance BinaryOperator(Instance i1, Instance i2);

    public delegate Instance UnaryOperator(Instance i);

    public Dictionary<int, BinaryOperator> BinaryOperators = new();
    public Dictionary<int, UnaryOperator> UnaryOperators = new();

    public virtual void ThrowUnsupported(Instance i1, Instance i2 = null, [CallerMemberName] string caller = "Unknown")
    {
        throw new NotSupportedException(
            $"Cannot perform {caller} operation on {i1.Type} {(i2 != null ? "and " + i2.Type : "")}");
    }

    public virtual Instance Add(Instance i1, Instance i2)
    {
        ThrowUnsupported(i1, i2);
        return null;
    }

    public virtual Instance Subtract(Instance i1, Instance i2)
    {
        ThrowUnsupported(i1, i2);
        return null;
    }

    public virtual Instance Multiply(Instance i1, Instance i2)
    {
        ThrowUnsupported(i1, i2);
        return null;
    }

    public virtual Instance Divide(Instance i1, Instance i2)
    {
        ThrowUnsupported(i1, i2);
        return null;
    }

    public virtual Instance IntDivide(Instance i1, Instance i2)
    {
        ThrowUnsupported(i1, i2);
        return null;
    }

    public virtual Instance Mod(Instance i1, Instance i2)
    {
        ThrowUnsupported(i1, i2);
        return null;
    }

    public virtual Instance Pow(Instance i1, Instance i2)
    {
        ThrowUnsupported(i1, i2);
        return null;
    }

    public virtual Instance Negative(Instance i)
    {
        ThrowUnsupported(i);
        return null;
    }

    public virtual Instance Index(Instance i1, Instance i2)
    {
        ThrowUnsupported(i1, i2);
        return null;
    }

    public virtual Instance Not(Instance i)
    {
        ThrowUnsupported(i);
        return null;
    }

    public virtual Instance And(Instance i1, Instance i2)
    {
        ThrowUnsupported(i1, i2);
        return null;
    }

    public virtual Instance Or(Instance i1, Instance i2)
    {
        ThrowUnsupported(i1, i2);
        return null;
    }

    public virtual Instance Equal(Instance i1, Instance i2)
    {
        ThrowUnsupported(i1, i2);
        return null;
    }

    public virtual Instance NotEqual(Instance i1, Instance i2)
    {
        ThrowUnsupported(i1, i2);
        return null;
    }

    public virtual Instance Greater(Instance i1, Instance i2)
    {
        ThrowUnsupported(i1, i2);
        return null;
    }

    public virtual Instance Smaller(Instance i1, Instance i2)
    {
        ThrowUnsupported(i1, i2);
        return null;
    }

    public virtual Instance GreaterEqual(Instance i1, Instance i2)
    {
        ThrowUnsupported(i1, i2);
        return null;
    }

    public virtual Instance SmallerEqual(Instance i1, Instance i2)
    {
        ThrowUnsupported(i1, i2);
        return null;
    }

    public virtual Instance CastFrom(Instance i)
    {
        throw new InvalidCastException($"{i.Type} to {this}");
    }

    public virtual void Assign(Instance to, Instance value)
    {
        if (to.Type.Id != value.Type.Id)
        {
            var casted = to.Type.CastFrom(value);
            to.Value = casted.Value;
            to.Type = casted.Type;
        }
        else
        {
            to.Value = value.Value;
        }

        to.Members.Clear();
        foreach (var p in value.Members) to.Members.Add(p.Key, p.Value);
    }

    public override string ToString()
    {
        return $"[{Name} ${Id}]";
    }
}