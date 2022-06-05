/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2014 Bart Kiers
 * Copyright (c) 2019 Robert Einhorn
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * Project      : Python3-parser; an ANTLR4 grammar for Python 3
 *                https://github.com/bkiers/Python3-parser
 * Developed by : Bart Kiers, bart@big-o.nl
 *
 * Project      : an ANTLR4 grammar for Tiny Python without embedded actions
 *                https://github.com/antlr/grammars-v4/tree/master/python/tiny-python/tiny-grammar-without-actions
 * Developed by : Robert Einhorn, robert.einhorn.hu@gmail.com
 */

// Based on the Bart Kiers's ANTLR4 Python 3.3 grammar: https://github.com/bkiers/Python3-parser
// and the Python 3.3.7 Language Reference:             https://docs.python.org/3.3/reference/grammar.html

grammar PseudoCode; // tiny version

tokens { INDENT, DEDENT }
@lexer::header {
using AntlrDenter;
}
@lexer::members {
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
}
@parser::header {
using PseudoCode.Core.Runtime;
using Range = PseudoCode.Core.Runtime.Range;
using System.Globalization;
}


/*
 * parser rules
 */

// startRules:
fileInput:  statement* EOF;
singleInput: statement NL;

statement: (simpleStatement | compoundStatement) | NL;

simpleStatement: smallStatement;
smallStatement
 : assignmentStatement
 | declarationStatement
 | constantStatement
 | ioStatement
 | fileStatement
 | returnStatement
 | callStatement
 ;
assignmentStatement: expression AssignmentNotation expression;
declarationStatement: Declare Identifier Colon dataType;
constantStatement: Constant Identifier Equal expression;
ioStatement: IoKeyword tuple;
returnStatement: Return expression;
callStatement: Call expression;
fileStatement
 : OpenFile expression For fileMode=(Read | Write | Append | Random)
 | ReadFile expression Comma expression
 | WriteFile expression Comma expression
 | CloseFile expression
 | Seek expression Comma expression
 | GetRecord expression Comma expression
 | PutRecord expression Comma expression
 ;

compoundStatement
 : ifStatement
 | caseStatement
 | whileStatement
 | forStatement
 | repeatStatement
 | procedureDefinition
 | functionDefinition
 | typeDefinition
 | enumDefinition
 | pointerDefinition
 | classDefinition
 ;
// This expression will be scoped.
scopedExpression: expression;
indentedBlock: INDENT statement+ DEDENT;
alignedBlock: statement (INDENT statement+ DEDENT)?;
ifStatement locals [bool HasElse]: If scopedExpression Then indentedBlock (Else indentedBlock {$HasElse = true;})? Endif;
forStatement locals [bool HasStep]: For expression AssignmentNotation expression To scopedExpression (Step scopedExpression {$HasStep = true;})? indentedBlock Next scopedExpression;
whileStatement: While scopedExpression Do indentedBlock Endwhile;
repeatStatement: Repeat indentedBlock Until scopedExpression;

caseStatement: Case arithmeticExpression Of caseBody Endcase;
caseBranch
 : (Otherwise | atom | valueRange) Colon alignedBlock
 | NL
 ;
caseBody: INDENT caseBranch+ DEDENT;

valueRange: expression To expression;

procedureDefinition: Procedure identifierWithNew argumentsDeclaration? indentedBlock Endprocedure;
functionDefinition: Function Identifier argumentsDeclaration? Returns dataType indentedBlock Endfunction;
argumentsDeclaration: OpenParen (argumentDeclaration (Comma argumentDeclaration)*)? CloseParen;
argumentDeclaration: (Byval | Byref)? Identifier Colon dataType;
tuple: expression (Comma expression)*;

enumDefinition: Type Identifier Equal OpenParen Identifier (Comma Identifier)* CloseParen;
pointerDefinition: Type Identifier Equal Caret dataType;
typeDefinition: Type Identifier typeBody Endtype;
typeBody: INDENT typeChild+ DEDENT;
typeChild
 : declarationStatement
 | NL
 ;
 
classDefinition: Class className=Identifier (Inherits inheritClass=Identifier)? classBody Endclass;
classBody: INDENT (classDataMember | classMethod | assignmentStatement | NL)+ DEDENT;
classDataMember: accessLevel? Identifier Colon dataType;
classMethod: accessLevel? (procedureDefinition | functionDefinition);
accessLevel: Public | Private;

expression
 : logicExpression
 ;

