using PseudoCode.Core.Runtime;

namespace PseudoCode.Core.CodeGen;

public class Symbol
{
    public DefinitionAttribute DefinitionAttribute;
    public bool IsType;
    public string Name;
    public Type Type;

    public Symbol(string name, bool isType, Type type, DefinitionAttribute definitionAttribute)
    {
        Name = name;
        IsType = isType;
        Type = type;
        DefinitionAttribute = definitionAttribute;
    }

    public Symbol Clone()
    {
        return new Symbol(Name, IsType, Type?.Clone(), DefinitionAttribute);
    }
}