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

        // ✅ Provider يقدم على Request
        [HttpPost("apply")]
        public async Task<IActionResult> ApplyToRequest(ApplicationDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var provider = await _userManager.FindByIdAsync(userId);
            if (provider == null)
                return Unauthorized("Invalid provider");

            // لازم يكون Provider
            var roles = await _userManager.GetRolesAsync(provider);
            if (!roles.Contains("Provider"))
                return Forbid();

            // تأكد أن الطلب موجود
            var request = await _context.Requests.FindAsync(dto.RequestId);
            if (request == null)
                return NotFound("Request not found");

            // تأكد أنه مفيش Duplicate
            var alreadyApplied = await _context.Applications
                .AnyAsync(a => a.RequestId == dto.RequestId && a.ProviderId == provider.Id);

            if (alreadyApplied)
                return BadRequest("You have already applied to this request");

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

        // ✅ جلب كل الـ Applications الخاصة بـ Request معين (العميل يشوف المتقدمين)
        [HttpGet("request/{requestId}")]
        public async Task<IActionResult> GetApplicationsForRequest(int requestId)
        {
            var applications = await _context.Applications
                .Where(a => a.RequestId == requestId)
                .Include(a => a.Provider)
                .Select(a => new
                {
                    a.Id,
                    a.Message,
                    a.AppliedAt,
                    ProviderName = a.Provider.UserName,
                    ProviderId = a.ProviderId
                })
                .ToListAsync();

            return Ok(applications);
        }

        // ✅ Provider يشوف كل الطلبات اللي قدم عليها
        [HttpGet("my-applications")]
        public async Task<IActionResult> GetMyApplications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var apps = await _context.Applications
                .Where(a => a.ProviderId == userId)
                .Include(a => a.Request)
                .Select(a => new
                {
                    a.Id,
                    a.Message,
                    a.AppliedAt,
                    RequestTitle = a.Request.Notes,
                    RequestId = a.RequestId
                })
                .ToListAsync();

            return Ok(apps);
        }
    }
}
