using ApiDay1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceBooking.DTOs;
using ServiceBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace ServiceBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderController : ControllerBase
    {
        private readonly MyContext _context;

        public ProviderController(MyContext context)
        {
            _context = context;
        }

        // GET: api/Provider
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProviderDto>>> GetProviders()
        {
            return await _context.Providers
                .Select(p => new ProviderDto { Id = p.Id, Name = p.FullName })
                .ToListAsync();
        }

        // GET: api/Provider/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProviderDto>> GetProvider(int id)
        {
            var provider = await _context.Providers.FindAsync(id);
            if (provider == null)
                return NotFound();

            return new ProviderDto { Id = provider.Id, Name = provider.FullName };
        }

        // POST: api/Provider
        [HttpPost]
        public async Task<ActionResult<ProviderDto>> PostProvider(ProviderDto dto)
        {
            var provider = new Provider { FullName = dto.Name };
            _context.Providers.Add(provider);
            await _context.SaveChangesAsync();

            dto.Id = provider.Id;
            return CreatedAtAction(nameof(GetProvider), new { id = provider.Id }, dto);
        }

        // PUT: api/Provider/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProvider(int id, ProviderDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var provider = await _context.Providers.FindAsync(id);
            if (provider == null)
                return NotFound();

            provider.FullName = dto.Name;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Provider/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProvider(int id)
        {
            var provider = await _context.Providers.FindAsync(id);
            if (provider == null)
                return NotFound();

            _context.Providers.Remove(provider);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
