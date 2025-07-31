using ApiDay1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceBooking.DTOs;
using ServiceBooking.Models;

namespace ServiceBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ComplaintController : ControllerBase
    {
        public MyContext context;
        public ComplaintController(MyContext _context)
        {
            context = _context;
        }

        [HttpPost]
        [AllowAnonymous]
        
        public async Task<IActionResult> CreateComplaint([FromBody] CreateComplaintDTO dto)
        {
            if (ModelState.IsValid)
            {
                var complaint = new Complaint
                {
                    Description = dto.Description,
                    ClientId = dto.ClientId,
                    ProviderId = dto.ProviderId,
                    RequestId = dto.RequestId,
                    CreatedAt = DateTime.Now,
                    Status = "New"
                };
                context.Complaints.Add(complaint);
                await context.SaveChangesAsync();

                return Ok(complaint);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<Complaint>>> GetAllComplaints()
        {
            var complaints = await context.Complaints
                .Include(c => c.Client)
                .Include(c => c.Provider)
                .Include(c => c.Request)
                .ToListAsync();

            return Ok(complaints);
        }

        [HttpGet("client/{clientId}")]
        [AllowAnonymous] 
        public async Task<ActionResult<IEnumerable<Complaint>>> GetComplaintsByClient(int clientId)
        {
            var complaints = await context.Complaints
                .Where(c => c.ClientId == clientId)
                .Include(c => c.Provider)
                .Include(c => c.Request)
                .ToListAsync();

            return Ok(complaints);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComplaintStatus(int id, [FromBody] UpdateComplaintStatusDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var complaint = await context.Complaints.FindAsync(id);

            if (complaint == null)
                return NotFound();

            complaint.Status = dto.Status;

            context.Entry(complaint).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return Ok(complaint);
        }
    }
}