logicExpression locals [bool IsUnary]
 : logicExpression comp=comparisonOp logicExpression
 | op=Not logicExpression {$IsUnary = true;}
 | logicExpression op=And logicExpression
 | logicExpression op=Or logicExpression
 | arithmeticExpression {$IsUnary = true;}
 | '(' logicExpression ')' {$IsUnary = true;}
 ;
comparisonOp: Smaller | Greater | Equal | GreaterEqual | SmallerEqual | NotEqual;

//expr: xorExpression ('|' xorExpression)*;
//xorExpression: andExpression ('^' andExpression)*;
//andExpression: shiftExpression ('&' shiftExpression)*;
//shiftExpression: arithExpression (('<<'|'>>') arithExpression)*;
//arithExpression: term (('+'|'-') term)*;
//term: factor (('*'|'@'|'/'|'%'|'//') factor)*;
//factor: ('+'|'-'|'~') factor | power;
//power
// : rvalue ('**' factor)?
// ;
arithmeticExpression locals [bool IsUnary]
 : New Identifier arguments
 | Identifier {$IsUnary = true;}
 | atom {$IsUnary = true;}
 | arithmeticExpression Dot Identifier
 | arithmeticExpression array
 | arithmeticExpression arguments
 | op=Caret operand=arithmeticExpression {$IsUnary = true;}
 | operand=arithmeticExpression op=Caret {$IsUnary = true;}
 | <assoc=right> operand1=arithmeticExpression op=Pow operand2=arithmeticExpression
 | op=Subtract operand=arithmeticExpression {$IsUnary = true;}
 | operand1=arithmeticExpression op=Divide operand2=arithmeticExpression
 | operand1=arithmeticExpression op=IntDivide operand2=arithmeticExpression
 | operand1=arithmeticExpression op=Multiply operand2=arithmeticExpression
 | operand1=arithmeticExpression op=Mod operand2=arithmeticExpression
 | operand1=arithmeticExpression op=Subtract operand2=arithmeticExpression
 | operand1=arithmeticExpression op=Add operand2=arithmeticExpression
 | operand1=arithmeticExpression op=BitAnd operand2=arithmeticExpression
 | '(' arithmeticExpression ')' {$IsUnary = true;}
 ;

 
//// Values that can be altered
//lvalue
// : Identifier
// | lvalue array
// | lvalue Dot Identifier
// ;

// an rvalue cannot be assigned a value
//rvalue
// : lvalue
// | atom
// | rvalue DOT IDENTIFIER
// | rvalue array
// | rvalue arguments
// ;

arguments: OpenParen tuple? CloseParen;

atom locals [string AtomType, object Value]
 : number
 | String {
    $AtomType = "STRING";
    var str = System.Text.RegularExpressions.Regex.Unescape($String.text);
    $Value = str.Substring(1, str.Length - 2);
 }
 | Character {
    $AtomType = "CHAR";
    var str = System.Text.RegularExpressions.Regex.Unescape($Character.text);
    $Value = str[1];
 }
 | Boolean {$AtomType = "BOOLEAN"; $Value = bool.Parse($Boolean.text);}
 | Date {$AtomType = "DATE";}
 | array {$AtomType = "ARRAY";}
 ;

dataType locals [string TypeName, bool IsArray, List<Range> Dimensions = new()]
 : Array OpenBrack arrayRange (Comma arrayRange)* CloseBrack Of basicDataType {
    $IsArray = true;
    $TypeName = $basicDataType.text;
 }
 | basicDataType {$TypeName = $basicDataType.text;}
 ;
basicDataType : Typename | Identifier;
 
arrayRange
 : s=expression Colon e=expression
 ;

array
 : OpenBrack expression (Comma expression)* CloseBrack
 ;

//array_index
// : OPEN_BRACK INTEGER (COMMA INTEGER)* CLOSE_BRACK
// ;


/*
 * lexer rules
 */

number
 : Integer {$atom::AtomType = "INTEGER"; $atom::Value = int.Parse($Integer.text);}
 | Decimal {$atom::AtomType = "REAL"; $atom::Value = decimal.Parse($Decimal.text);}
 ;
 
 identifierWithNew: Identifier | New;

 
Character
	:	'\'' SingleCharacter '\''
	|	'\'' EscapeSequence '\''
	;
// §3.10.5 String Literals
String
	:	'"' StringCharacters? '"'
	;


Date : Digit Digit '/' Digit Digit '/' Digit Digit Digit Digit;
// Value is either 1. or .1 and not just a single '.' to avoid ambiguity with DOT
Decimal
 : '-'? (Digit* '.' Digit+ | Digit+ '.' Digit*)
 ;
