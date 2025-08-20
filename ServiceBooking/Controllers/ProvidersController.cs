using ApiDay1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceBooking.DTOs;
using ServiceBooking.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

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

        // ✅ Provider يشوف كل الـ Requests المتاحة
        [HttpGet("{providerId}/available-requests")]
        public async Task<IActionResult> GetAvailableRequests(string providerId)
        {
            var provider = await _userManager.FindByIdAsync(providerId);
            if (provider == null)
                return NotFound("Provider not found");

            // هات كل الـ Requests اللي لسه مفيهاش Applications من الـ Provider ده
            var requests = await _context.Requests
                .Include(r => r.Client) // علشان يرجع بيانات العميل
                .Include(r => r.Service) // علشان يرجع بيانات الخدمة
                .Where(r => !_context.Applications
                    .Any(a => a.RequestId == r.Id && a.ProviderId == providerId))
                .ToListAsync();

            return Ok(requests);
        }

        // ✅ Provider يقدم Application على Request
        [HttpPost("{providerId}/apply/{requestId}")]
        public async Task<IActionResult> ApplyForRequest(string providerId, int requestId)
        {
            var provider = await _userManager.FindByIdAsync(providerId);
            var request = await _context.Requests.FindAsync(requestId);

            if (provider == null || request == null)
                return NotFound("Provider or Request not found");

            // تأكد إنه مش مقدم قبل كده
            var exists = await _context.Applications
                .AnyAsync(a => a.ProviderId == providerId && a.RequestId == requestId);
            if (exists)
                return BadRequest("Already applied for this request");

            var application = new Application
            {
                ProviderId = providerId,
                RequestId = requestId,
                Status = "Pending",
                AppliedAt = DateTime.UtcNow
            };

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            return Ok(application);
        }

        // ✅ Provider يشوف كل الـ Applications اللي قدمها
        [HttpGet("{providerId}/applications")]
        public async Task<ActionResult<IEnumerable<Application>>> GetProviderApplications(string providerId)
        {
            var provider = await _userManager.FindByIdAsync(providerId);
            if (provider == null)
                return NotFound("Provider not found");

            var apps = await _context.Applications
                .Include(a => a.Request)
                .ThenInclude(r => r.Client)
                .Where(a => a.ProviderId == providerId)
                .ToListAsync();

            return Ok(apps);
        }
    }

}
