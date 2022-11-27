using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Cashpoint.ClientTransactions
{
    public class Transactions : IBalance, ITranscript, IWithdrawal
    {
        private readonly string pathCardEntry = "C:\\Users\\mangri\\source\\repos\\Cashpoint\\ClientData\\CardEntry.xlsx";
        public decimal GetBalance(string guid)
        {
            XLWorkbook wbook = new XLWorkbook(pathCardEntry);
            IXLWorksheet ws = wbook.Worksheet(1);
            string rowOfInterest =
                ws.Column("A")
                .CellsUsed(cell => cell.GetString() == guid)
                .First().WorksheetRow().RowNumber()
                .ToString();
            return ws.Cell("C" + rowOfInterest).GetValue<decimal>();
        }
        public void printTranscript(string guid)
        {
            string pathForTranscript = "C:\\Users\\mangri\\source\\repos\\Cashpoint\\ClientData\\Transactions\\" + guid + ".xlsx";
            XLWorkbook wbook = new XLWorkbook(pathForTranscript);
            IXLWorksheet ws = wbook.Worksheet(1);
            int nRow = ws.Column("A").CellsUsed().Count();
            if (nRow > 5)
            {
                string transactionTitle = string.Format("{0, -17}| {1, -8}|      {2:0.00}", "Date and time", "Type", "Amount");
                Console.WriteLine(transactionTitle);
                Console.WriteLine("-----------------------------------------");
                for (int i = nRow - 5 + 1; i <= nRow; i++)
                {
                    string transaction = string.Format("{0, -17}| {1, -8}|      {2:0.00}",
                        ws.Cell("A" + i.ToString()).GetValue<DateTime>().ToString("yyyy-MM-dd HH:mm"),
                        !ws.Cell("B" + i.ToString()).IsEmpty() ? "credit" : "debit",
                        !ws.Cell("B" + i.ToString()).IsEmpty() ? ws.Cell("B" + i.ToString()).GetValue<decimal>() : ws.Cell("C" + i.ToString()).GetValue<decimal>());
                    Console.WriteLine(transaction);
                }
                Console.WriteLine("-----------------------------------------");
            }
            else if (nRow < 5 && nRow > 0)
            {
                string transactionTitle = string.Format("{0, -17}| {1, -8}|      {2:0.00}", "Date and time", "Type", "Amount");
                Console.WriteLine(transactionTitle);
                Console.WriteLine("-----------------------------------------");
                for (int i = 1; i <= nRow; i++)
                {

                    string transaction = string.Format("{0, -17}| {1, -8}|      {2:0.00}",
                        ws.Cell("A" + i.ToString()).GetValue<DateTime>().ToString("yyyy-MM-dd HH:mm"),
                        !ws.Cell("B" + i.ToString()).IsEmpty() ? "credit" : "debit",
                        !ws.Cell("B" + i.ToString()).IsEmpty() ? ws.Cell("B" + i.ToString()).GetValue<decimal>() : ws.Cell("C" + i.ToString()).GetValue<decimal>());
                    Console.WriteLine(transaction);
                }
                Console.WriteLine("-----------------------------------------");
            }
            else if (nRow == 0)
            {
                Console.WriteLine("No transactions registered");
            }
        }
        public (int, decimal) TransactionsToday(string guid)
        {
            string pathForTransactions = "C:\\Users\\mangri\\source\\repos\\Cashpoint\\ClientData\\Transactions\\" + guid + ".xlsx";
            XLWorkbook wbook = new XLWorkbook(pathForTransactions);
            IXLWorksheet ws = wbook.Worksheet(1);
            int nRow = ws.Column("A").CellsUsed().Count();
            int numberOfTransactions = 0;
            decimal totalCredit = 0.00M;
            if (nRow > 0)
            {
                for (int i = 1; i <= nRow; i++)
                {
                    if (ws.Cell("A" + i.ToString()).GetValue<DateTime>().ToString("yyyyMMdd") == DateTime.Today.ToString("yyyyMMdd"))
                    {
                        if (!ws.Cell("B" + i.ToString()).IsEmpty())
                        {
                            totalCredit += ws.Cell("B" + i.ToString()).GetValue<decimal>();
                        }
                        numberOfTransactions++;
                    }
                }
                return (numberOfTransactions, totalCredit);
            }
            else
            {
                return (0, 0.00M);
            }
        }
        public void Crediting(string guid, int totalNumber, decimal totalCredit, decimal balance)
        {
            if (totalNumber < 10 && totalCredit < 1000.00M)
            {
                string pathForWithdrawal = "C:\\Users\\mangri\\source\\repos\\Cashpoint\\ClientData\\Transactions\\" + guid + ".xlsx";
                XLWorkbook wbook = new XLWorkbook(pathForWithdrawal);
                IXLWorksheet ws = wbook.Worksheet(1);
                int nRow = ws.Column("A").CellsUsed().Count();
                Console.WriteLine("Enter the amount to withdraw");
                decimal amount;
                bool validNumber = decimal.TryParse(Console.ReadLine(), out amount);
                if (validNumber && totalCredit + amount <= 1000.00M)
                {
                    if (validNumber && balance - amount >= 0.00M)
                    {
                        Console.WriteLine("Withdrawal granted. Change is being counted.");

                        // Transaction registration
                        ws.Cell("A" + (nRow + 1).ToString()).SetValue(DateTime.Now);
                        ws.Cell("B" + (nRow + 1).ToString()).SetValue(amount);
                        wbook.Save(true);

                        // Changing the balance
                        XLWorkbook wbook_ = new XLWorkbook(pathCardEntry);
                        IXLWorksheet ws_ = wbook_.Worksheet(1);
                        string rowOfInterest =
                            ws_.Column("A")
                            .CellsUsed(cell => cell.GetString() == guid)
                            .First().WorksheetRow().RowNumber()
                            .ToString();
                        ws_.Cell("C" + rowOfInterest).SetValue(balance - amount);
                        wbook_.Save(true);
                    }
                    else
                    {
                        Console.WriteLine("Not enough money in the account");
                    }
                }
                else
                {
                    Console.WriteLine(1000 - totalCredit + "Euro available for Today");
                }
            }
            else
            {
                Console.WriteLine("Today's limit was reached");
            }

        }
    }
}
