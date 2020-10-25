using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1__Excel
{
    public static class Calculator
    {
        public static bool EvaluateBool(string expression)
        {
            var lexer = new Lab_1__ExcelLexer(new AntlrInputStream(expression));
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(new ThrowExceptionErrorListener());

            var tokens = new CommonTokenStream(lexer);
            var parser = new Lab_1__ExcelParser(tokens);

            var tree = parser.compileUnit();

            var visitor = new Lab_1_ExcelVisitor();

            return Convert.ToBoolean(visitor.Visit(tree));
        }
        public static double EvaluateDouble(string expression)
        {
            var lexer = new Lab_1__ExcelLexer(new AntlrInputStream(expression));
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(new ThrowExceptionErrorListener());

            var tokens = new CommonTokenStream(lexer);
            var parser = new Lab_1__ExcelParser(tokens);

            var tree = parser.compileUnit();

            var visitor = new Lab_1_ExcelVisitor();

            return visitor.Visit(tree);
        }

    }

}
