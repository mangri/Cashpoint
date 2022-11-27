using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashpoint.ClientTransactions
{
    public interface IBalance
    {
        decimal GetBalance(string guid);
    }
}
