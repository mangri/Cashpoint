using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashpoint.ClientTransactions
{
    internal interface ITranscript
    {
        void printTranscript(string guid);
    }
}
