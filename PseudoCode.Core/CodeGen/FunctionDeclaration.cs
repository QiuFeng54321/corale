using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.CodeGen.TypeLookups;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen;

public class FunctionDeclaration : Statement, IGenericExpression
{
    public readonly List<ArgumentType> Arguments;
    public readonly Block FunctionBody;
    public readonly GenericDeclaration GenericDeclaration;
    public readonly string Name;
    public readonly DataType ReturnType;

    public FunctionDeclaration(string name, List<ArgumentType> arguments, DataType returnType,
        Block functionBody, GenericDeclaration genericDeclaration = default)
    {
        GenericDeclaration = genericDeclaration;
        Name = name;
        Arguments = arguments;
        ReturnType = returnType;
        FunctionBody = functionBody;
    }

    public Symbol Generate(CodeGenContext ctx, Function function, List<Symbol> genericParams = default)
    {
        // Make function name
        var funcName = MakeFunctionName(genericParams);
        // Return existing function
        if (function.BodyNamespace.TryGetSymbol(Name, out var res))
            if (res.FunctionOverloads.FirstOrDefault(o => o.Name == funcName) is { } f)
                return res;
        // Fill generic parameters
        var subNs = function.BodyNamespace.AddNamespace(funcName);
        if (genericParams != null)
            for (var i = 0; i < genericParams.Count; i++)
                subNs.AddSymbol(genericParams[i], false, GenericDeclaration.Identifiers[i]);
        var filledArgumentSymbols =
            Arguments.Select(a =>
            {
                var dataTypeSymbol = a.DataType.Lookup(ctx, function, subNs);
                return new Symbol(a.Name, false, dataTypeSymbol.Type, dataTypeSymbol.Namespace,
                    a.IsRef ? DefinitionAttribute.Reference : DefinitionAttribute.None);
            });
        var filledReturnType = ReturnType.Lookup(ctx, function, subNs);

        // Make function
        var func = ctx.CompilationUnit.MakeFunction(Name, filledArgumentSymbols.ToList(), filledReturnType,
            function.BodyNamespace,
            addToList: false);
        func.BodyNamespace = subNs;
        // func.GeneratePrototype(ctx);
        FunctionBody.ParentFunction = func;
        func.ParentFunction = function;
        func.Block = FunctionBody;
        func.CodeGen(ctx, function);
        func.ResultFunction.Name = funcName;
        return func.ResultFunctionGroup;
    }

    public string MakeFunctionName(List<Symbol> genericParams = default)
    {
        if (genericParams == null) return Name;
        return $"{Name}<{string.Join(", ", genericParams.Select(g => g.Type.TypeName))}>";
    }

    public override void Format(PseudoFormatter formatter)
    {
        formatter.WriteStatement(
            $"FUNCTION {Name}{GenericDeclaration}({string.Join(", ", Arguments.Select(a => $"{a.Name} : {a.DataType}"))}) RETURNS {ReturnType}");
        FunctionBody.Format(formatter);
        formatter.WriteStatement("ENDFUNCTION");
    }

    public override void CodeGen(CodeGenContext ctx, Function function)
    {
        Generate(ctx, function, new List<Symbol>());
    }

    public class ArgumentType
    {
        public DataType DataType;
        public bool IsRef;
        public string Name;
    }
}