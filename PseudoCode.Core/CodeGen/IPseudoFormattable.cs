using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen;

public interface IPseudoFormattable
{
    public void Format(PseudoFormatter formatter);
}