using System.Runtime.InteropServices;

namespace PseudoCode.Core.Runtime.Reflection.Builtin;

[StructLayout(LayoutKind.Sequential)]
[BuiltinType("DATE")]
public struct PseudoDateStruct
{
    public long Year;
    public long Month;
    public long Day;

    public PseudoDateStruct(long year, long month, long day)
    {
        Year = year;
        Month = month;
        Day = day;
    }

    public static implicit operator PseudoDateStruct(DateOnly dateOnly)
    {
        return new PseudoDateStruct(dateOnly.Year, dateOnly.Month, dateOnly.Day);
    }
}