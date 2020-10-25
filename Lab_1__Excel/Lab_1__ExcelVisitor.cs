using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1__Excel


{
    class Lab_1_ExcelVisitor : Lab_1__ExcelBaseVisitor<double>
    {
        //таблиця ідентифікаторів (тут для прикладу)
        //в лабораторній роботі заміните на свою!!!!
        Dictionary<string, double> tableIdentifier = new Dictionary<string, double>();

        public override double VisitCompileUnit(Lab_1__ExcelParser.CompileUnitContext context)
        {
            return Visit(context.expression());
        }

        public override double VisitNumberExpr(Lab_1__ExcelParser.NumberExprContext context)
        {
            var result = double.Parse(context.GetText());
            Debug.WriteLine(result);

            return result;
        }

        //IdentifierExpr
        public override double VisitIdentifierExpr(Lab_1__ExcelParser.IdentifierExprContext context)
        {
            var result = context.GetText();
            double value;
            //видобути значення змінної з таблиці
            if (tableIdentifier.TryGetValue(result.ToString(), out value))
            {
                return value;
            }
            else
            {
                return 0.0;
            }
        }

        public override double VisitParenthesizedExpr(Lab_1__ExcelParser.ParenthesizedExprContext context)
        {
            return Visit(context.expression());
        }

        public override double VisitExponentialExpr(Lab_1__ExcelParser.ExponentialExprContext context)
        {
            var left = WalkLeft(context);
            var right = WalkRight(context);

            Debug.WriteLine("{0} ^ {1}", left, right);
            return System.Math.Pow(left, right);
        }

        public override double VisitAdditiveExpr(Lab_1__ExcelParser.AdditiveExprContext context)
        {
            var left = WalkLeft(context);
            var right = WalkRight(context);

            if (context.operatorToken.Type == Lab_1__ExcelParser.ADD)
            {
                Debug.WriteLine("{0} + {1}", left, right);
                return left + right;
            }
            else //LabCalculatorLexer.SUBTRACT
            {
                Debug.WriteLine("{0} - {1}", left, right);
                return left - right;
            }
        }

        public override double VisitMultiplicativeExpr(Lab_1__ExcelParser.MultiplicativeExprContext context)
        {
            var left = WalkLeft(context);
            var right = WalkRight(context);

            if (context.operatorToken.Type == Lab_1__ExcelParser.MULTIPLY)
            {
                Debug.WriteLine("{0} * {1}", left, right);
                return left * right;
            }
            else //LabCalculatorLexer.DIVIDE
            {
                Debug.WriteLine("{0} / {1}", left, right);
                return left / right;
            }
        }
        public override double VisitComparatorExpr(Lab_1__ExcelParser.ComparatorExprContext context)
        {
            var left = WalkLeft(context);
            var right = WalkRight(context);
            if (context.operatorToken.Type == Lab_1__ExcelParser.LESS)
            {
                Debug.WriteLine("{0} <{1}", left, right);
                return Convert.ToDouble(left < right);
                   
                
            }
            else if(context.operatorToken.Type == Lab_1__ExcelParser.EQUAL)//LabCalculatorLexer.DIVIDE
            {
                Debug.WriteLine("{0}={1}", left, right);
                return Convert.ToDouble(left == right);
            }
            else
            {
                Debug.WriteLine("{0}>{1}", left, right);
                return Convert.ToDouble(left > right);
            }
        }
        public override double VisitModDivExpr([NotNull] Lab_1__ExcelParser.ModDivExprContext context)
        {
            var left = WalkLeft(context);
            var right = WalkRight(context);
            if(context.operatorToken.Type == Lab_1__ExcelParser.MOD)
            {
                Debug.WriteLine("{0}mod{1}", left, right);
                return left % right;
            }
            else
            {
                Debug.WriteLine("{0}div{1}", left, right);
                return  (left - left % right) / right;
            }

        }
        public override double VisitNotExpr([NotNull] Lab_1__ExcelParser.NotExprContext context)
        {
            var right = WalkLeft(context);
            
            Debug.WriteLine("not{0}",right);
                if (right == 0
                )
                    return 1;
                else return 0;
            
           
        }
        private double WalkLeft(Lab_1__ExcelParser.ExpressionContext context)
        {
            return Visit(context.GetRuleContext<Lab_1__ExcelParser.ExpressionContext>(0));
        }

        private double WalkRight(Lab_1__ExcelParser.ExpressionContext context)
        {
            return Visit(context.GetRuleContext<Lab_1__ExcelParser.ExpressionContext>(1));
        }
    }
}

