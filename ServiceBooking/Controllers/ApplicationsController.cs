using ApiDay1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceBooking.DTOs;
using ServiceBooking.Models;
using System.Security.Claims;

namespace ServiceBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationsController(MyContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("apply")]
        public async Task<IActionResult> ApplyToRequest(ApplicationDTO dto)
        {
            // Get current logged-in provider
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var provider = await _userManager.FindByIdAsync(userId);
            if (provider == null || provider.Role != "Provider")
                return BadRequest("Unauthorized or invalid user");

            // Check if request exists
            var request = await _context.Requests.FindAsync(dto.RequestId);
            if (request == null)
                return NotFound("Request not found");

            // Check if already applied
            var alreadyApplied = await _context.Applications
                .AnyAsync(a => a.RequestId == dto.RequestId && a.ProviderId == provider.Id);

            if (alreadyApplied)
                return BadRequest("You have already applied to this request");

            // Create new application
            var application = new Application
            {
                RequestId = dto.RequestId,
                ProviderId = provider.Id,
                Message = dto.Message,
                AppliedAt = DateTime.UtcNow
            };

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            return Ok("Application submitted successfully");
        }
    }
}
