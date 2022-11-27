using System;
using Xunit;
using System.Linq;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Wordprocessing;
using Cashpoint.ClientIdentification;
using Cashpoint.ClientTransactions;

namespace xUnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void CheckingTransactionClass_RetrieveBalance_CompareBalances()
        {
            // --Arrange
            string path = "C:\\Users\\mangri\\source\\repos\\Cashpoint\\ClientData\\CardEntry.xlsx";
            XLWorkbook wbook = new XLWorkbook(path);
            IXLWorksheet ws = wbook.Worksheet(1);
            decimal expectedBalance = ws.Cell("C1").GetValue<decimal>();
            IBalance transactions = new Transactions();

            // --Act
            decimal actualBalance =
                transactions.GetBalance(ws.Cell("A1").GetValue<string>());

            // --Assert
            Assert.Equal(expectedBalance, actualBalance);
        }
    }
}
