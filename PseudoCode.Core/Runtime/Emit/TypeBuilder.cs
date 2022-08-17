using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Runtime.Emit;

public class TypeBuilder
{
    public Dictionary<string, Identifier> StaticMembers = new();
    public Dictionary<string, Identifier> Members = new();
    public Dictionary<PseudoOperator, Identifier> OverriddenOperators = new();
    
}