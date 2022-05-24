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
using PseudoCode.Runtime;
using Range = PseudoCode.Runtime.Range;
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
 | returnStatement
 | callStatement
 ;
assignmentStatement: lvalue ASSIGNMENT_NOTATION expression;
declarationStatement: DECLARE IDENTIFIER COLON dataType;
constantStatement: CONSTANT IDENTIFIER EQUAL expression;
ioStatement: IO_KEYWORD expression;
returnStatement: RETURN expression;
callStatement: CALL? functionCall;

compoundStatement
 : ifStatement
 | caseStatement
 | whileStatement
 | forStatement
 | repeatStatement
 | procedureDefinition
 | functionDefinition
 | typeDefinition
 ;
block: INDENT statement+ DEDENT;
ifStatement: IF expression then ENDIF;
forStatement: FOR assignmentStatement TO arithmeticExpression (STEP arithmeticExpression)? block NEXT IDENTIFIER;
whileStatement: WHILE logicExpression DO block ENDWHILE;
repeatStatement: REPEAT block UNTIL expression;
then: INDENT THEN block (ELSE block)? DEDENT;

caseStatement: CASE arithmeticExpression OF caseBody ENDCASE;
caseBranch
 : (OTHERWISE | atom) COLON (smallStatement | block)
 | NL
 ;
caseBody: INDENT caseBranch+ DEDENT;

procedureDefinition: PROCEDURE IDENTIFIER argumentsDeclaration? block ENDPROCEDURE;
functionDefinition: FUNCTION IDENTIFIER argumentsDeclaration? RETURNS dataType block ENDFUNCTION;
argumentsDeclaration: OPEN_PAREN (argumentDeclaration (COMMA argumentDeclaration)*)? CLOSE_PAREN;
argumentDeclaration: IDENTIFIER COLON dataType;
tuple: expression (COMMA expression)*;

typeDefinition: TYPE IDENTIFIER typeBody ENDTYPE;
typeBody: INDENT typeChild+ DEDENT;
typeChild
 : declarationStatement
 | NL
 ;

expression
 : logicExpression
 ;

logicExpression
 : logicExpression comparisonOp logicExpression
 | NOT logicExpression
 | logicExpression AND logicExpression
 | logicExpression OR logicExpression
 | arithmeticExpression
 | '(' logicExpression ')'
 ;
comparisonOp: SMALLER | GREATER | EQUAL | GREATER_EQUAL | SMALLER_EQUAL | UNEQUAL;

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
 : op=SUB operand=arithmeticExpression {$IsUnary = true;}
 | <assoc=right> operand1=arithmeticExpression op=POW operand2=arithmeticExpression
 | operand1=arithmeticExpression op=MOD operand2=arithmeticExpression
 | operand1=arithmeticExpression op=DIV operand2=arithmeticExpression
 | operand1=arithmeticExpression op=INTDIV operand2=arithmeticExpression
 | operand1=arithmeticExpression op=MULT operand2=arithmeticExpression
 | operand1=arithmeticExpression op=SUB operand2=arithmeticExpression
 | operand1=arithmeticExpression op=ADD operand2=arithmeticExpression
 | rvalue {$IsUnary = true;}
 | '(' arithmeticExpression ')' {$IsUnary = true;}
 ;

 
// Values that can be altered
lvalue
 : IDENTIFIER
 | lvalue array
 | lvalue DOT IDENTIFIER
 ;

// an rvalue cannot be assigned a value
rvalue
 : lvalue
 | atom
 | rvalue DOT IDENTIFIER
 | rvalue array
 | rvalue arguments
 ;

functionCall: rvalue arguments;
arguments: OPEN_PAREN tuple? CLOSE_PAREN;

atom locals [string Type, object Value]
 : number
 | STRING
 | CHAR
 | BOOLEAN
 | DATE
 | array
 ;

dataType locals [string TypeName, bool IsArray, List<Range> Dimensions = new()]
 : ARRAY OPEN_BRACK range (COMMA range)* CLOSE_BRACK OF basicDataType {
    $IsArray = true;
    $TypeName = $basicDataType.text;
 }
 | basicDataType {$TypeName = $basicDataType.text;}
 ;
basicDataType : TYPENAME | IDENTIFIER;
 
