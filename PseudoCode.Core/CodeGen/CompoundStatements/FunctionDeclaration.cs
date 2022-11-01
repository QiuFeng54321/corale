using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.CodeGen.Expressions;
using PseudoCode.Core.CodeGen.TypeLookups;
using PseudoCode.Core.Formatting;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.CodeGen.CompoundStatements;

public class FunctionDeclaration : Statement, IGenericExpression
{
    public readonly List<ArgumentOrReturnType> Arguments;
    public readonly Block FunctionBody;
    public readonly GenericDeclaration GenericDeclaration;
    public readonly string Name;
    public readonly ArgumentOrReturnType ReturnType;
    public PseudoOperator Operator;

    public FunctionDeclaration(string name, List<ArgumentOrReturnType> arguments, ArgumentOrReturnType returnType,
        Block functionBody, GenericDeclaration genericDeclaration = default,
        PseudoOperator @operator = PseudoOperator.None)
    {
        GenericDeclaration = genericDeclaration;
        Name = name;
        Arguments = arguments;
        ReturnType = returnType;
        FunctionBody = functionBody;
        Operator = @operator;
    }

    public Symbol Generate(CodeGenContext ctx, CompilationUnit cu, Function function,
        List<Symbol> genericParams = default)
    {
        // Make function name
        // Fill generic parameters
        var subNs = function.BodyNamespace.AddNamespace(ctx.NameGenerator.Request(Name));
        if (genericParams != null)
            for (var i = 0; i < genericParams.Count; i++)
                subNs.AddSymbol(genericParams[i], false, GenericDeclaration.Identifiers[i]);
        var filledArgumentSymbols =
            Arguments.Select(a =>
            {
                var dataTypeSymbol = a.DataType.Lookup(ctx, cu, function, subNs);
                return new Symbol(a.Name, false, dataTypeSymbol.Type, dataTypeSymbol.Namespace,
                    a.IsRef ? DefinitionAttribute.Reference : DefinitionAttribute.None);
            }).ToList();
        var filledReturnTypeSym = ReturnType.DataType.Lookup(ctx, cu, function, subNs);
        var filledReturnSym = new Symbol(ReturnType.Name, false, filledReturnTypeSym.Type,
            filledReturnTypeSym.Namespace, ReturnType.IsRef ? DefinitionAttribute.Reference : DefinitionAttribute.None);
        var funcName = MakeFunctionName(Name, filledReturnSym, genericParams ?? new List<Symbol>());
        // Return existing function
        if (function.BodyNamespace.TryGetSymbol(Name, out var res))
            if (res.FindFunctionOverload(filledArgumentSymbols) is { })
                return res;

        // Make function
        var func = cu.MakeFunction(Name, filledArgumentSymbols.ToList(), filledReturnSym,
            function.BodyNamespace,
            addToList: false);
        func.BodyNamespace = subNs;
        func.Operator = Operator;
        func.DebugInformation = DebugInformation;
        // func.GeneratePrototype(ctx);
        FunctionBody.ParentFunction = func;
        func.ParentFunction = function;
        func.Block = FunctionBody;
        func.CodeGen(ctx, cu, function);
        return func.ResultFunctionGroup;
    }

    public static string MakeFunctionName(string name, Symbol returnSym, IEnumerable<Symbol> genericParams)
    {
        return
            $"{name}<{string.Join(", ", genericParams.Select(g => g.Namespace.GetFullQualifier(g.Type.TypeName)))}>:{returnSym.Namespace.GetFullQualifier(returnSym.Type.TypeName)}";
    }

    public override void Format(PseudoFormatter formatter)
    {
        formatter.WriteStatement(
            $"FUNCTION {Name}{GenericDeclaration}({string.Join(", ", Arguments.Select(a => $"{a.Name} : {a.DataType}"))}) RETURNS {ReturnType.DataType}");
        FunctionBody.Format(formatter);
        formatter.WriteStatement("ENDFUNCTION");
    }

    public override void CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        Generate(ctx, cu, function, new List<Symbol>());
    }

    public class ArgumentOrReturnType : AstNode
    {
        public DataType DataType;
        public bool IsRef;
        public string Name;
    }
}