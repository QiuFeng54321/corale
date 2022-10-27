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


using AntlrDenter;

using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.10.1")]
[System.CLSCompliant(false)]
public partial class PseudoCodeLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		Character=1, String=2, Date=3, NumberSequence=4, Boolean=5, NL=6, Newline=7, 
		Declare=8, Constant=9, IoKeyword=10, Array=11, Case=12, Of=13, Otherwise=14, 
		Endcase=15, For=16, To=17, Step=18, Next=19, While=20, Do=21, Endwhile=22, 
		Repeat=23, Until=24, If=25, Then=26, Else=27, Endif=28, Procedure=29, 
		Endprocedure=30, Call=31, Extern=32, Function=33, Endfunction=34, Byval=35, 
		Byref=36, Returns=37, Return=38, OperatorKeyword=39, EndOperator=40, Type=41, 
		Endtype=42, OpenFile=43, ReadFile=44, WriteFile=45, CloseFile=46, Seek=47, 
		GetRecord=48, PutRecord=49, Read=50, Write=51, Append=52, Random=53, Class=54, 
		Endclass=55, Inherits=56, Private=57, Public=58, New=59, Namespace=60, 
		EndNamespace=61, Use=62, Malloc=63, SizeOf=64, Import=65, And=66, Or=67, 
		Not=68, OpenParen=69, CloseParen=70, OpenBrack=71, CloseBrack=72, OpenBrace=73, 
		CloseBrace=74, Colon=75, Comma=76, Dot=77, Add=78, Subtract=79, Multiply=80, 
		BitAnd=81, IntDivide=82, Divide=83, Mod=84, Pow=85, Caret=86, Equal=87, 
		Greater=88, Smaller=89, GreaterEqual=90, SmallerEqual=91, NotEqual=92, 
		AssignmentNotation=93, NamespaceAccess=94, Identifier=95, Skip=96, UnknownChar=97;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"Character", "String", "Date", "NumberSequence", "Boolean", "NL", "Newline", 
		"Declare", "Constant", "IoKeyword", "Array", "Case", "Of", "Otherwise", 
		"Endcase", "For", "To", "Step", "Next", "While", "Do", "Endwhile", "Repeat", 
		"Until", "If", "Then", "Else", "Endif", "Procedure", "Endprocedure", "Call", 
		"Extern", "Function", "Endfunction", "Byval", "Byref", "Returns", "Return", 
		"OperatorKeyword", "EndOperator", "Type", "Endtype", "OpenFile", "ReadFile", 
		"WriteFile", "CloseFile", "Seek", "GetRecord", "PutRecord", "Read", "Write", 
		"Append", "Random", "Class", "Endclass", "Inherits", "Private", "Public", 
		"New", "Namespace", "EndNamespace", "Use", "Malloc", "SizeOf", "Import", 
		"And", "Or", "Not", "OpenParen", "CloseParen", "OpenBrack", "CloseBrack", 
		"OpenBrace", "CloseBrace", "Colon", "Comma", "Dot", "Add", "Subtract", 
		"Multiply", "BitAnd", "IntDivide", "Divide", "Mod", "Pow", "Caret", "Equal", 
		"Greater", "Smaller", "GreaterEqual", "SmallerEqual", "NotEqual", "AssignmentNotation", 
		"NamespaceAccess", "Identifier", "Skip", "UnknownChar", "NonZeroDigit", 
		"Digit", "Spaces", "Comment", "LineJoining", "IdStart", "IdContinue", 
		"SingleCharacter", "StringCharacters", "StringCharacter", "EscapeSequence", 
		"OctalEscape", "UnicodeEscape", "OctalDigit", "HexDigit", "ZeroToThree"
	};


	private DenterHelper denter;
	  
	public override IToken NextToken()
	{
	    if (denter == null)
	    {
	        denter = DenterHelper.Builder()
	            .Nl(NL)
	            .Indent(PseudoCodeParser.INDENT)
	            .Dedent(PseudoCodeParser.DEDENT)
	            .PullToken(base.NextToken);
	    }

	    return denter.NextToken();
	}


	public PseudoCodeLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public PseudoCodeLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, null, null, null, null, null, null, null, "'DECLARE'", "'CONSTANT'", 
		null, "'ARRAY'", "'CASE'", "'OF'", "'OTHERWISE'", "'ENDCASE'", "'FOR'", 
		"'TO'", "'STEP'", "'NEXT'", "'WHILE'", "'DO'", "'ENDWHILE'", "'REPEAT'", 
		"'UNTIL'", "'IF'", "'THEN'", "'ELSE'", "'ENDIF'", "'PROCEDURE'", "'ENDPROCEDURE'", 
		"'CALL'", "'EXTERN'", "'FUNCTION'", "'ENDFUNCTION'", "'BYVAL'", "'BYREF'", 
		"'RETURNS'", "'RETURN'", "'OPERATOR'", "'ENDOPERATOR'", "'TYPE'", "'ENDTYPE'", 
		"'OPENFILE'", "'READFILE'", "'WRITEFILE'", "'CLOSEFILE'", "'SEEK'", "'GETRECORD'", 
		"'PUTRECORD'", "'READ'", "'WRITE'", "'APPEND'", "'RANDOM'", "'CLASS'", 
		"'ENDCLASS'", "'INHERITS'", "'PRIVATE'", "'PUBLIC'", "'NEW'", "'NAMESPACE'", 
		"'ENDNAMESPACE'", "'USE'", "'MALLOC'", "'SIZEOF'", "'IMPORT'", "'AND'", 
		"'OR'", "'NOT'", "'('", "')'", "'['", "']'", "'{'", "'}'", "':'", "','", 
		"'.'", "'+'", "'-'", "'*'", "'&'", "'DIV'", "'/'", "'MOD'", "'POW'", "'^'", 
		"'='", "'>'", "'<'", "'>='", "'<='", "'<>'", "'<-'", "'::'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "Character", "String", "Date", "NumberSequence", "Boolean", "NL", 
		"Newline", "Declare", "Constant", "IoKeyword", "Array", "Case", "Of", 
		"Otherwise", "Endcase", "For", "To", "Step", "Next", "While", "Do", "Endwhile", 
		"Repeat", "Until", "If", "Then", "Else", "Endif", "Procedure", "Endprocedure", 
		"Call", "Extern", "Function", "Endfunction", "Byval", "Byref", "Returns", 
		"Return", "OperatorKeyword", "EndOperator", "Type", "Endtype", "OpenFile", 
		"ReadFile", "WriteFile", "CloseFile", "Seek", "GetRecord", "PutRecord", 
		"Read", "Write", "Append", "Random", "Class", "Endclass", "Inherits", 
		"Private", "Public", "New", "Namespace", "EndNamespace", "Use", "Malloc", 
		"SizeOf", "Import", "And", "Or", "Not", "OpenParen", "CloseParen", "OpenBrack", 
		"CloseBrack", "OpenBrace", "CloseBrace", "Colon", "Comma", "Dot", "Add", 
		"Subtract", "Multiply", "BitAnd", "IntDivide", "Divide", "Mod", "Pow", 
		"Caret", "Equal", "Greater", "Smaller", "GreaterEqual", "SmallerEqual", 
		"NotEqual", "AssignmentNotation", "NamespaceAccess", "Identifier", "Skip", 
		"UnknownChar"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "PseudoCode.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static PseudoCodeLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static int[] _serializedATN = {
		4,0,97,888,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
		6,2,7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,
		7,14,2,15,7,15,2,16,7,16,2,17,7,17,2,18,7,18,2,19,7,19,2,20,7,20,2,21,
		7,21,2,22,7,22,2,23,7,23,2,24,7,24,2,25,7,25,2,26,7,26,2,27,7,27,2,28,
		7,28,2,29,7,29,2,30,7,30,2,31,7,31,2,32,7,32,2,33,7,33,2,34,7,34,2,35,
		7,35,2,36,7,36,2,37,7,37,2,38,7,38,2,39,7,39,2,40,7,40,2,41,7,41,2,42,
		7,42,2,43,7,43,2,44,7,44,2,45,7,45,2,46,7,46,2,47,7,47,2,48,7,48,2,49,
		7,49,2,50,7,50,2,51,7,51,2,52,7,52,2,53,7,53,2,54,7,54,2,55,7,55,2,56,
		7,56,2,57,7,57,2,58,7,58,2,59,7,59,2,60,7,60,2,61,7,61,2,62,7,62,2,63,
		7,63,2,64,7,64,2,65,7,65,2,66,7,66,2,67,7,67,2,68,7,68,2,69,7,69,2,70,
		7,70,2,71,7,71,2,72,7,72,2,73,7,73,2,74,7,74,2,75,7,75,2,76,7,76,2,77,
		7,77,2,78,7,78,2,79,7,79,2,80,7,80,2,81,7,81,2,82,7,82,2,83,7,83,2,84,
		7,84,2,85,7,85,2,86,7,86,2,87,7,87,2,88,7,88,2,89,7,89,2,90,7,90,2,91,
		7,91,2,92,7,92,2,93,7,93,2,94,7,94,2,95,7,95,2,96,7,96,2,97,7,97,2,98,
		7,98,2,99,7,99,2,100,7,100,2,101,7,101,2,102,7,102,2,103,7,103,2,104,7,
		104,2,105,7,105,2,106,7,106,2,107,7,107,2,108,7,108,2,109,7,109,2,110,
		7,110,2,111,7,111,2,112,7,112,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,3,0,236,
		8,0,1,1,1,1,3,1,240,8,1,1,1,1,1,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,
		2,1,2,1,3,4,3,256,8,3,11,3,12,3,257,1,4,1,4,1,4,1,4,1,4,1,4,1,4,1,4,1,
		4,3,4,269,8,4,1,5,3,5,272,8,5,1,5,1,5,5,5,276,8,5,10,5,12,5,279,9,5,1,
		6,3,6,282,8,6,1,6,1,6,3,6,286,8,6,1,6,3,6,289,8,6,1,7,1,7,1,7,1,7,1,7,
		1,7,1,7,1,7,1,8,1,8,1,8,1,8,1,8,1,8,1,8,1,8,1,8,1,9,1,9,1,9,1,9,1,9,1,
		9,1,9,1,9,1,9,1,9,1,9,3,9,319,8,9,1,10,1,10,1,10,1,10,1,10,1,10,1,11,1,
		11,1,11,1,11,1,11,1,12,1,12,1,12,1,13,1,13,1,13,1,13,1,13,1,13,1,13,1,
		13,1,13,1,13,1,14,1,14,1,14,1,14,1,14,1,14,1,14,1,14,1,15,1,15,1,15,1,
		15,1,16,1,16,1,16,1,17,1,17,1,17,1,17,1,17,1,18,1,18,1,18,1,18,1,18,1,
		19,1,19,1,19,1,19,1,19,1,19,1,20,1,20,1,20,1,21,1,21,1,21,1,21,1,21,1,
		21,1,21,1,21,1,21,1,22,1,22,1,22,1,22,1,22,1,22,1,22,1,23,1,23,1,23,1,
		23,1,23,1,23,1,24,1,24,1,24,1,25,1,25,1,25,1,25,1,25,1,26,1,26,1,26,1,
		26,1,26,1,27,1,27,1,27,1,27,1,27,1,27,1,28,1,28,1,28,1,28,1,28,1,28,1,
		28,1,28,1,28,1,28,1,29,1,29,1,29,1,29,1,29,1,29,1,29,1,29,1,29,1,29,1,
		29,1,29,1,29,1,30,1,30,1,30,1,30,1,30,1,31,1,31,1,31,1,31,1,31,1,31,1,
		31,1,32,1,32,1,32,1,32,1,32,1,32,1,32,1,32,1,32,1,33,1,33,1,33,1,33,1,
		33,1,33,1,33,1,33,1,33,1,33,1,33,1,33,1,34,1,34,1,34,1,34,1,34,1,34,1,
		35,1,35,1,35,1,35,1,35,1,35,1,36,1,36,1,36,1,36,1,36,1,36,1,36,1,36,1,
		37,1,37,1,37,1,37,1,37,1,37,1,37,1,38,1,38,1,38,1,38,1,38,1,38,1,38,1,
		38,1,38,1,39,1,39,1,39,1,39,1,39,1,39,1,39,1,39,1,39,1,39,1,39,1,39,1,
		40,1,40,1,40,1,40,1,40,1,41,1,41,1,41,1,41,1,41,1,41,1,41,1,41,1,42,1,
		42,1,42,1,42,1,42,1,42,1,42,1,42,1,42,1,43,1,43,1,43,1,43,1,43,1,43,1,
		43,1,43,1,43,1,44,1,44,1,44,1,44,1,44,1,44,1,44,1,44,1,44,1,44,1,45,1,
		45,1,45,1,45,1,45,1,45,1,45,1,45,1,45,1,45,1,46,1,46,1,46,1,46,1,46,1,
		47,1,47,1,47,1,47,1,47,1,47,1,47,1,47,1,47,1,47,1,48,1,48,1,48,1,48,1,
		48,1,48,1,48,1,48,1,48,1,48,1,49,1,49,1,49,1,49,1,49,1,50,1,50,1,50,1,
		50,1,50,1,50,1,51,1,51,1,51,1,51,1,51,1,51,1,51,1,52,1,52,1,52,1,52,1,
		52,1,52,1,52,1,53,1,53,1,53,1,53,1,53,1,53,1,54,1,54,1,54,1,54,1,54,1,
		54,1,54,1,54,1,54,1,55,1,55,1,55,1,55,1,55,1,55,1,55,1,55,1,55,1,56,1,
		56,1,56,1,56,1,56,1,56,1,56,1,56,1,57,1,57,1,57,1,57,1,57,1,57,1,57,1,
		58,1,58,1,58,1,58,1,59,1,59,1,59,1,59,1,59,1,59,1,59,1,59,1,59,1,59,1,
		60,1,60,1,60,1,60,1,60,1,60,1,60,1,60,1,60,1,60,1,60,1,60,1,60,1,61,1,
		61,1,61,1,61,1,62,1,62,1,62,1,62,1,62,1,62,1,62,1,63,1,63,1,63,1,63,1,
		63,1,63,1,63,1,64,1,64,1,64,1,64,1,64,1,64,1,64,1,65,1,65,1,65,1,65,1,
		66,1,66,1,66,1,67,1,67,1,67,1,67,1,68,1,68,1,69,1,69,1,70,1,70,1,71,1,
		71,1,72,1,72,1,73,1,73,1,74,1,74,1,75,1,75,1,76,1,76,1,77,1,77,1,78,1,
		78,1,79,1,79,1,80,1,80,1,81,1,81,1,81,1,81,1,82,1,82,1,83,1,83,1,83,1,
		83,1,84,1,84,1,84,1,84,1,85,1,85,1,86,1,86,1,87,1,87,1,88,1,88,1,89,1,
		89,1,89,1,90,1,90,1,90,1,91,1,91,1,91,1,92,1,92,1,92,1,93,1,93,1,93,1,
		94,1,94,5,94,792,8,94,10,94,12,94,795,9,94,1,95,1,95,1,95,3,95,800,8,95,
		1,95,1,95,1,96,1,96,1,97,1,97,1,98,1,98,1,99,4,99,811,8,99,11,99,12,99,
		812,1,100,1,100,1,100,1,100,5,100,819,8,100,10,100,12,100,822,9,100,1,
		101,1,101,3,101,826,8,101,1,101,3,101,829,8,101,1,101,1,101,3,101,833,
		8,101,1,102,3,102,836,8,102,1,103,1,103,3,103,840,8,103,1,104,1,104,1,
		105,4,105,845,8,105,11,105,12,105,846,1,106,1,106,3,106,851,8,106,1,107,
		1,107,1,107,1,107,3,107,857,8,107,1,108,1,108,1,108,1,108,1,108,1,108,
		1,108,1,108,1,108,1,108,1,108,3,108,870,8,108,1,109,1,109,4,109,874,8,
		109,11,109,12,109,875,1,109,1,109,1,109,1,109,1,109,1,110,1,110,1,111,
		1,111,1,112,1,112,0,0,113,1,1,3,2,5,3,7,4,9,5,11,6,13,7,15,8,17,9,19,10,
		21,11,23,12,25,13,27,14,29,15,31,16,33,17,35,18,37,19,39,20,41,21,43,22,
		45,23,47,24,49,25,51,26,53,27,55,28,57,29,59,30,61,31,63,32,65,33,67,34,
		69,35,71,36,73,37,75,38,77,39,79,40,81,41,83,42,85,43,87,44,89,45,91,46,
		93,47,95,48,97,49,99,50,101,51,103,52,105,53,107,54,109,55,111,56,113,
		57,115,58,117,59,119,60,121,61,123,62,125,63,127,64,129,65,131,66,133,
		67,135,68,137,69,139,70,141,71,143,72,145,73,147,74,149,75,151,76,153,
		77,155,78,157,79,159,80,161,81,163,82,165,83,167,84,169,85,171,86,173,
		87,175,88,177,89,179,90,181,91,183,92,185,93,187,94,189,95,191,96,193,
		97,195,0,197,0,199,0,201,0,203,0,205,0,207,0,209,0,211,0,213,0,215,0,217,
		0,219,0,221,0,223,0,225,0,1,0,11,1,0,49,57,1,0,48,57,2,0,9,9,32,32,2,0,
		10,10,12,13,3,0,65,90,95,95,97,122,4,0,10,10,13,13,39,39,92,92,4,0,10,
		10,13,13,34,34,92,92,8,0,34,34,39,39,92,92,98,98,102,102,110,110,114,114,
		116,116,1,0,48,55,3,0,48,57,65,70,97,102,1,0,48,51,897,0,1,1,0,0,0,0,3,
		1,0,0,0,0,5,1,0,0,0,0,7,1,0,0,0,0,9,1,0,0,0,0,11,1,0,0,0,0,13,1,0,0,0,
		0,15,1,0,0,0,0,17,1,0,0,0,0,19,1,0,0,0,0,21,1,0,0,0,0,23,1,0,0,0,0,25,
		1,0,0,0,0,27,1,0,0,0,0,29,1,0,0,0,0,31,1,0,0,0,0,33,1,0,0,0,0,35,1,0,0,
		0,0,37,1,0,0,0,0,39,1,0,0,0,0,41,1,0,0,0,0,43,1,0,0,0,0,45,1,0,0,0,0,47,
		1,0,0,0,0,49,1,0,0,0,0,51,1,0,0,0,0,53,1,0,0,0,0,55,1,0,0,0,0,57,1,0,0,
		0,0,59,1,0,0,0,0,61,1,0,0,0,0,63,1,0,0,0,0,65,1,0,0,0,0,67,1,0,0,0,0,69,
		1,0,0,0,0,71,1,0,0,0,0,73,1,0,0,0,0,75,1,0,0,0,0,77,1,0,0,0,0,79,1,0,0,
		0,0,81,1,0,0,0,0,83,1,0,0,0,0,85,1,0,0,0,0,87,1,0,0,0,0,89,1,0,0,0,0,91,
		1,0,0,0,0,93,1,0,0,0,0,95,1,0,0,0,0,97,1,0,0,0,0,99,1,0,0,0,0,101,1,0,
		0,0,0,103,1,0,0,0,0,105,1,0,0,0,0,107,1,0,0,0,0,109,1,0,0,0,0,111,1,0,
		0,0,0,113,1,0,0,0,0,115,1,0,0,0,0,117,1,0,0,0,0,119,1,0,0,0,0,121,1,0,
		0,0,0,123,1,0,0,0,0,125,1,0,0,0,0,127,1,0,0,0,0,129,1,0,0,0,0,131,1,0,
		0,0,0,133,1,0,0,0,0,135,1,0,0,0,0,137,1,0,0,0,0,139,1,0,0,0,0,141,1,0,
		0,0,0,143,1,0,0,0,0,145,1,0,0,0,0,147,1,0,0,0,0,149,1,0,0,0,0,151,1,0,
		0,0,0,153,1,0,0,0,0,155,1,0,0,0,0,157,1,0,0,0,0,159,1,0,0,0,0,161,1,0,
		0,0,0,163,1,0,0,0,0,165,1,0,0,0,0,167,1,0,0,0,0,169,1,0,0,0,0,171,1,0,
		0,0,0,173,1,0,0,0,0,175,1,0,0,0,0,177,1,0,0,0,0,179,1,0,0,0,0,181,1,0,
		0,0,0,183,1,0,0,0,0,185,1,0,0,0,0,187,1,0,0,0,0,189,1,0,0,0,0,191,1,0,
		0,0,0,193,1,0,0,0,1,235,1,0,0,0,3,237,1,0,0,0,5,243,1,0,0,0,7,255,1,0,
		0,0,9,268,1,0,0,0,11,271,1,0,0,0,13,285,1,0,0,0,15,290,1,0,0,0,17,298,
		1,0,0,0,19,318,1,0,0,0,21,320,1,0,0,0,23,326,1,0,0,0,25,331,1,0,0,0,27,
		334,1,0,0,0,29,344,1,0,0,0,31,352,1,0,0,0,33,356,1,0,0,0,35,359,1,0,0,
		0,37,364,1,0,0,0,39,369,1,0,0,0,41,375,1,0,0,0,43,378,1,0,0,0,45,387,1,
		0,0,0,47,394,1,0,0,0,49,400,1,0,0,0,51,403,1,0,0,0,53,408,1,0,0,0,55,413,
		1,0,0,0,57,419,1,0,0,0,59,429,1,0,0,0,61,442,1,0,0,0,63,447,1,0,0,0,65,
		454,1,0,0,0,67,463,1,0,0,0,69,475,1,0,0,0,71,481,1,0,0,0,73,487,1,0,0,
		0,75,495,1,0,0,0,77,502,1,0,0,0,79,511,1,0,0,0,81,523,1,0,0,0,83,528,1,
		0,0,0,85,536,1,0,0,0,87,545,1,0,0,0,89,554,1,0,0,0,91,564,1,0,0,0,93,574,
		1,0,0,0,95,579,1,0,0,0,97,589,1,0,0,0,99,599,1,0,0,0,101,604,1,0,0,0,103,
		610,1,0,0,0,105,617,1,0,0,0,107,624,1,0,0,0,109,630,1,0,0,0,111,639,1,
		0,0,0,113,648,1,0,0,0,115,656,1,0,0,0,117,663,1,0,0,0,119,667,1,0,0,0,
		121,677,1,0,0,0,123,690,1,0,0,0,125,694,1,0,0,0,127,701,1,0,0,0,129,708,
		1,0,0,0,131,715,1,0,0,0,133,719,1,0,0,0,135,722,1,0,0,0,137,726,1,0,0,
		0,139,728,1,0,0,0,141,730,1,0,0,0,143,732,1,0,0,0,145,734,1,0,0,0,147,
		736,1,0,0,0,149,738,1,0,0,0,151,740,1,0,0,0,153,742,1,0,0,0,155,744,1,
		0,0,0,157,746,1,0,0,0,159,748,1,0,0,0,161,750,1,0,0,0,163,752,1,0,0,0,
		165,756,1,0,0,0,167,758,1,0,0,0,169,762,1,0,0,0,171,766,1,0,0,0,173,768,
		1,0,0,0,175,770,1,0,0,0,177,772,1,0,0,0,179,774,1,0,0,0,181,777,1,0,0,
		0,183,780,1,0,0,0,185,783,1,0,0,0,187,786,1,0,0,0,189,789,1,0,0,0,191,
		799,1,0,0,0,193,803,1,0,0,0,195,805,1,0,0,0,197,807,1,0,0,0,199,810,1,
		0,0,0,201,814,1,0,0,0,203,823,1,0,0,0,205,835,1,0,0,0,207,839,1,0,0,0,
		209,841,1,0,0,0,211,844,1,0,0,0,213,850,1,0,0,0,215,856,1,0,0,0,217,869,
		1,0,0,0,219,871,1,0,0,0,221,882,1,0,0,0,223,884,1,0,0,0,225,886,1,0,0,
		0,227,228,5,39,0,0,228,229,3,209,104,0,229,230,5,39,0,0,230,236,1,0,0,
		0,231,232,5,39,0,0,232,233,3,215,107,0,233,234,5,39,0,0,234,236,1,0,0,
		0,235,227,1,0,0,0,235,231,1,0,0,0,236,2,1,0,0,0,237,239,5,34,0,0,238,240,
		3,211,105,0,239,238,1,0,0,0,239,240,1,0,0,0,240,241,1,0,0,0,241,242,5,
		34,0,0,242,4,1,0,0,0,243,244,3,197,98,0,244,245,3,197,98,0,245,246,5,47,
		0,0,246,247,3,197,98,0,247,248,3,197,98,0,248,249,5,47,0,0,249,250,3,197,
		98,0,250,251,3,197,98,0,251,252,3,197,98,0,252,253,3,197,98,0,253,6,1,
		0,0,0,254,256,3,197,98,0,255,254,1,0,0,0,256,257,1,0,0,0,257,255,1,0,0,
		0,257,258,1,0,0,0,258,8,1,0,0,0,259,260,5,84,0,0,260,261,5,82,0,0,261,
		262,5,85,0,0,262,269,5,69,0,0,263,264,5,70,0,0,264,265,5,65,0,0,265,266,
		5,76,0,0,266,267,5,83,0,0,267,269,5,69,0,0,268,259,1,0,0,0,268,263,1,0,
		0,0,269,10,1,0,0,0,270,272,5,13,0,0,271,270,1,0,0,0,271,272,1,0,0,0,272,
		273,1,0,0,0,273,277,5,10,0,0,274,276,5,32,0,0,275,274,1,0,0,0,276,279,
		1,0,0,0,277,275,1,0,0,0,277,278,1,0,0,0,278,12,1,0,0,0,279,277,1,0,0,0,
		280,282,5,13,0,0,281,280,1,0,0,0,281,282,1,0,0,0,282,283,1,0,0,0,283,286,
		5,10,0,0,284,286,2,12,13,0,285,281,1,0,0,0,285,284,1,0,0,0,286,288,1,0,
		0,0,287,289,3,199,99,0,288,287,1,0,0,0,288,289,1,0,0,0,289,14,1,0,0,0,
		290,291,5,68,0,0,291,292,5,69,0,0,292,293,5,67,0,0,293,294,5,76,0,0,294,
		295,5,65,0,0,295,296,5,82,0,0,296,297,5,69,0,0,297,16,1,0,0,0,298,299,
		5,67,0,0,299,300,5,79,0,0,300,301,5,78,0,0,301,302,5,83,0,0,302,303,5,
		84,0,0,303,304,5,65,0,0,304,305,5,78,0,0,305,306,5,84,0,0,306,18,1,0,0,
		0,307,308,5,79,0,0,308,309,5,85,0,0,309,310,5,84,0,0,310,311,5,80,0,0,
		311,312,5,85,0,0,312,319,5,84,0,0,313,314,5,73,0,0,314,315,5,78,0,0,315,
		316,5,80,0,0,316,317,5,85,0,0,317,319,5,84,0,0,318,307,1,0,0,0,318,313,
		1,0,0,0,319,20,1,0,0,0,320,321,5,65,0,0,321,322,5,82,0,0,322,323,5,82,
		0,0,323,324,5,65,0,0,324,325,5,89,0,0,325,22,1,0,0,0,326,327,5,67,0,0,
		327,328,5,65,0,0,328,329,5,83,0,0,329,330,5,69,0,0,330,24,1,0,0,0,331,
		332,5,79,0,0,332,333,5,70,0,0,333,26,1,0,0,0,334,335,5,79,0,0,335,336,
		5,84,0,0,336,337,5,72,0,0,337,338,5,69,0,0,338,339,5,82,0,0,339,340,5,
		87,0,0,340,341,5,73,0,0,341,342,5,83,0,0,342,343,5,69,0,0,343,28,1,0,0,
		0,344,345,5,69,0,0,345,346,5,78,0,0,346,347,5,68,0,0,347,348,5,67,0,0,
		348,349,5,65,0,0,349,350,5,83,0,0,350,351,5,69,0,0,351,30,1,0,0,0,352,
		353,5,70,0,0,353,354,5,79,0,0,354,355,5,82,0,0,355,32,1,0,0,0,356,357,
		5,84,0,0,357,358,5,79,0,0,358,34,1,0,0,0,359,360,5,83,0,0,360,361,5,84,
		0,0,361,362,5,69,0,0,362,363,5,80,0,0,363,36,1,0,0,0,364,365,5,78,0,0,
		365,366,5,69,0,0,366,367,5,88,0,0,367,368,5,84,0,0,368,38,1,0,0,0,369,
		370,5,87,0,0,370,371,5,72,0,0,371,372,5,73,0,0,372,373,5,76,0,0,373,374,
		5,69,0,0,374,40,1,0,0,0,375,376,5,68,0,0,376,377,5,79,0,0,377,42,1,0,0,
		0,378,379,5,69,0,0,379,380,5,78,0,0,380,381,5,68,0,0,381,382,5,87,0,0,
		382,383,5,72,0,0,383,384,5,73,0,0,384,385,5,76,0,0,385,386,5,69,0,0,386,
		44,1,0,0,0,387,388,5,82,0,0,388,389,5,69,0,0,389,390,5,80,0,0,390,391,
		5,69,0,0,391,392,5,65,0,0,392,393,5,84,0,0,393,46,1,0,0,0,394,395,5,85,
		0,0,395,396,5,78,0,0,396,397,5,84,0,0,397,398,5,73,0,0,398,399,5,76,0,
		0,399,48,1,0,0,0,400,401,5,73,0,0,401,402,5,70,0,0,402,50,1,0,0,0,403,
		404,5,84,0,0,404,405,5,72,0,0,405,406,5,69,0,0,406,407,5,78,0,0,407,52,
		1,0,0,0,408,409,5,69,0,0,409,410,5,76,0,0,410,411,5,83,0,0,411,412,5,69,
		0,0,412,54,1,0,0,0,413,414,5,69,0,0,414,415,5,78,0,0,415,416,5,68,0,0,
		416,417,5,73,0,0,417,418,5,70,0,0,418,56,1,0,0,0,419,420,5,80,0,0,420,
		421,5,82,0,0,421,422,5,79,0,0,422,423,5,67,0,0,423,424,5,69,0,0,424,425,
		5,68,0,0,425,426,5,85,0,0,426,427,5,82,0,0,427,428,5,69,0,0,428,58,1,0,
		0,0,429,430,5,69,0,0,430,431,5,78,0,0,431,432,5,68,0,0,432,433,5,80,0,
		0,433,434,5,82,0,0,434,435,5,79,0,0,435,436,5,67,0,0,436,437,5,69,0,0,
		437,438,5,68,0,0,438,439,5,85,0,0,439,440,5,82,0,0,440,441,5,69,0,0,441,
		60,1,0,0,0,442,443,5,67,0,0,443,444,5,65,0,0,444,445,5,76,0,0,445,446,
		5,76,0,0,446,62,1,0,0,0,447,448,5,69,0,0,448,449,5,88,0,0,449,450,5,84,
		0,0,450,451,5,69,0,0,451,452,5,82,0,0,452,453,5,78,0,0,453,64,1,0,0,0,
		454,455,5,70,0,0,455,456,5,85,0,0,456,457,5,78,0,0,457,458,5,67,0,0,458,
		459,5,84,0,0,459,460,5,73,0,0,460,461,5,79,0,0,461,462,5,78,0,0,462,66,
		1,0,0,0,463,464,5,69,0,0,464,465,5,78,0,0,465,466,5,68,0,0,466,467,5,70,
		0,0,467,468,5,85,0,0,468,469,5,78,0,0,469,470,5,67,0,0,470,471,5,84,0,
		0,471,472,5,73,0,0,472,473,5,79,0,0,473,474,5,78,0,0,474,68,1,0,0,0,475,
		476,5,66,0,0,476,477,5,89,0,0,477,478,5,86,0,0,478,479,5,65,0,0,479,480,
		5,76,0,0,480,70,1,0,0,0,481,482,5,66,0,0,482,483,5,89,0,0,483,484,5,82,
		0,0,484,485,5,69,0,0,485,486,5,70,0,0,486,72,1,0,0,0,487,488,5,82,0,0,
		488,489,5,69,0,0,489,490,5,84,0,0,490,491,5,85,0,0,491,492,5,82,0,0,492,
		493,5,78,0,0,493,494,5,83,0,0,494,74,1,0,0,0,495,496,5,82,0,0,496,497,
		5,69,0,0,497,498,5,84,0,0,498,499,5,85,0,0,499,500,5,82,0,0,500,501,5,
		78,0,0,501,76,1,0,0,0,502,503,5,79,0,0,503,504,5,80,0,0,504,505,5,69,0,
		0,505,506,5,82,0,0,506,507,5,65,0,0,507,508,5,84,0,0,508,509,5,79,0,0,
		509,510,5,82,0,0,510,78,1,0,0,0,511,512,5,69,0,0,512,513,5,78,0,0,513,
		514,5,68,0,0,514,515,5,79,0,0,515,516,5,80,0,0,516,517,5,69,0,0,517,518,
		5,82,0,0,518,519,5,65,0,0,519,520,5,84,0,0,520,521,5,79,0,0,521,522,5,
		82,0,0,522,80,1,0,0,0,523,524,5,84,0,0,524,525,5,89,0,0,525,526,5,80,0,
		0,526,527,5,69,0,0,527,82,1,0,0,0,528,529,5,69,0,0,529,530,5,78,0,0,530,
		531,5,68,0,0,531,532,5,84,0,0,532,533,5,89,0,0,533,534,5,80,0,0,534,535,
		5,69,0,0,535,84,1,0,0,0,536,537,5,79,0,0,537,538,5,80,0,0,538,539,5,69,
		0,0,539,540,5,78,0,0,540,541,5,70,0,0,541,542,5,73,0,0,542,543,5,76,0,
		0,543,544,5,69,0,0,544,86,1,0,0,0,545,546,5,82,0,0,546,547,5,69,0,0,547,
		548,5,65,0,0,548,549,5,68,0,0,549,550,5,70,0,0,550,551,5,73,0,0,551,552,
		5,76,0,0,552,553,5,69,0,0,553,88,1,0,0,0,554,555,5,87,0,0,555,556,5,82,
		0,0,556,557,5,73,0,0,557,558,5,84,0,0,558,559,5,69,0,0,559,560,5,70,0,
		0,560,561,5,73,0,0,561,562,5,76,0,0,562,563,5,69,0,0,563,90,1,0,0,0,564,
		565,5,67,0,0,565,566,5,76,0,0,566,567,5,79,0,0,567,568,5,83,0,0,568,569,
		5,69,0,0,569,570,5,70,0,0,570,571,5,73,0,0,571,572,5,76,0,0,572,573,5,
		69,0,0,573,92,1,0,0,0,574,575,5,83,0,0,575,576,5,69,0,0,576,577,5,69,0,
		0,577,578,5,75,0,0,578,94,1,0,0,0,579,580,5,71,0,0,580,581,5,69,0,0,581,
		582,5,84,0,0,582,583,5,82,0,0,583,584,5,69,0,0,584,585,5,67,0,0,585,586,
		5,79,0,0,586,587,5,82,0,0,587,588,5,68,0,0,588,96,1,0,0,0,589,590,5,80,
		0,0,590,591,5,85,0,0,591,592,5,84,0,0,592,593,5,82,0,0,593,594,5,69,0,
		0,594,595,5,67,0,0,595,596,5,79,0,0,596,597,5,82,0,0,597,598,5,68,0,0,
		598,98,1,0,0,0,599,600,5,82,0,0,600,601,5,69,0,0,601,602,5,65,0,0,602,
		603,5,68,0,0,603,100,1,0,0,0,604,605,5,87,0,0,605,606,5,82,0,0,606,607,
		5,73,0,0,607,608,5,84,0,0,608,609,5,69,0,0,609,102,1,0,0,0,610,611,5,65,
		0,0,611,612,5,80,0,0,612,613,5,80,0,0,613,614,5,69,0,0,614,615,5,78,0,
		0,615,616,5,68,0,0,616,104,1,0,0,0,617,618,5,82,0,0,618,619,5,65,0,0,619,
		620,5,78,0,0,620,621,5,68,0,0,621,622,5,79,0,0,622,623,5,77,0,0,623,106,
		1,0,0,0,624,625,5,67,0,0,625,626,5,76,0,0,626,627,5,65,0,0,627,628,5,83,
		0,0,628,629,5,83,0,0,629,108,1,0,0,0,630,631,5,69,0,0,631,632,5,78,0,0,
		632,633,5,68,0,0,633,634,5,67,0,0,634,635,5,76,0,0,635,636,5,65,0,0,636,
		637,5,83,0,0,637,638,5,83,0,0,638,110,1,0,0,0,639,640,5,73,0,0,640,641,
		5,78,0,0,641,642,5,72,0,0,642,643,5,69,0,0,643,644,5,82,0,0,644,645,5,
		73,0,0,645,646,5,84,0,0,646,647,5,83,0,0,647,112,1,0,0,0,648,649,5,80,
		0,0,649,650,5,82,0,0,650,651,5,73,0,0,651,652,5,86,0,0,652,653,5,65,0,
		0,653,654,5,84,0,0,654,655,5,69,0,0,655,114,1,0,0,0,656,657,5,80,0,0,657,
		658,5,85,0,0,658,659,5,66,0,0,659,660,5,76,0,0,660,661,5,73,0,0,661,662,
		5,67,0,0,662,116,1,0,0,0,663,664,5,78,0,0,664,665,5,69,0,0,665,666,5,87,
		0,0,666,118,1,0,0,0,667,668,5,78,0,0,668,669,5,65,0,0,669,670,5,77,0,0,
		670,671,5,69,0,0,671,672,5,83,0,0,672,673,5,80,0,0,673,674,5,65,0,0,674,
		675,5,67,0,0,675,676,5,69,0,0,676,120,1,0,0,0,677,678,5,69,0,0,678,679,
		5,78,0,0,679,680,5,68,0,0,680,681,5,78,0,0,681,682,5,65,0,0,682,683,5,
		77,0,0,683,684,5,69,0,0,684,685,5,83,0,0,685,686,5,80,0,0,686,687,5,65,
		0,0,687,688,5,67,0,0,688,689,5,69,0,0,689,122,1,0,0,0,690,691,5,85,0,0,
		691,692,5,83,0,0,692,693,5,69,0,0,693,124,1,0,0,0,694,695,5,77,0,0,695,
		696,5,65,0,0,696,697,5,76,0,0,697,698,5,76,0,0,698,699,5,79,0,0,699,700,
		5,67,0,0,700,126,1,0,0,0,701,702,5,83,0,0,702,703,5,73,0,0,703,704,5,90,
		0,0,704,705,5,69,0,0,705,706,5,79,0,0,706,707,5,70,0,0,707,128,1,0,0,0,
		708,709,5,73,0,0,709,710,5,77,0,0,710,711,5,80,0,0,711,712,5,79,0,0,712,
		713,5,82,0,0,713,714,5,84,0,0,714,130,1,0,0,0,715,716,5,65,0,0,716,717,
		5,78,0,0,717,718,5,68,0,0,718,132,1,0,0,0,719,720,5,79,0,0,720,721,5,82,
		0,0,721,134,1,0,0,0,722,723,5,78,0,0,723,724,5,79,0,0,724,725,5,84,0,0,
		725,136,1,0,0,0,726,727,5,40,0,0,727,138,1,0,0,0,728,729,5,41,0,0,729,
		140,1,0,0,0,730,731,5,91,0,0,731,142,1,0,0,0,732,733,5,93,0,0,733,144,
		1,0,0,0,734,735,5,123,0,0,735,146,1,0,0,0,736,737,5,125,0,0,737,148,1,
		0,0,0,738,739,5,58,0,0,739,150,1,0,0,0,740,741,5,44,0,0,741,152,1,0,0,
		0,742,743,5,46,0,0,743,154,1,0,0,0,744,745,5,43,0,0,745,156,1,0,0,0,746,
		747,5,45,0,0,747,158,1,0,0,0,748,749,5,42,0,0,749,160,1,0,0,0,750,751,
		5,38,0,0,751,162,1,0,0,0,752,753,5,68,0,0,753,754,5,73,0,0,754,755,5,86,
		0,0,755,164,1,0,0,0,756,757,5,47,0,0,757,166,1,0,0,0,758,759,5,77,0,0,
		759,760,5,79,0,0,760,761,5,68,0,0,761,168,1,0,0,0,762,763,5,80,0,0,763,
		764,5,79,0,0,764,765,5,87,0,0,765,170,1,0,0,0,766,767,5,94,0,0,767,172,
		1,0,0,0,768,769,5,61,0,0,769,174,1,0,0,0,770,771,5,62,0,0,771,176,1,0,
		0,0,772,773,5,60,0,0,773,178,1,0,0,0,774,775,5,62,0,0,775,776,5,61,0,0,
		776,180,1,0,0,0,777,778,5,60,0,0,778,779,5,61,0,0,779,182,1,0,0,0,780,
		781,5,60,0,0,781,782,5,62,0,0,782,184,1,0,0,0,783,784,5,60,0,0,784,785,
		5,45,0,0,785,186,1,0,0,0,786,787,5,58,0,0,787,788,5,58,0,0,788,188,1,0,
		0,0,789,793,3,205,102,0,790,792,3,207,103,0,791,790,1,0,0,0,792,795,1,
		0,0,0,793,791,1,0,0,0,793,794,1,0,0,0,794,190,1,0,0,0,795,793,1,0,0,0,
		796,800,3,199,99,0,797,800,3,201,100,0,798,800,3,203,101,0,799,796,1,0,
		0,0,799,797,1,0,0,0,799,798,1,0,0,0,800,801,1,0,0,0,801,802,6,95,0,0,802,
		192,1,0,0,0,803,804,9,0,0,0,804,194,1,0,0,0,805,806,7,0,0,0,806,196,1,
		0,0,0,807,808,7,1,0,0,808,198,1,0,0,0,809,811,7,2,0,0,810,809,1,0,0,0,
		811,812,1,0,0,0,812,810,1,0,0,0,812,813,1,0,0,0,813,200,1,0,0,0,814,815,
		5,47,0,0,815,816,5,47,0,0,816,820,1,0,0,0,817,819,8,3,0,0,818,817,1,0,
		0,0,819,822,1,0,0,0,820,818,1,0,0,0,820,821,1,0,0,0,821,202,1,0,0,0,822,
		820,1,0,0,0,823,825,5,92,0,0,824,826,3,199,99,0,825,824,1,0,0,0,825,826,
		1,0,0,0,826,832,1,0,0,0,827,829,5,13,0,0,828,827,1,0,0,0,828,829,1,0,0,
		0,829,830,1,0,0,0,830,833,5,10,0,0,831,833,2,12,13,0,832,828,1,0,0,0,832,
		831,1,0,0,0,833,204,1,0,0,0,834,836,7,4,0,0,835,834,1,0,0,0,836,206,1,
		0,0,0,837,840,3,205,102,0,838,840,7,1,0,0,839,837,1,0,0,0,839,838,1,0,
		0,0,840,208,1,0,0,0,841,842,8,5,0,0,842,210,1,0,0,0,843,845,3,213,106,
		0,844,843,1,0,0,0,845,846,1,0,0,0,846,844,1,0,0,0,846,847,1,0,0,0,847,
		212,1,0,0,0,848,851,8,6,0,0,849,851,3,215,107,0,850,848,1,0,0,0,850,849,
		1,0,0,0,851,214,1,0,0,0,852,853,5,92,0,0,853,857,7,7,0,0,854,857,3,217,
		108,0,855,857,3,219,109,0,856,852,1,0,0,0,856,854,1,0,0,0,856,855,1,0,
		0,0,857,216,1,0,0,0,858,859,5,92,0,0,859,870,3,221,110,0,860,861,5,92,
		0,0,861,862,3,221,110,0,862,863,3,221,110,0,863,870,1,0,0,0,864,865,5,
		92,0,0,865,866,3,225,112,0,866,867,3,221,110,0,867,868,3,221,110,0,868,
		870,1,0,0,0,869,858,1,0,0,0,869,860,1,0,0,0,869,864,1,0,0,0,870,218,1,
		0,0,0,871,873,5,92,0,0,872,874,5,117,0,0,873,872,1,0,0,0,874,875,1,0,0,
		0,875,873,1,0,0,0,875,876,1,0,0,0,876,877,1,0,0,0,877,878,3,223,111,0,
		878,879,3,223,111,0,879,880,3,223,111,0,880,881,3,223,111,0,881,220,1,
		0,0,0,882,883,7,8,0,0,883,222,1,0,0,0,884,885,7,9,0,0,885,224,1,0,0,0,
		886,887,7,10,0,0,887,226,1,0,0,0,25,0,235,239,257,268,271,277,281,285,
		288,318,793,799,812,820,825,828,832,835,839,846,850,856,869,875,1,6,0,
		0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
