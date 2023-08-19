using PseudoCode.Core.Runtime.Errors;
using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types;

public class SetType : Type
{
    public Type ElementType;

    public SetType(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override string Name => "SET";
    public override uint Id => SetId;

    public override Instance Instance(object value = null, Scope scope = null)
    {
        return DefaultInstance<Instance>(value ?? new HashSet<Instance>(Instances.Instance.SetComparer), scope);
    }

    public override Instance Assign(Instance to, Instance value)
    {
        if (to.Type is not SetType toType || value.Type is not SetType valueType
                                          || !toType.ElementType.IsConvertableFrom(valueType.ElementType))
            throw new InvalidTypeError($"Cannot assign {value.Type} to {to.Type}", null);

        to.Value = toType.CastFrom(value).Value;
        return to;
    }

    public override bool IsConvertableFrom(Type type)
    {
        return type is SetType;
    }

    public override Instance CastFrom(Instance i)
    {
        if (i.Type is not SetType otherSetType) throw MakeUnsupported(i.Type, this);

        if (ElementType == otherSetType.ElementType || ElementType is AnyType) return i;
        var newSet = new HashSet<Instance>(Instances.Instance.SetComparer);
        foreach (var item in i.Get<HashSet<Instance>>()) newSet.Add(ElementType.CastFrom(item));

        var newSetInstance = Instance(newSet);
        return newSetInstance;
    }

    public override Type BinaryResultType(PseudoOperator type, Type right)
    {
        if (right is not SetType rightSetType || !ElementType.IsConvertableFrom(rightSetType.ElementType))
            return new NullType(ParentScope, Program);
        return this;
    }

    public override Instance Add(Instance i1, Instance i2)
    {
        return Instance(
            i1.Get<HashSet<Instance>>()
                .Union(HandledCastFrom(i2).Get<HashSet<Instance>>(), Instances.Instance.SetComparer)
                .ToHashSet(Instances.Instance.SetComparer), ParentScope);
    }

    public override Instance Subtract(Instance i1, Instance i2)
    {
        return Instance(
            i1.Get<HashSet<Instance>>()
                .Except(HandledCastFrom(i2).Get<HashSet<Instance>>(), Instances.Instance.SetComparer)
                .ToHashSet(Instances.Instance.SetComparer), ParentScope);
    }

    public override Instance BitAnd(Instance i1, Instance i2)
    {
        return Instance(
            i1.Get<HashSet<Instance>>()
                .Intersect(HandledCastFrom(i2).Get<HashSet<Instance>>(), Instances.Instance.SetComparer)
                .ToHashSet(Instances.Instance.SetComparer), ParentScope);
    }
}