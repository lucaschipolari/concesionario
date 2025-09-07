using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Application.Exceptions
{
    public class DuplicatedNameException : System.ApplicationException
    {
        public DuplicatedNameException(string message) : base(message)
        {

        }
    }
}
