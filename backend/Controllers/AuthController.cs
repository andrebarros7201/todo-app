using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
[ApiController]
public class AuthController: ControllerBase
{
    private readonly Context _context;
    public AuthController(Context context)
    {
        _context = context;
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if(await _context.Users.AnyAsync(u => u.Username == model.Username))
        {
            return BadRequest("Username already exists");
        }

        var user = new User
        {
            Username = model.Username,
            Password = HashPassword(model.Password)
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("User Registered Successfully");
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}

public class RegisterModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}