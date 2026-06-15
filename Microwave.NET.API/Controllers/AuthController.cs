using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microwave.NET.API.Helpers;
using Microwave.NET.DataStructures.Constants;
using Microwave.NET.DataStructures.DTOs;
using Microwave.NET.DataStructures.Enums.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Microwave.NET.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IWebHostEnvironment env) : ControllerBase
{
    private const string UsersFile = "Data/users.json";

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        var usersPath = Path.Combine(env.ContentRootPath, UsersFile);

        if (!System.IO.File.Exists(usersPath))
            throw new ApiExceptionBase("Nenhum usuário cadastrado. Configure as credenciais primeiro.", 401);

        var json = await System.IO.File.ReadAllTextAsync(usersPath);
        var users = JsonSerializer.Deserialize<List<UserRecord>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? [];

        var user = users.FirstOrDefault(u =>
            u.Username.Equals(request.Username, StringComparison.OrdinalIgnoreCase));

        if (user == null || !CryptoHelper.VerifyPassword(request.Password, user.PasswordHash))
            throw new ApiExceptionBase("Usuário ou senha inválidos.", 401);

        var token = GenerateJwt(user.Username);

        return Ok(ApiResponse.Ok(new { token, expiresIn = 3600 }));
    }

    [HttpPost("register")]
    public async Task<IActionResult> UserRegisterAsync([FromBody] UserRegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            throw new ApiExceptionBase("Username e password são obrigatórios.");

        var usersPath = Path.Combine(env.ContentRootPath, UsersFile);

        List<UserRecord> users = [];
        if (System.IO.File.Exists(usersPath))
        {
            var existingJson = await System.IO.File.ReadAllTextAsync(usersPath);
            users = JsonSerializer.Deserialize<List<UserRecord>>(existingJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? [];
        }

        var existing = users.FirstOrDefault(u =>
            u.Username.Equals(request.Username, StringComparison.OrdinalIgnoreCase));

        if (existing != null)
            existing.PasswordHash = CryptoHelper.HashPasswordSha256(request.Password);
        else
            users.Add(new UserRecord { Username = request.Username, PasswordHash = CryptoHelper.HashPasswordSha256(request.Password) });

        var dir = Path.GetDirectoryName(usersPath)!;
        Directory.CreateDirectory(dir);
        await System.IO.File.WriteAllTextAsync(usersPath,
            JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true }));

        return Ok(ApiResponse.Ok($"Usuário '{request.Username}' configurado com sucesso."));
    }

    private static string GenerateJwt(string username)
    {
        var jwtKey = GlobalConstants.JwtSecret;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "api",
            audience: "client",
            claims: [new Claim(ClaimTypes.Name, username)],
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}



