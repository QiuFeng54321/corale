using LLVMSharp.Interop;
using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.CodeGen.Expressions;
using PseudoCode.Core.Formatting;
using PseudoCode.Core.Runtime.Reflection;
using PseudoCode.Core.Runtime.Reflection.Builtin;

namespace PseudoCode.Core.CodeGen.SimpleStatements;

public class OutputStatement : Statement
{
    private static Symbol _printFunctionGroup;
    private static Symbol _spaceChar;
    private static Symbol _newlineChar;
    private readonly List<Expression> _expressions;

    public OutputStatement(List<Expression> expressions)
    {
        _expressions = expressions;
    }

    public override void Format(PseudoFormatter formatter)
    {
        formatter.WriteStatement($"OUTPUT {string.Join(", ", _expressions.Select(e => e.ToFormatString()))}");
    }

    public static void MakeConstants()
    {
        _printFunctionGroup = FunctionBinder.FunctionGroupMap[nameof(Printer.Printf)];
        _spaceChar = new Symbol("__space", false, BuiltinTypes.Char.Type,
            definitionAttribute: DefinitionAttribute.Const)
        {
            ValueRef = LLVMValueRef.CreateConstInt(BuiltinTypes.Char.Type.GetLLVMType(), ' ')
        };
        _newlineChar = new Symbol("__newline", false, BuiltinTypes.Char.Type,
            definitionAttribute: DefinitionAttribute.Const)
        {
            ValueRef = LLVMValueRef.CreateConstInt(BuiltinTypes.Char.Type.GetLLVMType(), '\n')
        };
    }

    public override void CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        for (var i = 0; i < _expressions.Count; i++)
        {
            var param = _expressions[i].CodeGen(ctx, cu, function);
            CallExpression.CodeGenCallFuncGroup(ctx, cu, _printFunctionGroup, new[] { param });
            CallExpression.CodeGenCallFuncGroup(ctx, cu, _printFunctionGroup,
                new[] { i == _expressions.Count - 1 ? _newlineChar : _spaceChar });
        }
    }
}