namespace PseudoCode.Core.Runtime.Reflection;

public struct BlittableBoolean
{
    private byte _byteValue;

    public bool Value
    {
        get => Convert.ToBoolean(_byteValue);
        set => _byteValue = Convert.ToByte(value);
    }

    public static explicit operator BlittableBoolean(bool value)
    {
        return new BlittableBoolean { Value = value };
    }

    public static implicit operator bool(BlittableBoolean value)
    {
        return value.Value;
    }
}