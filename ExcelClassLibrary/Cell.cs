using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace ExcelClassLibrary
{
    public class Cell
    {
        private int row { get; set; }
        private int column { get; set; }
        private string expr { get; set; }
        private string name;
        private double valueDouble;
        private bool valueBool;
        private bool identifierValue;// if identifier==1, expression is logic and value should be bool,else value is double; 
       public  List<Cell> referenceToThisCell = new List<Cell>();
        public List<Cell> referenceFromThisCell = new List<Cell>();
        public List<Cell> newReferenceToThisCell= new List<Cell>();
        public Cell(int c,int r)
        {
           
            row = r;
            column = c;
            name=Convert.ToString(TwentySixNumeralSystem.ToTwentySixBasedNumeralSystem(column))+Convert.ToString(row);
            expr = "";
            valueDouble = 0.0;
            identifierValue = false;
        }
        public int Column
        {
            get { return column; }
        }
        public int Row
        {
            get { return row; }
        }
        public string Name
        {
            get {
                return name = TwentySixNumeralSystem.ToTwentySixBasedNumeralSystem(column) + row.ToString();
            }
        } 
        public void CheckExpr()
        {
            bool f = false;
            for (int i = 0; i < expr.Length; i++)
            {
                if ((expr[i] == '<') || (expr[i] == '>') || (expr[i] == '='))
                    f = true;
                else if ((expr[i] == 'n') && (expr[i + 1] == 'o') && (expr[i + 2] == 't'))
                    f = true;
            }
            identifierValue= f;
        }
     
        public string Expr
        {
            get { return expr; }
            set { expr = value;}
        }
        public bool ValueBool
        {
            get { return valueBool; }
            set { valueBool = value; }
        }
        public double ValueDouble
        {
            get { return valueDouble; }
            set { valueDouble = value; }
        }
        
        
        public object Value
        {
            get { if (identifierValue == true)
                    return valueBool;
                else return valueDouble;
            }
        }
        public bool IdentifierValue
        {
            get { return identifierValue; }
            set { identifierValue = value; }
        }
        
    }

}
