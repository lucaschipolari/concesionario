using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Application.Exceptions
{
    public class ApplicationException : Exception
    {
        public ApplicationException(string message) : base(message)
        {

        }
    }
}
