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
    public class ClientController : ControllerBase
    {
        private readonly MyContext _context;

        public ClientController(MyContext context)
        {
            _context = context;
        }

        // GET: api/Client
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetClients()
        {
            return await _context.Clients
                .Select(c => new ClientDto { Id = c.Id, Name = c.FullName })
                .ToListAsync();
        }

        // GET: api/Client/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDto>> GetClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
                return NotFound();

            return new ClientDto { Id = client.Id, Name = client.FullName };
        }

        // POST: api/Client
        [HttpPost]
        public async Task<ActionResult<ClientDto>> PostClient(ClientDto dto)
        {
            var client = new Client { FullName = dto.Name };
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            dto.Id = client.Id;
            return CreatedAtAction(nameof(GetClient), new { id = client.Id }, dto);
        }

        // PUT: api/Client/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, ClientDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
                return NotFound();

            client.FullName = dto.Name;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Client/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
                return NotFound();

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
