using ApiDay1.Models;
using Microsoft.AspNetCore.Authorization;
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
        private readonly MyContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailService emailService;
        private readonly IConfiguration config;

        public AccountController(MyContext context , UserManager<ApplicationUser> userManager, IEmailService emailService, IConfiguration config)
        {
            _context = context;
            this.userManager = userManager;
            this.emailService = emailService;
            this.config = config;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterUserDTO userdto)
        {
            if (ModelState.IsValid)
            {
                var verificationCode = new Random().Next(100000, 999999).ToString();

                string? imagePath = null;
                if (userdto.NationalIdImage != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(userdto.NationalIdImage.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/nationalIds", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await userdto.NationalIdImage.CopyToAsync(stream);
                    }

                    imagePath = $"/nationalIds/{fileName}";
                }

                var user = new ApplicationUser
                {
                    UserName = userdto.UserName,
                    Full_Name = userdto.FullName,
                    Email = userdto.Email,
                    PhoneNumber = userdto.PhoneNumber,
                    Role = userdto.Role,
                    Craft = userdto.Role == "Provider" ? userdto.Craft : null,
                    VerificationCode = verificationCode,
                    IsVerified = false,
                    NationalIdImage = imagePath
                };
                var result = await userManager.CreateAsync(user, userdto.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, userdto.Role);

                    // إضافة للـ profile حسب الدور
                    if (userdto.Role == "Client")
                    {
                        var clientProfile = new Client
                        {
                            UserId = user.Id,
                            User = user
                        };

                        _context.Clients.Add(clientProfile);
                    }
                    else if (userdto.Role == "Provider")
                    {
                        var providerProfile = new Provider
                        {
                            UserId = user.Id,
                            User = user,
                            DisplayName = user.Full_Name,
                            Bio = userdto.Craft != null ? $"Specialized in {userdto.Craft}" : null
                        };

                        _context.Providers.Add(providerProfile);
                    }

                    await _context.SaveChangesAsync();


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

            if (user == null || !await userManager.CheckPasswordAsync(user, userDTO.Password))
                return Unauthorized("Invalid username or password");

            if (!user.IsVerified)
                return Unauthorized("Please verify your account first by checking your email.");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("Role", user.Role),
                new Claim("PhoneNumber", user.PhoneNumber ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var jwtSettings = config.GetSection("JWT");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(double.Parse(jwtSettings["DurationInDays"] ?? "1")),
 signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                user = new
                {
                    user.Full_Name,
                    user.UserName,
                    user.Role,
                    user.PhoneNumber,
                    user.Craft,
                    user.NationalIdImage
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
                user.VerificationCode = null;
                await userManager.UpdateAsync(user);
                return Ok("Account verified successfully.");
            }

            return BadRequest("Invalid code.");
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userName = User.Identity?.Name;

            if (string.IsNullOrEmpty(userName))
                return Unauthorized("User not logged in");

            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
                return NotFound("User not found");

            var roles = await userManager.GetRolesAsync(user);

            return Ok(new
            {
                FullName = user.Full_Name,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = roles.FirstOrDefault() ?? "User",
                Craft = user.Craft,
                NationalIdImage = user.NationalIdImage
            });
        }

    }
}
