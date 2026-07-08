using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using GestionCommerciale.Api.DTOs;

namespace GestionCommerciale.Api.Services;

public class AuthService
{
    private readonly IConfiguration _config;

    // Hardcoded credentials for this mini-project (no user table required by the spec)
    private const string ValidEmail = "tareksoualhia2016@gmail.com";
    private const string ValidPassword = "123456";

    public AuthService(IConfiguration config)
    {
        _config = config;
    }

    public LoginResponseDto? Login(LoginDto dto)
    {
        if (dto.Email != ValidEmail || dto.Password != ValidPassword)
            return null;

        var jwtKey = _config["Jwt:Key"]!;
        var issuer = _config["Jwt:Issuer"]!;
        var audience = _config["Jwt:Audience"]!;
        var expiryMinutes = int.Parse(_config["Jwt:ExpiryMinutes"]!);

        var expiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, dto.Email),
            new Claim(JwtRegisteredClaimNames.Email, dto.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: creds
        );

        return new LoginResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Email = dto.Email,
            ExpiresAt = expiresAt
        };
    }
}