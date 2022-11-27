using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashpoint.ClientIdentification
{
    internal interface ICard
    {
        (bool, string) validatingCard();
    }
}
