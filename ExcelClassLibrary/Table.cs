using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;



namespace ExcelClassLibrary
{
    public class Table
    {
        private int columnsCount;
        private int rowsCount;
        public List<List<Cell>> table = new List<List<Cell>>();
        public Dictionary<string, string> dictionary = new Dictionary<string, string>();
        public bool LoopIdentifier = false;
        public int ColumnsCount
        {
            get { return columnsCount; }
            set { columnsCount = value; }
        }
        public int RowsCount
        {
            get { return rowsCount; }
            set { rowsCount = value; }
        }
        public Table(int c, int r)
        {
            columnsCount = c;
            rowsCount = r;
            for (int i = 0; i < columnsCount; i++)
            {
                List<Cell> newColumn = new List<Cell>();
                for (int j = 0; j < rowsCount; j++)
                {
                    newColumn.Add(new Cell(i, j));
                    dictionary.Add(newColumn.Last().Name, "");
                }
                table.Add(newColumn);
            }
        }        
        public void AddRows(int i)
        {
            rowsCount += i;
           for(int j=0;j<columnsCount;j++)
            {
                table[j].Add(new Cell(j, rowsCount - 1));
                dictionary.Add(table[j][rowsCount - 1].Name, "");
            }
        }
        public void AddColumns(int i)
        {
            List<Cell> newColumn = new List<Cell>();
            columnsCount += i;
            for (int j = 0; j < rowsCount; j++)
            {
                newColumn.Add(new Cell(columnsCount - 1, j));
                dictionary.Add(newColumn.Last().Name, "");
            }
            table.Add(newColumn);
           
        }
        public void Clear()
        {
            table.Clear();
            dictionary.Clear();
            rowsCount = 0;
            columnsCount = 0;
        }
        public void DeleteRows(int i)
        {
            if (rowsCount > i)
                rowsCount -= i;
            else
                rowsCount = 0;
        }
        public void DeleteColumns(int i)
        {
            if (columnsCount > i)
                columnsCount -= i;
            else
                columnsCount = 0;
        }
        public string ConvertReferences(int colmn, int row, string expr)
        {
            string CellPattern = @"[A-Z]+[0-9]+";
            Regex regex = new Regex(CellPattern, RegexOptions.IgnoreCase);
            Index res;
            foreach (Match match in regex.Matches(expr))
            {
                if (dictionary.ContainsKey(match.Value))
                {
                    res = TwentySixNumeralSystem.FromTwentySixBasedNumeralSystem(match.Value);
                    if ((colmn != res.column) || (row != res.row))
                    {   
                        table[colmn][row].referenceFromThisCell.Add(table[res.column][res.row]);
                        table[res.column][res.row].newReferenceToThisCell.Add(table[colmn][row]);
                    }
                    else throw new Exception("There is a loop"); 
                }
            }
                MatchEvaluator ev = new MatchEvaluator(referenceToValue);
                string newExpr = regex.Replace(expr, ev);
                return newExpr;
        }
        public string referenceToValue(Match m)
        {
            if (dictionary.ContainsKey(m.Value))
            {
                if (dictionary[m.Value] == "")
                    return "0";
                else
                    return dictionary[m.Value];
            }
            return m.Value;
        }
        public bool CheckLoop(Cell cell1,Cell cell2)
        {
            foreach (Cell cell in cell2.referenceFromThisCell)
            {
                if (cell1.Name == cell.Name)
                    return true;
                else
                {if (cell.referenceFromThisCell.Count != 0)
                        if (CheckLoop(cell1, cell) == true)
                            return true;
                }
            }

            return false;
        }
        public void Open(int colmn, int row, StreamReader str)
        {
            for(int i=0;i<colmn;i++)
                for(int j=0;j<row;j++)
                {
                    string index = str.ReadLine();
                    string expression = str.ReadLine();
                    string value = str.ReadLine();
                    string boolIdentifier = str.ReadLine();
                    if (expression != "")
                        dictionary[index] = value;
                    else
                        dictionary[index] = "";
                    table[i][j].Expr = expression;
                    if(Convert.ToBoolean(boolIdentifier)==true)
                    {
                        table[i][j].IdentifierValue = true;
                        table[i][j].ValueBool = Convert.ToBoolean(value);
                    }
                    else
                    {
                        table[i][j].IdentifierValue = false;
                        table[i][j].ValueDouble = Convert.ToDouble(value);
                    }
                    int refCount = Convert.ToInt32(str.ReadLine());
                    string refer;
                    for(int k=0;k<refCount;k++)
                    { refer = str.ReadLine();
                      if((TwentySixNumeralSystem.FromTwentySixBasedNumeralSystem(refer).row<rowsCount)&&(TwentySixNumeralSystem.FromTwentySixBasedNumeralSystem(refer).column) < columnsCount)
                               table[i][j].newReferenceToThisCell.Add(table[TwentySixNumeralSystem.FromTwentySixBasedNumeralSystem(refer).column][TwentySixNumeralSystem.FromTwentySixBasedNumeralSystem(refer).row]);
                    }
                    int refFromCount = Convert.ToInt32(str.ReadLine());
                    for (int k = 0; k < refFromCount; k++)
                    {
                        refer = str.ReadLine();
                        if ((TwentySixNumeralSystem.FromTwentySixBasedNumeralSystem(refer).row < rowsCount) && (TwentySixNumeralSystem.FromTwentySixBasedNumeralSystem(refer).column) < columnsCount)
                            table[i][j].referenceFromThisCell.Add(table[TwentySixNumeralSystem.FromTwentySixBasedNumeralSystem(refer).column][TwentySixNumeralSystem.FromTwentySixBasedNumeralSystem(refer).row]);
                    }
                    
                }
        }
        public void Save(System.IO.StreamWriter sw) 
        {
            sw.WriteLine(columnsCount);
            sw.WriteLine(rowsCount);
            foreach(List<Cell> list in table)
            {
                foreach(Cell cell in list)
                {
                    sw.WriteLine(cell.Name);
                    sw.WriteLine(cell.Expr);
                    sw.WriteLine(cell.Value);
                    sw.WriteLine(cell.IdentifierValue);
                    if (cell.newReferenceToThisCell.Count == 0)
                        sw.WriteLine("0");
                    else
                    {
                        sw.WriteLine(cell.newReferenceToThisCell.Count);
                        foreach(Cell cell1 in cell.newReferenceToThisCell)
                        {
                            sw.WriteLine(cell1.Name);
                        }
                    }
                    if (cell.referenceFromThisCell.Count == 0)
                        sw.WriteLine("0");
                    else
                    {
                        sw.WriteLine(cell.referenceFromThisCell.Count);
                        foreach (Cell cell1 in cell.referenceFromThisCell)
                        {
                            sw.WriteLine(cell1.Name);
                        }
                    }
                }
            }
        
        }
    }
}
