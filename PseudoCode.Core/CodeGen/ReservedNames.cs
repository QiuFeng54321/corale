namespace PseudoCode.Core.CodeGen;

public static class ReservedNames
{
    public const string String = "str";
    public const string Integer = "int";
    public const string Char = "char";
    public const string Real = "real";
    public const string Boolean = "bool";
    public const string Load = "load";
    public const string Type = "type";
    public const string Pointer = "ptr";
    public const string Function = "func";
    public const string Main = "__main";
    public const string Temp = "_";

    public static readonly Dictionary<Types, string> Map = new()
    {
        [Types.Boolean] = Boolean,
        [Types.Integer] = Integer,
        [Types.Real] = Real,
        [Types.Character] = Char,
        [Types.Type] = Type,
        [Types.Pointer] = Pointer,
        [Types.Function] = Function
    };

    public static string ToTemp(this Types type)
    {
        return Map.GetValueOrDefault(type, Temp);
    }

    public static string RequestTemp(this Types type, CodeGenContext ctx)
    {
        return ctx.NameGenerator.RequestTemp(type.ToTemp());
    }
}