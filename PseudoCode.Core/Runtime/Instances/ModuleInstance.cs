using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Runtime.Instances;

public class ModuleInstance : Instance
{
    public ModuleType ModuleType => (ModuleType)Type;
}