range
 : s=INTEGER COLON e=INTEGER {($dataType::Dimensions).Add(new Range{Start = int.Parse($s.text), End = int.Parse($e.text)});}
 ;

array
 : OPEN_BRACK expression (COMMA expression)* CLOSE_BRACK
 ;

//array_index
// : OPEN_BRACK INTEGER (COMMA INTEGER)* CLOSE_BRACK
// ;


/*
 * lexer rules
 */

number
 : INTEGER {$atom::Type = "INTEGER"; $atom::Value = int.Parse($INTEGER.text);}
 | DECIMAL {$atom::Type = "REAL"; $atom::Value = decimal.Parse($DECIMAL.text);}
 ;
 

 
STRING
 : '"' .*? '"' 
 ;
CHAR
 : '\'' . '\''
 ;

// Value is either 1. or .1 and not just a single '.' to avoid ambiguity with DOT
DECIMAL
 : '-'? (DIGIT* '.' DIGIT+ | DIGIT+ '.' DIGIT*)
 ;
INTEGER : '-' ? DIGIT+;

BOOLEAN : 'TRUE' | 'FALSE';

DATE : DIGIT DIGIT '/' DIGIT DIGIT '/' DIGIT DIGIT DIGIT DIGIT;
 


NL: ('\r'? '\n' ' '*); // For tabs just switch out ' '* with '\t'*
NEWLINE
 : ( '\r'? '\n' | '\r' | '\f' ) SPACES?
 ;

DECLARE : 'DECLARE';
CONSTANT : 'CONSTANT';
IO_KEYWORD : 'PRINT' | 'OUTPUT' | 'INPUT';
TYPENAME : 'INTEGER' | 'STRING' | 'REAL' | 'CHAR' | 'BOOLEAN' | 'DATE';
ARRAY : 'ARRAY';
CASE : 'CASE';
OF : 'OF';
OTHERWISE : 'OTHERWISE';
ENDCASE : 'ENDCASE';
DO : 'DO';
FOR : 'FOR';
TO : 'TO';
STEP : 'STEP';
NEXT : 'NEXT';
WHILE : 'WHILE';
ENDWHILE : 'ENDWHILE';
REPEAT : 'REPEAT';
UNTIL : 'UNTIL';
IF : 'IF';
THEN : 'THEN';
ELSE : 'ELSE';
ENDIF : 'ENDIF';
PROCEDURE : 'PROCEDURE';
ENDPROCEDURE : 'ENDPROCEDURE';
CALL : 'CALL';
FUNCTION : 'FUNCTION';
ENDFUNCTION : 'ENDFUNCTION';
RETURNS : 'RETURNS';
RETURN : 'RETURN';
TYPE : 'TYPE';
ENDTYPE : 'ENDTYPE';

// Logic
AND : 'AND';
OR : 'OR';
NOT : 'NOT';


IDENTIFIER
 : ID_START ID_CONTINUE*
 ;



OPEN_PAREN : '(';
CLOSE_PAREN : ')';
OPEN_BRACK : '[';
CLOSE_BRACK : ']';
OPEN_BRACE : '{';
CLOSE_BRACE : '}';
COLON : ':';
COMMA : ',';
DOT : '.';
ADD : '+';
SUB : '-';
MULT : '*';
INTDIV : 'DIV' | '//';
DIV : '/';
MOD : 'MOD' | '%';
POW : 'POW' | '^';
EQUAL : '=';
GREATER : '>';
SMALLER : '<';
GREATER_EQUAL : '>=';
SMALLER_EQUAL : '<=';
UNEQUAL : '<>';

ASSIGNMENT_NOTATION : '<-';

SKIP_
 : ( SPACES | COMMENT | LINE_JOINING ) -> skip
 ;

UNKNOWN_CHAR
 : .
 ;


/* 
 * fragments 
 */

fragment NON_ZERO_DIGIT
 : [1-9]
 ;

fragment DIGIT
 : [0-9]
 ;

fragment SPACES
 : [ \t]+
 ;

fragment COMMENT
 : '#' ~[\r\n\f]*
 ;

fragment LINE_JOINING
 : '\\' SPACES? ( '\r'? '\n' | '\r' | '\f' )
 ;

fragment ID_START
 : '_'
 | [A-Z]
 | [a-z]
 ;

fragment ID_CONTINUE
 : ID_START
 | [0-9]
 ;