grammar Lab_1__Excel;


/*
 * Parser Rules
 */
compileUnit : expression EOF;

expression :
	LPAREN expression RPAREN #ParenthesizedExpr
	|expression EXPONENT expression #ExponentialExpr
    | expression operatorToken=(MULTIPLY | DIVIDE) expression #MultiplicativeExpr
	| expression operatorToken=(ADD | SUBTRACT) expression #AdditiveExpr
	| expression operatorToken=(MOD|DIV) expression #ModDivExpr
	| NOT expression #NotExpr
	| expression operatorToken=(LESS | EQUAL | GREATER) expression #ComparatorExpr
	| BOOL #BoolExpr
	| NUMBER #NumberExpr
	| IDENTIFIER #IdentifierExpr
	;
	

/*
 * Lexer Rules
 */
// COMPARATOR : LESS | EQUAL | GREATER;
 BOOL : TRUE|FALSE;

NUMBER : INT ('.' INT)?; 
IDENTIFIER : [a-zA-Z]+[1-9][0-9]+;

INT : ('0'..'9')+;
EXPONENT : '^';
MULTIPLY : '*';
DIVIDE : '/';
SUBTRACT : '-';
ADD : '+';
MOD:'mod';
DIV:'div';
NOT:'not';
TRUE : 'TRUE';
FALSE : 'FALSE';
LESS: '<';
EQUAL : '=';
GREATER : '>';
LPAREN : '(';
RPAREN : ')';


WS : [ \t\r\n] -> channel(HIDDEN);
