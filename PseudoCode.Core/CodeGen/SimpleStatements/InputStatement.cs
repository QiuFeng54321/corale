using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.CodeGen.Expressions;
using PseudoCode.Core.Formatting;
using PseudoCode.Core.Runtime.Reflection;
using PseudoCode.Core.Runtime.Reflection.Builtin;

namespace PseudoCode.Core.CodeGen.SimpleStatements;

public class InputStatement : Statement
{
    private static Symbol _scanFunctionGroup;
    private readonly Expression _expression;

    public InputStatement(Expression expression)
    {
        _expression = expression;
    }

    public override void Format(PseudoFormatter formatter)
    {
        formatter.WriteStatement($"INPUT {_expression.ToFormatString()}");
    }

    public static void MakeConstants()
    {
        _scanFunctionGroup = FunctionBinder.FunctionGroupMap[nameof(Scanner.Scan)];
    }

    public override void CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        var param = _expression.CodeGen(ctx, cu, function);
        CallExpression.CodeGenCallFuncGroup(ctx, cu, _scanFunctionGroup, new[] { param });
    }
}