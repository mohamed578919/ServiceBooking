using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceBooking.Models;
using Microsoft.EntityFrameworkCore;
using ApiDay1.Models;
using Microsoft.AspNetCore.Authorization;
using ServiceBooking.DTOs;
using ServiceBooking.Logics;
using System.Security.Claims;
using ServiceBooking.Enums;

namespace ServiceBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintController : ControllerBase
    {
        private readonly MyContext _context;
        public ComplaintController(MyContext context) => _context = context;

        // POST: api/Complaints
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ComplaintCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var against = await _context.Users.FindAsync(dto.AgainstUserId);
            if (against is null) return BadRequest("AgainstUser not found");

            var complaint = new Complaint
            {
                Title = dto.Title,
                Description = dto.Description,
                FiledByUserId = userId,
                AgainstUserId = dto.AgainstUserId,
                RelatedRequestId = dto.RelatedRequestId,
                RelatedServiceId = dto.RelatedServiceId,
                CreatedAtUtc = DateTime.UtcNow,
                Status = ComplaintStatus.Open
            };

            _context.Complaints.Add(complaint);
            await _context.SaveChangesAsync();
            return Ok(complaint);
        }

        // GET: api/Complaints/my
        [HttpGet("my")]
        public async Task<IActionResult> MyComplaints()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var list = await _context.Complaints
                .Where(c => c.FiledByUserId == userId)
                .OrderByDescending(c => c.CreatedAtUtc)
                .ToListAsync();

            return Ok(list);
        }
    }
}