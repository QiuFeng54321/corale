using System.Text.RegularExpressions;
using Newtonsoft.Json;
using PseudoCode.Core.Runtime.Operations;
using PseudoCode.Core.Runtime.Types;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Instances;

public class Instance
{
    public static Instance Null;
    public uint InstanceAddress;
    public virtual Instance ParentInstance { get; set; } = null;
    [JsonIgnore] public Scope ParentScope;
    [JsonIgnore] public PseudoProgram Program;
    

    public Instance()
    {
    }

    public Instance(Scope parentScope, PseudoProgram program)
    {
        ParentScope = parentScope;
        Program = program;
    }

    public virtual Type Type { get; set; }
    public virtual Dictionary<string, Instance> Members { get; init; } = new();
    public virtual object Value { get; set; } = null!;

    [JsonIgnore] public virtual Instance RealInstance => this;

    public virtual T Get<T>()
    {
        return (T)Value;
    }


    public virtual string Represent()
    {
        return Type switch
        {
            DateType => Get<DateOnly>().ToString("dd/MM/yyyy"),
            BooleanType => Value?.ToString()?.ToUpper(),
            NullType => "NULL",
            TypeType => $"{Type.Name} {MembersString()}",
            // Type.StringId or Type.CharId or Type.IntegerId or Type.RealId => Value?.ToString(),
            _ => Value?.ToString()
        } ?? "NULL";
    }

    private string MembersString()
    {
        return $"{{{(Members == null ? "" : string.Join(", ", Members.Select(m => $"{m.Key} = {m.Value}")))}}}";
    }
    

    public virtual string DebugRepresent()
    {
        var represent = Represent();
        if (Type.Id is Type.StringId or Type.CharId) represent = Regex.Escape(represent);
        return
            $"{{{Type} {represent} {MembersString()}}}";
    }

    public override string ToString()
    {
        return Program?.DebugRepresentation ?? false ? DebugRepresent() : Represent();
    }
}