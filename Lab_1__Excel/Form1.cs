using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelClassLibrary;

namespace Lab_1__Excel
{
    public partial class Form1 : Form
    {
        Table table = new Table(5,5);
        public Form1()
        {
            InitializeComponent();
            DataGridViewInitialize(5,5);
        }

        
        private void DataGridViewInitialize(int columnsCount,int rowsCount)
        {
            dataGridView1.ColumnCount = columnsCount;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ColumnHeadersVisible = true;
            
            for(int i = 0; i < columnsCount; i++)
            {
                dataGridView1.Columns[i].Name = TwentySixNumeralSystem.ToTwentySixBasedNumeralSystem(i);
            }
            dataGridView1.RowCount = rowsCount;
            dataGridView1.RowHeadersVisible = true;
            for(int i = 0; i < rowsCount; i++)
            {
                dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.ColumnHeadersVisible = true;
            dataGridView1.RowHeadersVisible = true;
            int count = dataGridView1.ColumnCount;
            dataGridView1.ColumnCount += 1;
            table.AddColumns(1);
            dataGridView1.Columns[count].Name = TwentySixNumeralSystem.ToTwentySixBasedNumeralSystem(count);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int count= dataGridView1.RowCount;
            dataGridView1.ColumnHeadersVisible = true;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.RowCount += 1;
            table.AddRows(1);
            dataGridView1.RowHeadersVisible = true;
            for (int i = 0; i <= count; i++)
            {
                dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (DeleteColumn())
            {
                dataGridView1.ColumnCount -= 1;
                if (dataGridView1.ColumnCount == 0)
                { dataGridView1.RowHeadersVisible = false;
                    table.Clear();
                }
                table.ColumnsCount--;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (DeleteRow())
            {
                dataGridView1.RowCount -= 1;
                if (dataGridView1.RowCount == 0)
                {
                    dataGridView1.ColumnHeadersVisible = false;
                    table.Clear();
                }
                table.RowsCount--;
            }
        }
       private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int colmn = dataGridView1.SelectedCells[0].ColumnIndex;
            int row = dataGridView1.SelectedCells[0].RowIndex;
            table.table[colmn][row].Expr = Convert.ToString(dataGridView1.SelectedCells[0].EditedFormattedValue);
            table.table[colmn][row].referenceFromThisCell.Clear();
            SetValue(table.table[colmn][row]);
            if (table.table[colmn][row].IdentifierValue == true)
                table.dictionary[table.table[colmn][row].Name] = Convert.ToString(Convert.ToDouble(table.table[colmn][row].ValueBool));
            else table.dictionary[table.table[colmn][row].Name] = Convert.ToString(table.table[colmn][row].ValueDouble);
            if (table.CheckLoop(table.table[colmn][row], table.table[colmn][row]))
            { MessageBox.Show("There is a loop");
                table.table[colmn][row].Expr = "";
                table.table[colmn][row].ValueBool = false;
                table.table[colmn][row].ValueDouble = 0.0;
                table.dictionary[table.table[colmn][row].Name] = "";
            }
            else
            {
                dataGridView1[colmn, row].Value = table.table[colmn][row].Value;
                RefreshValue(colmn, row);
            }
            
        }
       public void SetValue(Cell cell )
        {
            try
            {
                cell.CheckExpr();
                if (cell.IdentifierValue == true)
                {
                    cell.ValueBool = Calculator.EvaluateBool(table.ConvertReferences(cell.Column, cell.Row, cell.Expr));
                }
                else
                {
                    cell.ValueDouble = Calculator.EvaluateDouble(table.ConvertReferences(cell.Column, cell.Row, cell.Expr));
                }
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }
        public void RefreshValue(int column, int row)
        {
            table.table[column][row].referenceToThisCell.Clear();
            for(int i=0;i< table.table[column][row].newReferenceToThisCell.Count;i++)
                table.table[column][row].referenceToThisCell.Add(table.table[column][row].newReferenceToThisCell[i]);
            table.table[column][row].newReferenceToThisCell.Clear();
            foreach (Cell cell in table.table[column][row].referenceToThisCell)
            {   try
                    {
                        if (cell.IdentifierValue == true)
                        {
                            cell.ValueBool = Calculator.EvaluateBool(table.ConvertReferences(cell.Column, cell.Row, cell.Expr));
                            table.dictionary[table.table[column][row].Name] = Convert.ToString(Convert.ToDouble(table.table[column][row].ValueBool));
                        }
                        else
                        {
                            cell.ValueDouble = Calculator.EvaluateDouble(table.ConvertReferences(cell.Column, cell.Row, cell.Expr));
                            table.dictionary[table.table[column][row].Name] = Convert.ToString(table.table[column][row].ValueDouble);
                        }
                        dataGridView1[cell.Column, cell.Row].Value = table.table[cell.Column][cell.Row].Value;
                        RefreshValue(cell.Column, cell.Row);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                    }
                if (cell.newReferenceToThisCell.Count != 0)
                    RefreshValue(cell.Column, cell.Row);
            }
        }
        private void RefreshCellandReferences(Cell cell)
        {
            cell.referenceFromThisCell.Clear();
            foreach(Cell point in cell.newReferenceToThisCell)
            {
                RefreshValue(point.Column, point.Row);
            }

        }
        private bool DeleteRow()
        {
            List<Cell> referencesToLastRow = new List<Cell>();
            List<Cell> notEmptyCells = new List<Cell>();
            string errorMesssage = "";
            int rowCount = dataGridView1.RowCount;
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                if (table.table[i][rowCount - 1].newReferenceToThisCell.Count != 0)
                {
                    referencesToLastRow.Add(table.table[i][rowCount - 1]);
                }
                if ((table.dictionary[table.table[i][rowCount - 1].Name] != "") || (table.dictionary[table.table[i][rowCount - 1].Name] == "0"))
                {
                    notEmptyCells.Add(table.table[i][rowCount - 1]);
                }
            }
            if(notEmptyCells.Count!=0)
            {
                errorMesssage = "There are meaningful cells. ";
            }
            else if(referencesToLastRow.Count!=0)
            {
                errorMesssage = "There are references to cells from this row. ";
            }
            if (errorMesssage != "")
            {
                errorMesssage += "Are you sure you want to delete this cell?";
                System.Windows.Forms.DialogResult res = System.Windows.Forms.MessageBox.Show(errorMesssage, "Warning", System.Windows.Forms.MessageBoxButtons.YesNo);
                if (res == System.Windows.Forms.DialogResult.No)
                    return false;
            }
            else
            {
                for(int i=0;i<dataGridView1.ColumnCount;i++)
                {
                   bool t=table.dictionary.Remove(table.table[i][rowCount - 1].Name);
                   table.table[i].RemoveAt(rowCount - 1);
                }
                foreach (Cell cell in referencesToLastRow)
                {
                    RefreshCellandReferences(cell);
                }
            }
            return true;
        }
        private bool DeleteColumn()
        {
            List<Cell> referencesToLastColumn = new List<Cell>();
            List<Cell> notEmptyCells = new List<Cell>();
            string errorMesssage = "";
            int columnCount = dataGridView1.ColumnCount;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (table.table[columnCount-1][i].newReferenceToThisCell.Count != 0)
                {
                    referencesToLastColumn.Add(table.table[columnCount - 1][i]);
                }
                if ((table.dictionary[table.table[columnCount-1][i].Name] != "") || (table.dictionary[table.table[columnCount-1][i].Name] == "0"))
                {
                    notEmptyCells.Add(table.table[columnCount - 1][i]);
                }
            }
            if (notEmptyCells.Count != 0)
            {
                errorMesssage += "There are meaningful cells. ";
            }
            else if (referencesToLastColumn.Count != 0)
            {
                errorMesssage += "There are references to cells from this column. ";
            }
            if (errorMesssage != "")
            {
                errorMesssage += "Are you sure you want to delete this cell?";
                System.Windows.Forms.DialogResult res = System.Windows.Forms.MessageBox.Show(errorMesssage, "Warning", System.Windows.Forms.MessageBoxButtons.YesNo);
                if (res == System.Windows.Forms.DialogResult.No)
                    return false;
            }
           
            {
                int i = table.table[columnCount-1].Count-1;
                while (i>=0)
                {                    
                    table.dictionary.Remove(table.table[columnCount-1][i].Name);
                    //table.table[columnCount - 1].RemoveAt(i);
                     i--;

                }
                table.table.RemoveAt(columnCount - 1);
                foreach (Cell cell in referencesToLastColumn)
                {
                    RefreshCellandReferences(cell);
                }
            }
            return true;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            foreach(List<Cell> list in table.table)
            {
                foreach(Cell cell in list)
                {
                    dataGridView1[cell.Column, cell.Row].Value = cell.Value;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            foreach (List<Cell> list in table.table)
            {
                foreach (Cell cell in list)
                {
                    dataGridView1[cell.Column, cell.Row].Value = cell.Expr;
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)//save table
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "TableFile|*.txt";
            saveFileDialog.Title = "Save table into file";
            saveFileDialog.ShowDialog();
            if(saveFileDialog.FileName!="")
            {
                FileStream fs = (FileStream)saveFileDialog.OpenFile();
                StreamWriter sw = new StreamWriter(fs);
                table.Save(sw);
                sw.Close();
                fs.Close();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "TableFile|*.txt";
            openFileDialog.Title = "Open Table File";
            if(openFileDialog.ShowDialog()!=DialogResult.OK)
            { return; }
            StreamReader sr = new StreamReader(openFileDialog.FileName);
            table.Clear();
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            int row;
            int column;
            Int32.TryParse(sr.ReadLine(), out column);
            Int32.TryParse(sr.ReadLine(), out row);
            DataGridViewInitialize(column, row);
            table = new Table(column, row);
            table.Open(row, column, sr);
            foreach(List<Cell> list in table.table)
                foreach(Cell cell in list)
                {
                    dataGridView1[cell.Column, cell.Row].Value = cell.Value;
                }
        }
    }
}
