//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.10.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from PseudoCode.g4 by ANTLR 4.10.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419


using PseudoCode.Runtime;
using Range = PseudoCode.Runtime.Range;


using Antlr4.Runtime.Misc;
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="IPseudoCodeListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.10.1")]
[System.Diagnostics.DebuggerNonUserCode]
[System.CLSCompliant(false)]
public partial class PseudoCodeBaseListener : IPseudoCodeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.fileInput"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFileInput([NotNull] PseudoCodeParser.FileInputContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.fileInput"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFileInput([NotNull] PseudoCodeParser.FileInputContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.singleInput"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterSingleInput([NotNull] PseudoCodeParser.SingleInputContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.singleInput"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitSingleInput([NotNull] PseudoCodeParser.SingleInputContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.statement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterStatement([NotNull] PseudoCodeParser.StatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.statement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitStatement([NotNull] PseudoCodeParser.StatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.simpleStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterSimpleStatement([NotNull] PseudoCodeParser.SimpleStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.simpleStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitSimpleStatement([NotNull] PseudoCodeParser.SimpleStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.smallStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterSmallStatement([NotNull] PseudoCodeParser.SmallStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.smallStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitSmallStatement([NotNull] PseudoCodeParser.SmallStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.assignmentStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAssignmentStatement([NotNull] PseudoCodeParser.AssignmentStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.assignmentStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAssignmentStatement([NotNull] PseudoCodeParser.AssignmentStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.declarationStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDeclarationStatement([NotNull] PseudoCodeParser.DeclarationStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.declarationStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDeclarationStatement([NotNull] PseudoCodeParser.DeclarationStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.constantStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterConstantStatement([NotNull] PseudoCodeParser.ConstantStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.constantStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitConstantStatement([NotNull] PseudoCodeParser.ConstantStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.ioStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterIoStatement([NotNull] PseudoCodeParser.IoStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.ioStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitIoStatement([NotNull] PseudoCodeParser.IoStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.returnStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterReturnStatement([NotNull] PseudoCodeParser.ReturnStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.returnStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitReturnStatement([NotNull] PseudoCodeParser.ReturnStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.callStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterCallStatement([NotNull] PseudoCodeParser.CallStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.callStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitCallStatement([NotNull] PseudoCodeParser.CallStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.fileStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFileStatement([NotNull] PseudoCodeParser.FileStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.fileStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFileStatement([NotNull] PseudoCodeParser.FileStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.compoundStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterCompoundStatement([NotNull] PseudoCodeParser.CompoundStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.compoundStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitCompoundStatement([NotNull] PseudoCodeParser.CompoundStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.block"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBlock([NotNull] PseudoCodeParser.BlockContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.block"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBlock([NotNull] PseudoCodeParser.BlockContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.aligned_block"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAligned_block([NotNull] PseudoCodeParser.Aligned_blockContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.aligned_block"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAligned_block([NotNull] PseudoCodeParser.Aligned_blockContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.ifStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterIfStatement([NotNull] PseudoCodeParser.IfStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.ifStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitIfStatement([NotNull] PseudoCodeParser.IfStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.forStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterForStatement([NotNull] PseudoCodeParser.ForStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.forStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitForStatement([NotNull] PseudoCodeParser.ForStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.whileStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterWhileStatement([NotNull] PseudoCodeParser.WhileStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.whileStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitWhileStatement([NotNull] PseudoCodeParser.WhileStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.repeatStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterRepeatStatement([NotNull] PseudoCodeParser.RepeatStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.repeatStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitRepeatStatement([NotNull] PseudoCodeParser.RepeatStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.caseStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterCaseStatement([NotNull] PseudoCodeParser.CaseStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.caseStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitCaseStatement([NotNull] PseudoCodeParser.CaseStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.caseBranch"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterCaseBranch([NotNull] PseudoCodeParser.CaseBranchContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.caseBranch"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitCaseBranch([NotNull] PseudoCodeParser.CaseBranchContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.caseBody"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterCaseBody([NotNull] PseudoCodeParser.CaseBodyContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.caseBody"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitCaseBody([NotNull] PseudoCodeParser.CaseBodyContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.valueRange"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterValueRange([NotNull] PseudoCodeParser.ValueRangeContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.valueRange"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitValueRange([NotNull] PseudoCodeParser.ValueRangeContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.procedureDefinition"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterProcedureDefinition([NotNull] PseudoCodeParser.ProcedureDefinitionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.procedureDefinition"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitProcedureDefinition([NotNull] PseudoCodeParser.ProcedureDefinitionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.functionDefinition"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFunctionDefinition([NotNull] PseudoCodeParser.FunctionDefinitionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.functionDefinition"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFunctionDefinition([NotNull] PseudoCodeParser.FunctionDefinitionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.argumentsDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterArgumentsDeclaration([NotNull] PseudoCodeParser.ArgumentsDeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.argumentsDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitArgumentsDeclaration([NotNull] PseudoCodeParser.ArgumentsDeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.argumentDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterArgumentDeclaration([NotNull] PseudoCodeParser.ArgumentDeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.argumentDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitArgumentDeclaration([NotNull] PseudoCodeParser.ArgumentDeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.tuple"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTuple([NotNull] PseudoCodeParser.TupleContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.tuple"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTuple([NotNull] PseudoCodeParser.TupleContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.enumDefinition"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterEnumDefinition([NotNull] PseudoCodeParser.EnumDefinitionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.enumDefinition"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitEnumDefinition([NotNull] PseudoCodeParser.EnumDefinitionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.pointerDefinition"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPointerDefinition([NotNull] PseudoCodeParser.PointerDefinitionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.pointerDefinition"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPointerDefinition([NotNull] PseudoCodeParser.PointerDefinitionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.typeDefinition"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTypeDefinition([NotNull] PseudoCodeParser.TypeDefinitionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.typeDefinition"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTypeDefinition([NotNull] PseudoCodeParser.TypeDefinitionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.typeBody"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTypeBody([NotNull] PseudoCodeParser.TypeBodyContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.typeBody"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTypeBody([NotNull] PseudoCodeParser.TypeBodyContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.typeChild"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTypeChild([NotNull] PseudoCodeParser.TypeChildContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.typeChild"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTypeChild([NotNull] PseudoCodeParser.TypeChildContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.classDefinition"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterClassDefinition([NotNull] PseudoCodeParser.ClassDefinitionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.classDefinition"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitClassDefinition([NotNull] PseudoCodeParser.ClassDefinitionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.classBody"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterClassBody([NotNull] PseudoCodeParser.ClassBodyContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.classBody"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitClassBody([NotNull] PseudoCodeParser.ClassBodyContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.classDataMember"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterClassDataMember([NotNull] PseudoCodeParser.ClassDataMemberContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.classDataMember"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitClassDataMember([NotNull] PseudoCodeParser.ClassDataMemberContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.classMethod"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterClassMethod([NotNull] PseudoCodeParser.ClassMethodContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.classMethod"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitClassMethod([NotNull] PseudoCodeParser.ClassMethodContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.accessLevel"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAccessLevel([NotNull] PseudoCodeParser.AccessLevelContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.accessLevel"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAccessLevel([NotNull] PseudoCodeParser.AccessLevelContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExpression([NotNull] PseudoCodeParser.ExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExpression([NotNull] PseudoCodeParser.ExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.logicExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLogicExpression([NotNull] PseudoCodeParser.LogicExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.logicExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLogicExpression([NotNull] PseudoCodeParser.LogicExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.comparisonOp"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterComparisonOp([NotNull] PseudoCodeParser.ComparisonOpContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.comparisonOp"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitComparisonOp([NotNull] PseudoCodeParser.ComparisonOpContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.arithmeticExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterArithmeticExpression([NotNull] PseudoCodeParser.ArithmeticExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.arithmeticExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitArithmeticExpression([NotNull] PseudoCodeParser.ArithmeticExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.lvalue"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLvalue([NotNull] PseudoCodeParser.LvalueContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.lvalue"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLvalue([NotNull] PseudoCodeParser.LvalueContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.arguments"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterArguments([NotNull] PseudoCodeParser.ArgumentsContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.arguments"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitArguments([NotNull] PseudoCodeParser.ArgumentsContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.atom"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAtom([NotNull] PseudoCodeParser.AtomContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.atom"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAtom([NotNull] PseudoCodeParser.AtomContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.dataType"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDataType([NotNull] PseudoCodeParser.DataTypeContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.dataType"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDataType([NotNull] PseudoCodeParser.DataTypeContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.basicDataType"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBasicDataType([NotNull] PseudoCodeParser.BasicDataTypeContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.basicDataType"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBasicDataType([NotNull] PseudoCodeParser.BasicDataTypeContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.arrayRange"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterArrayRange([NotNull] PseudoCodeParser.ArrayRangeContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.arrayRange"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitArrayRange([NotNull] PseudoCodeParser.ArrayRangeContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.array"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterArray([NotNull] PseudoCodeParser.ArrayContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.array"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitArray([NotNull] PseudoCodeParser.ArrayContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.number"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterNumber([NotNull] PseudoCodeParser.NumberContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.number"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitNumber([NotNull] PseudoCodeParser.NumberContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PseudoCodeParser.identifierWithNew"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterIdentifierWithNew([NotNull] PseudoCodeParser.IdentifierWithNewContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PseudoCodeParser.identifierWithNew"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitIdentifierWithNew([NotNull] PseudoCodeParser.IdentifierWithNewContext context) { }

	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void EnterEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void ExitEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitTerminal([NotNull] ITerminalNode node) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitErrorNode([NotNull] IErrorNode node) { }
}
