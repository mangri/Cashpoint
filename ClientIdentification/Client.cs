using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Cashpoint.ClientIdentification
{
    public class Client : ICard, IPassword
    {
        private readonly string pathCardEntry = "C:\\Users\\mangri\\source\\repos\\Cashpoint\\ClientData\\CardEntry.xlsx";
        public (bool, string) validatingCard()
        {
            XLWorkbook wbook = new XLWorkbook(pathCardEntry);
            IXLWorksheet ws = wbook.Worksheet(1);
            int nRow = ws.Column("A").CellsUsed().Count();
            Random rnd = new Random();
            string guid = ws.Cell("A" + rnd.Next(1, nRow + 1)
                .ToString())
                .GetValue<string>();
            if(guid == "00000000-0000-0000-0000-000000000000")
            {
                Console.WriteLine("Inserted card is not supported");
                return (false, guid);
            }
            else
            {
                return (true, guid);
            }
        }
        public (bool, decimal) matchingPassword(string guid)
        {
            XLWorkbook wbook = new XLWorkbook(pathCardEntry);
            IXLWorksheet ws = wbook.Worksheet(1);
            for (int i = 1; i < 4; i++)
            {
                Console.WriteLine("Enter PIN for " + guid);
                string rowOfInterest = 
                    ws.Column("A")
                    .CellsUsed(cell => cell.GetString() == guid)
                    .First().WorksheetRow().RowNumber().ToString();
                string expectedPassword = 
                    ws.Cell("B" + rowOfInterest)
                    .GetValue<string>();
                string actualPassword = Console.ReadLine();
                if(expectedPassword == actualPassword)
                {
                    Console.WriteLine("PIN accepted");
                    return (true, ws.Cell("C" + rowOfInterest).GetValue<decimal>());
                }
                else if (i < 3)
                {
                    Console.WriteLine($"Wrong PIN. Try again ({3 - i} attempt(s) left)");
                }
                else if (i == 3)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Closing transaction");
                    Console.ResetColor();
                }
            }
            return (false, 0.00M);
        }
    }
}
