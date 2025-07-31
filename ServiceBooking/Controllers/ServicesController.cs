using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiDay1.Models;
using ServiceBooking.DTOs;
using ServiceBooking.Models;

namespace ServiceBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly MyContext _context;

        public ServiceController(MyContext context)
        {
            _context = context;
        }

        // GET: api/Service
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceDto>>> GetAll()
        {
            var services = await _context.Services
                .Select(s => new ServiceDto
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToListAsync();

            return Ok(services);
        }

        // GET: api/Service/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceDto>> GetById(int id)
        {
            var service = await _context.Services
                .Where(s => s.Id == id)
                .Select(s => new ServiceDto
                {
                    Id = s.Id,
                    Name = s.Name
                }).FirstOrDefaultAsync();

            if (service == null)
                return NotFound();

            return Ok(service);
        }

        // POST: api/Service
        [HttpPost]
        public async Task<ActionResult<ServiceDto>> Create(ServiceDto dto)
        {
            var service = new Service
            {
                Name = dto.Name
            };

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            dto.Id = service.Id;

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        // PUT: api/Service/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ServiceDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var service = await _context.Services.FindAsync(id);
            if (service == null)
                return NotFound();

            service.Name = dto.Name;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Service/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
                return NotFound();

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
