using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashpoint.ClientIdentification
{
    public interface IPassword
    {
        (bool, decimal) matchingPassword(string guid);
    }
}
