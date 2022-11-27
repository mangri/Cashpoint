using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashpoint.ClientTransactions
{
    internal interface IWithdrawal
    {
        (int, decimal) TransactionsToday(string guid);
        void Crediting(string guid, int totalNumber, decimal totalCredit, decimal balance);
    }
}
