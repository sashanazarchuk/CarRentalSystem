using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Exceptions
{
    public class RefreshTokenUpdateException : Exception
    {
        public RefreshTokenUpdateException(string message) : base(message) { }
    }
}