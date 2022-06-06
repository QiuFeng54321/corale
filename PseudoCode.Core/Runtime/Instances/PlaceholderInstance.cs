using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Instances;

/// <summary>
/// Acts as placeholder variable when the variable is not declared.
/// CAIE allows this to happen but not on arrays
/// </summary>
public class PlaceholderInstance : Instance
{

    public string Name => Get<string>();
    public PlaceholderInstance(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }
    
}