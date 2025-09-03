using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Commands.Account.Logout
{
    public record LogoutUserCommand(string UserId) : IRequest;
}
