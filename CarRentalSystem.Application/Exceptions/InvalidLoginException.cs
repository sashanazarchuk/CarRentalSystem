using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Exceptions
{
    public class InvalidLoginException:Exception
    {
        public InvalidLoginException() : base("Invalid email or password.") { }
        public InvalidLoginException(string message) : base(message) { }
    }
}
