using CarRentalSystem.Application.DTOs;
using CarRentalSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Interfaces.Services
{
    public interface IJwtTokenService
    {
        Task<string> CreateAccessToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        Task<TokenDto> RefreshToken(TokenDto tokenDto);
    }
}
