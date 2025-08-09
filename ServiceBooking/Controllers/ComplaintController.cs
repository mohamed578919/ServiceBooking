using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceBooking.Models;
using Microsoft.EntityFrameworkCore;
using ApiDay1.Models;

namespace ServiceBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintController : ControllerBase
    {
        private readonly MyContext _context;

        public ComplaintController(MyContext context)
        {
            _context = context;
        }

        // POST: api/Complaints
        [HttpPost]
        public async Task<ActionResult<Complaint>> CreateComplaint([FromBody] Complaint complaint)
        {
            if (string.IsNullOrEmpty(complaint.Text))
                return BadRequest("النص مطلوب");

            _context.Complaints.Add(complaint);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComplaint), new { id = complaint.Id }, complaint);
        }

        // GET: api/Complaints/5 (اختياري، لو محتاج تشوف الشكاوى لاحقًا)
        [HttpGet("{id}")]
        public async Task<ActionResult<Complaint>> GetComplaint(int id)
        {
            var complaint = await _context.Complaints.FindAsync(id);
            if (complaint == null)
                return NotFound();

            return complaint;
        }
    }
}