Integer : '-' ? Digit+;

Boolean : 'TRUE' | 'FALSE';

 


NL: ('\r'? '\n' ' '*); // For tabs just switch out ' '* with '\t'*
Newline
 : ( '\r'? '\n' | '\r' | '\f' ) Spaces?
 ;

Declare : 'DECLARE';
Constant : 'CONSTANT';
IoKeyword : 'OUTPUT' | 'INPUT';
Typename : 'INTEGER' | 'STRING' | 'REAL' | 'CHAR' | 'BOOLEAN' | 'DATE';
Array : 'ARRAY';
Case : 'CASE';
Of : 'OF';
Otherwise : 'OTHERWISE';
Endcase : 'ENDCASE';

For : 'FOR';
To : 'TO';
Step : 'STEP';
Next : 'NEXT';

While : 'WHILE';
Do : 'DO';
Endwhile : 'ENDWHILE';
Repeat : 'REPEAT';
Until : 'UNTIL';

If : 'IF';
Then : 'THEN';
Else : 'ELSE';
Endif : 'ENDIF';

Procedure : 'PROCEDURE';
Endprocedure : 'ENDPROCEDURE';

Call : 'CALL';
Function : 'FUNCTION';
Endfunction : 'ENDFUNCTION';
Byval : 'BYVAL';
Byref : 'BYREF';
Returns : 'RETURNS';
Return : 'RETURN';

Type : 'TYPE';
Endtype : 'ENDTYPE';

OpenFile : 'OPENFILE';
ReadFile : 'READFILE';
WriteFile : 'WRITEFILE';
CloseFile : 'CLOSEFILE';
Seek : 'SEEK';
GetRecord : 'GETRECORD';
PutRecord : 'PUTRECORD';
Read : 'READ';
Write : 'WRITE';
Append : 'APPEND';
Random : 'RANDOM';

Class : 'CLASS';
Endclass : 'ENDCLASS';
Inherits : 'INHERITS';
Private : 'PRIVATE';
Public : 'PUBLIC';
New : 'NEW';

// Logic
And : 'AND';
Or : 'OR';
Not : 'NOT';





OpenParen : '(';
CloseParen : ')';
OpenBrack : '[';
CloseBrack : ']';
OpenBrace : '{';
CloseBrace : '}';
Colon : ':';
Comma : ',';
Dot : '.';
Add : '+';
Subtract : '-';
Multiply : '*';
BitAnd : '&';
IntDivide : 'DIV';
Divide : '/';
Mod : 'MOD';
Pow : 'POW';
Caret : '^';
Equal : '=';
Greater : '>';
Smaller : '<';
GreaterEqual : '>=';
SmallerEqual : '<=';
NotEqual : '<>';

AssignmentNotation : '<-';

Identifier
 : IdStart IdContinue*
 ;
Skip
 : ( Spaces | Comment | LineJoining ) -> skip
 ;

UnknownChar
 : .
 ;


/* 
 * fragments 
 */

fragment NonZeroDigit
 : [1-9]
 ;

fragment Digit
 : [0-9]
 ;

fragment Spaces
 : [ \t]+
 ;

fragment Comment
 : '//' ~[\r\n\f]*
 ;

fragment LineJoining
 : '\\' Spaces? ( '\r'? '\n' | '\r' | '\f' )
 ;

fragment IdStart
 : '_'
 | [A-Z]
 | [a-z]
 ;

fragment IdContinue
 : IdStart
 | [0-9]
 ;
 
fragment SingleCharacter
    	:	~['\\\r\n]
    	;
fragment StringCharacters
	:	StringCharacter+
	;
fragment StringCharacter
	:	~["\\\r\n]
	|	EscapeSequence
	;
// §3.10.6 Escape Sequences for Character and String Literals
fragment EscapeSequence
	:	'\\' [btnfr"'\\]
	|	OctalEscape
    |   UnicodeEscape // This is not in the spec but prevents having to preprocess the input
	;

fragment OctalEscape
	:	'\\' OctalDigit
	|	'\\' OctalDigit OctalDigit
	|	'\\' ZeroToThree OctalDigit OctalDigit
	;
// This is not in the spec but prevents having to preprocess the input
fragment UnicodeEscape
    :   '\\' 'u'+  HexDigit HexDigit HexDigit HexDigit
    ;
fragment OctalDigit
	:	[0-7]
	;
fragment HexDigit
	:	[0-9a-fA-F]
	;
fragment ZeroToThree
    	:	[0-3]
    	;
