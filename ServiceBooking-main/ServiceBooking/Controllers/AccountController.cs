using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServiceBooking.DTOs;
using ServiceBooking.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IEmailService emailService;

    public AccountController(UserManager<ApplicationUser> userManager, IEmailService emailService)
    {
        this.userManager = userManager;
        this.emailService = emailService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDTO userdto)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = userdto.UserName,
                Email = userdto.Email,
                PhoneNumber = userdto.PhoneNumber,
                Role = userdto.Role,
                Craft = userdto.Role == "Provider" ? userdto.Craft : null
            };

            var result = await userManager.CreateAsync(user, userdto.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, userdto.Role);

                // ممكن تبعت إيميل ترحيبي هنا لو حابب

                return Ok("تم التسجيل بنجاح");
            }

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("RegisterError", item.Description);
            }
        }

        return BadRequest(ModelState);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO userDTO)
    {
        var user = await userManager.FindByNameAsync(userDTO.UserName);
        if (user != null && await userManager.CheckPasswordAsync(user, userDTO.Password))
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("Role", user.Role),
                new Claim("PhoneNumber", user.PhoneNumber ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperStrongKey_Androw2025_123456"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                user = new
                {
                    user.UserName,
                    user.Role,
                    user.PhoneNumber,
                    user.Craft
                }
            });
        }

        return Unauthorized("Invalid username or password");
    }

    [HttpPost("verify-by-email")]
    public async Task<IActionResult> VerifyUserByEmail([FromQuery] string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
            return NotFound("User not found");

        if (user.EmailConfirmed)
            return BadRequest("User already verified");

        user.EmailConfirmed = true;
        await userManager.UpdateAsync(user);

        string subject = "✅ Account Verified";
        string body = $"<h2>Hello {user.UserName},</h2><p>Your account has been successfully verified.</p>";

        await emailService.SendEmailAsync(user.Email, subject, body);

        return Ok("User verified and email sent.");
    }

    [HttpGet("test-email")]
    public async Task<IActionResult> TestEmail()
    {
        await emailService.SendEmailAsync("your_real_email@gmail.com", "Test Email", "This is a test email from ASP.NET API.");
        return Ok("تم إرسال الإيميل (لو مفيش خطأ).");
    }
}
