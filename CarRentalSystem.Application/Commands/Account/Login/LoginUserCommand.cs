using CarRentalSystem.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Commands.Account.Login
{
    public record LoginUserCommand(string Email, string Password) : IRequest<TokenDto>;
}
