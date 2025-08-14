using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Application.Exceptions
{
    public class DuplicatedEntityException : System.ApplicationException
    {
        public DuplicatedEntityException(string message) : base(message)
        {

        }
    }
}
