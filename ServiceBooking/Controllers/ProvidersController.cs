using ApiDay1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceBooking.DTOs;
using ServiceBooking.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ServiceBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProviderController(MyContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("GetAllProviders")]
        public async Task<IActionResult> GetAllProviders()
        {
            var providers = await _context.Providers
                .Include(p => p.User) // علشان يجيب بيانات اليوزر المرتبط
                .Select(p => new
                {
                    p.Id,
                    p.DisplayName,
                    p.Bio,
                    p.Rating,
                    UserId = p.UserId,
                    FullName = p.User.Full_Name,
                    Email = p.User.Email,
                    Phone = p.User.PhoneNumber,
                    Role = p.User.Role,
                    Craft = p.User.Craft,
                    Status = p.User.Status
                })
                .ToListAsync();

            return Ok(providers);
        }




        //=============================================================

        [HttpGet("me")]
        //[Authorize(Roles = "Provider")
        public async Task<IActionResult> GetCurrentProvider([FromQuery] string ProviderUserName)
        {
            var user = await _context.Users
          .FirstOrDefaultAsync(u => u.UserName == ProviderUserName);

            if (user == null)
                return NotFound("User not found");

            var Provider = await _context.Providers
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (Provider == null)
                return NotFound("Client not found");

            var ProviderId = Provider.Id;

            var provider = await _context.Providers
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == ProviderId);

            if (provider == null)
                return NotFound("Provider profile not found");

            var dto = new
            {
                provider.Id,
                provider.DisplayName,
                provider.Bio,
                provider.Rating,
                UserId = provider.UserId,
                FullName = provider.User.Full_Name,
                Email = provider.User.Email,
                Phone = provider.User.PhoneNumber,
                Role = provider.User.Role,
                Craft = provider.User.Craft,
                Status = provider.User.Status
            };

            return Ok(dto);
        }
    }

}
