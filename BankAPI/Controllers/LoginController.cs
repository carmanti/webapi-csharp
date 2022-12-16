using System.Text;
using System.Security.Claims;
using BankAPI.Data.DTOs;
using BankAPI.DataBankModels;
using BankAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace BankAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly LoginServices loginServices;
    private IConfiguration config;

    public LoginController(LoginServices loginServices, IConfiguration config)
    {
        this.loginServices = loginServices;
        this.config = config;
    }

    [HttpPost("authenticate")]
    public async Task<ActionResult> Login(AdminDTO adminDto)
    {
        var admin = await loginServices.GetAdmin(adminDto);

        if (admin is null)
            return BadRequest(new { message = "Credenciales invalidas" });

        //generar token

        string jwtToken = GenerateToken(admin);

        return Ok(new { token = jwtToken });
    }

    private string GenerateToken(Administrator admin)
    {
        var claims = new[]{
            new Claim(ClaimTypes.Name, admin.Name),
            new Claim(ClaimTypes.Email, admin.Email),
            new Claim("AdminType", admin.AdminType)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWT:Key").Value));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var securityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: creds
        );

        string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return token;
    }
}