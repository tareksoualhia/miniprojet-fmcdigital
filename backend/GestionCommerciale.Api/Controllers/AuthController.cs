using Microsoft.AspNetCore.Mvc;
using GestionCommerciale.Api.Services;
using GestionCommerciale.Api.DTOs;

namespace GestionCommerciale.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public ActionResult<LoginResponseDto> Login(LoginDto dto)
    {
        var result = _authService.Login(dto);
        if (result is null)
            return Unauthorized(new { message = "Email ou mot de passe incorrect." });

        return Ok(result);
    }
}