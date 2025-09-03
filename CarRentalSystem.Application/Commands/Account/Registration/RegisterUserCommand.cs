using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Commands.Account.Registration
{
    public record RegisterUserCommand(string FullName, string Email, string PhoneNumber, string Password) : IRequest<IdentityResult>;

}
