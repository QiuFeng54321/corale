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
        EnumId = 8,
        PointerId = 9,
        ArrayId = 10,
        SetId = 11,
        AnyId = 12;

    private static uint _incrementId = ArrayId;

    [JsonIgnore] public readonly Dictionary<PseudoOperator, BinaryOperator> BinaryOperators;
    [JsonIgnore] public readonly PseudoProgram Program;
    [JsonIgnore] public readonly Dictionary<PseudoOperator, UnaryOperator> UnaryOperators;
    [JsonIgnore] public Scope ParentScope;

    public Type(Scope parentScope, PseudoProgram program)
    {
        Program = program;
        ParentScope = parentScope;
        BinaryOperators = new Dictionary<PseudoOperator, BinaryOperator>
        {
            { PseudoOperator.Add, Add },
            { PseudoOperator.Multiply, Multiply },
            { PseudoOperator.Divide, Divide },
            { PseudoOperator.Subtract, Subtract },
            { PseudoOperator.Pow, Pow },
            { PseudoOperator.IntDivide, IntDivide },
            { PseudoOperator.Mod, Mod },
            { PseudoOperator.Greater, Greater },
            { PseudoOperator.GreaterEqual, GreaterEqual },
            { PseudoOperator.SmallerEqual, SmallerEqual },
            { PseudoOperator.Smaller, Smaller },
            { PseudoOperator.Equal, Equal },
            { PseudoOperator.NotEqual, NotEqual },
            { PseudoOperator.And, And },
            { PseudoOperator.BitAnd, BitAnd },
            { PseudoOperator.Or, Or }
        };
        UnaryOperators = new Dictionary<PseudoOperator, UnaryOperator>
        {
            { PseudoOperator.Subtract, Negative },
            { PseudoOperator.Not, Not },
            { PseudoOperator.GetPointer, GetPointer },
            { PseudoOperator.GetPointed, GetPointed }
        };
    }


    public virtual bool Serializable => false;

    public virtual uint Id { get; set; } = ++_incrementId;
    public virtual string Name { get; set; } = null!;
    public virtual uint Size => (uint)(Members?.Count ?? 0 + 1);
    public virtual Dictionary<string, Definition> Members { get; init; } = new();

    public virtual Instance Instance(object value = null, Scope scope = null)
    {
        return DefaultInstance<Instance>(value, scope);
    }

    public virtual T DefaultInstance<T>(object value = null, Scope scope = null) where T : Instance, new()
    {
        var instance = new T
        {
            Type = this,
            Members = new Dictionary<string, Instance>(),
            Value = value,
            Program = Program,
            ParentScope = scope ?? ParentScope
        };
        foreach (var member in Members)
        {
            var memberInstance = member.Value.ConstantInstance?.Type?.Clone(member.Value.ConstantInstance) ??
                                 member.Value.Type.Instance(scope: ParentScope);
            memberInstance.ParentInstance = instance;
            instance.Members[member.Key] = new ReferenceInstance(ParentScope, Program)
            {
                ReferenceAddress = Program.AllocateId(memberInstance)
            };
        }

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
            string.Format(strings.Type_ThrowUnsupported, caller, i1, i2 != null ? strings.and + i2 : ""), null);
    }

    public virtual Type BinaryResultType(PseudoOperator type, Type right)
    {
        return new NullType(ParentScope, Program);
    }

    public virtual Type UnaryResultType(PseudoOperator type)
    {
        if (type == PseudoOperator.GetPointer) return new PointerType(ParentScope, Program) { PointedType = this };
        return new NullType(ParentScope, Program);
    }

    public virtual Type MemberAccessResultType(string member)
    {
        return MemberAccessResultDefinition(member)?.Type ?? new NullType(ParentScope, Program);
    }

    public virtual Definition MemberAccessResultDefinition(string member)
    {
        return !Members.ContainsKey(member)
            ? new Definition(ParentScope, Program)
            {
                Name = member,
                Type = new NullType(ParentScope, Program),
                Attributes = Definition.Attribute.Reference
            }
            : Members[member];
    }

    public virtual Instance GetPointer(Instance i)
    {
        return new PointerType(i.ParentScope, i.Program)
        {
            PointedType = i.Type
        }.Instance(i.RealInstance.InstanceAddress);
    }

    public virtual Instance GetPointed(Instance i)
    {
        throw MakeUnsupported(i);
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

    public virtual Instance MemberAccess(Instance i1, string member)
    {
        if (!i1.Members.ContainsKey(member))
            throw new InvalidAccessError($"Accessing non-existent member {member} on {i1.Type}", null);

        return i1.Members[member];
    }

    public virtual bool IsConvertableFrom(Type type)
    {
        return type == this;
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

    public override string ToString()
    {
        return Name;
    }
}