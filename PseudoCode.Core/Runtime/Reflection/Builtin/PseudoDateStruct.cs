using System.Runtime.InteropServices;

namespace PseudoCode.Core.Runtime.Reflection.Builtin;

[StructLayout(LayoutKind.Sequential)]
[BuiltinType("DATE")]
public struct PseudoDateStruct
{
    public int Year;
    public int Month;
    public int Day;

    public PseudoDateStruct(int year, int month, int day)
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