using System;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExcelClassLibrary;
using Lab_1__Excel;

namespace Lab_1__ExcelTest
{
    [TestClass]
    public class Lab_1__ExcelTests
    {
        [TestMethod]
        public void SetValueTest_15plus1_equal_4multiply4_expected_true()
        {
            Cell cell=new Cell(5,5);
            Form1 form1 = new Form1();
            cell.Expr = "(15+1)=(4*4)";
            bool expected = true;
            form1.SetValue(cell);
            Assert.AreEqual(cell.Value, expected);
        }
        [TestMethod]
        public void ConvertReferences_B1plus15_expect_5plus15()
        {
            Table table = new Table(5, 5);
            table.table[0][0].Expr = "B1+15";
            table.table[1][1].Expr = "5";
            table.table[1][1].ValueDouble = 5;
            table.table[1][1].IdentifierValue = false;
            table.dictionary[table.table[1][1].Name] = "5";
            string expexted = "5+15";
            string res=table.ConvertReferences(0, 0, table.table[0][0].Expr);
            Assert.AreEqual(res, expexted);

        }
        [TestMethod]
        public void CheckLoopTest()
        {
            Table table = new Table(5, 5);
            bool expected = true;
            table.table[0][0].Expr = "B0";
            table.table[0][0].referenceFromThisCell.Add(table.table[1][0]);
            table.table[1][0].newReferenceToThisCell.Add(table.table[0][0]);
            table.table[1][0].Expr = "C0";
            table.table[1][0].referenceFromThisCell.Add(table.table[2][0]);
            table.table[2][0].newReferenceToThisCell.Add(table.table[1][0]);
            table.table[2][0].Expr = "A0";
            table.table[2][0].referenceFromThisCell.Add(table.table[0][0]);
            table.table[0][0].referenceToThisCell.Add(table.table[2][0]);
            bool res=table.CheckLoop(table.table[0][0], table.table[0][0]);
            Assert.AreEqual(res, expected);
        }

    }
}
