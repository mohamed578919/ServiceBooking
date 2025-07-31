using ApiDay1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceBooking.DTOs;
using ServiceBooking.Models;

[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly MyContext _context;

    public AdminController(MyContext context)
    {
        _context = context;
    }

    // GET: api/Admin
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AdminDto>>> GetAdmins()
    {
        return await _context.Admins
            .Select(a => new AdminDto
            {
                Id = a.Id,
                Username = a.Username,
                Email = a.Email
            }).ToListAsync();
    }

    // GET: api/Admin/5
    [HttpGet("{id}")]
    public async Task<ActionResult<AdminDto>> GetAdmin(int id)
    {
        var admin = await _context.Admins.FindAsync(id);

        if (admin == null)
            return NotFound();

        var dto = new AdminDto
        {
            Id = admin.Id,
            Username = admin.Username,
            Email = admin.Email
        };

        return dto;
    }

    // POST: api/Admin
    [HttpPost]
    public async Task<ActionResult<AdminDto>> PostAdmin(AdminDto dto)
    {
        var admin = new Admin
        {
            Username = dto.Username,
            Email = dto.Email
        };

        _context.Admins.Add(admin);
        await _context.SaveChangesAsync();

        dto.Id = admin.Id;

        return CreatedAtAction(nameof(GetAdmin), new { id = admin.Id }, dto);
    }

    // PUT: api/Admin/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAdmin(int id, AdminDto dto)
    {
        if (id != dto.Id)
            return BadRequest();

        var admin = await _context.Admins.FindAsync(id);
        if (admin == null)
            return NotFound();

        admin.Username = dto.Username;
        admin.Email = dto.Email;

        _context.Entry(admin).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Admin/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAdmin(int id)
    {
        var admin = await _context.Admins.FindAsync(id);
        if (admin == null)
            return NotFound();

        _context.Admins.Remove(admin);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
