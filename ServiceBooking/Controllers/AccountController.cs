using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServiceBooking.DTOs;
using ServiceBooking.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ServiceBooking.Controllers
{
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

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserDTO userdto)
        {
            if (ModelState.IsValid)
            {
                var verificationCode = new Random().Next(100000, 999999).ToString();

                var user = new ApplicationUser
                {
                    UserName = userdto.UserName,
                    Email = userdto.Email,
                    PhoneNumber = userdto.PhoneNumber,
                    Role = userdto.Role,
                    Craft = userdto.Role == "Provider" ? userdto.Craft : null,
                    VerificationCode = verificationCode,
                    IsVerified = false
                };

                var result = await userManager.CreateAsync(user, userdto.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, userdto.Role);
                    await emailService.SendEmailAsync(user.Email, "Verification Code", $"Your code is: {verificationCode}");

                    return Ok("User registered. Verification code sent to email.");
                }

                return BadRequest(result.Errors);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO userDTO)
        {
            var user = await userManager.FindByNameAsync(userDTO.UserName);

            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }

          
            if (!user.IsVerified)
            {
                return Unauthorized("Please verify your account first by checking your email.");
            }

            if (!await userManager.CheckPasswordAsync(user, userDTO.Password))
            {
                return Unauthorized("Invalid username or password");
            }

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

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeDTO dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return NotFound("User not found");

            if (user.IsVerified)
                return BadRequest("Account already verified.");

            if (user.VerificationCode == dto.Code)
            {
                user.IsVerified = true;
                user.VerificationCode = null; // حذف الكود بعد التفعيل
                await userManager.UpdateAsync(user);
                return Ok("Account verified successfully.");
            }

            return BadRequest("Invalid code.");
        }




    }
}
