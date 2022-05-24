using System.Collections;
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
    public virtual uint Id { get; set; } = ++_incrementId;
    public virtual string Name { get; set; } = null!;
    public Dictionary<string, Type> Members { get; } = new();
    public Scope ParentScope;

    public virtual Instance Instance(object value = null)
    {
        var instance = new Instance
        {
            Type = this,
            Members = new Dictionary<string, Instance>()
        };
        foreach (var member in Members)
        {
            instance.Members[member.Key] = member.Value.Instance();
        }

        return instance;
    }

    public virtual Instance Add(Instance i1, Instance i2) => throw new NotSupportedException();
    public virtual Instance Subtract(Instance i1, Instance i2) => throw new NotSupportedException();
    public virtual Instance Multiply(Instance i1, Instance i2) => throw new NotSupportedException();
    public virtual Instance Divide(Instance i1, Instance i2) => throw new NotSupportedException();
    public virtual Instance IntDivide(Instance i1, Instance i2) => throw new NotSupportedException();
    public virtual Instance Mod(Instance i1, Instance i2) => throw new NotSupportedException();
    public virtual Instance Pow(Instance i1, Instance i2) => throw new NotSupportedException();
    public virtual Instance Negative(Instance i) => throw new NotSupportedException();

    public virtual Instance Index(Instance i) => throw new NotSupportedException();

    public virtual Instance Not(Instance i) => throw new NotSupportedException();
    public virtual Instance And(Instance i1, Instance i2) => throw new NotSupportedException();
    public virtual Instance Or(Instance i1, Instance i2) => throw new NotSupportedException();

    public virtual Instance Equal(Instance i1, Instance i2) => throw new NotSupportedException();
    public virtual Instance Greater(Instance i1, Instance i2) => throw new NotSupportedException();
    public virtual Instance Smaller(Instance i1, Instance i2) => throw new NotSupportedException();
    public virtual Instance GreaterEqual(Instance i1, Instance i2) => throw new NotSupportedException();
    public virtual Instance SmallerEqual(Instance i1, Instance i2) => throw new NotSupportedException();

    public virtual Instance CastFrom(Instance i) => throw new InvalidCastException($"{i.Type} to {this}");
    
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
        foreach (var p in value.Members)
        {
            to.Members.Add(p.Key, p.Value);
        }
    }

    public override string ToString()
    {
        return $"[{Name} ${Id}]";
    }
}