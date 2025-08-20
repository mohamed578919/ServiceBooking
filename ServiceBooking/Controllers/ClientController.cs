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
    [Authorize(Roles = "Client")]
    public class ClientController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClientController(MyContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // 1️⃣ يعمل Request جديد
        [HttpPost("CreateRequest")]
        public async Task<IActionResult> CreateRequest([FromBody] Request requestDto)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            var request = new Request
            {
                Title = requestDto.Title,
                Description = requestDto.Description,
                ClientId = user.Id
            };

            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Request created successfully", request });
        }

        // 2️⃣ يجيب Requests بتاعته
        [HttpGet("MyRequests")]
        public async Task<IActionResult> GetMyRequests()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            var requests = await _context.Requests
                .Where(r => r.ClientId == user.Id)
                .Include(r => r.Applications)
                .ToListAsync();

            return Ok(requests);
        }

        // 3️⃣ يعدل Request بتاعه
        [HttpPut("UpdateRequest/{id}")]
        public async Task<IActionResult> UpdateRequest(int id, [FromBody] Request updatedRequest)
        {
            var user = await _userManager.GetUserAsync(User);

            var request = await _context.Requests.FirstOrDefaultAsync(r => r.Id == id && r.ClientId == user.Id);

            if (request == null)
                return NotFound("Request not found or you don't own it.");

            request.Title = updatedRequest.Title;
            request.Description = updatedRequest.Description;

            _context.Requests.Update(request);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Request updated successfully", request });
        }

        // 4️⃣ يمسح Request بتاعه
        [HttpDelete("DeleteRequest/{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var request = await _context.Requests.FirstOrDefaultAsync(r => r.Id == id && r.ClientId == user.Id);

            if (request == null)
                return NotFound("Request not found or you don't own it.");

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Request deleted successfully" });
        }

        // 5️⃣ يتابع الـ Applications اللي اتقدمت له (على الـ Requests بتاعته)
        [HttpGet("ApplicationsForMyRequests")]
        public async Task<IActionResult> GetApplicationsForMyRequests()
        {
            var user = await _userManager.GetUserAsync(User);

            var applications = await _context.Applications
                .Include(a => a.Provider)
                .Include(a => a.Request)
                .Where(a => a.Request.ClientId == user.Id)
                .ToListAsync();

            return Ok(applications);
        }

    }

}
