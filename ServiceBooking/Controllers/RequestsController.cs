using System.Security.Claims;
using ApiDay1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceBooking.DTOs;
using ServiceBooking.Models;

[Route("api/[controller]")]
[ApiController]
public class RequestController : ControllerBase
{
    private readonly MyContext _context;
    public RequestController(MyContext context) => _context = context;

    // POST: api/Requests
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RequestCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(clientId)) return Unauthorized();

        var request = new Request
        {
            Title = dto.Title,
            Description = dto.Description,
            ServiceId = dto.ServiceId,
            Notes = dto.Notes,
            RequestDate = DateTime.UtcNow,
            Status = "Pending",
            ClientId = clientId
        };

        _context.Requests.Add(request);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = request.Id }, request);
    }

    // GET: api/Requests/my-requests
    [HttpGet("my-requests")]
    public async Task<IActionResult> MyRequests()
    {
        var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(clientId)) return Unauthorized();

        var my = await _context.Requests
            .Include(r => r.Service)
            .Where(r => r.ClientId == clientId)
            .OrderByDescending(r => r.Id)
            .ToListAsync();

        return Ok(my);
    }

    // GET: api/Requests/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(clientId)) return Unauthorized();

        var r = await _context.Requests
            .Include(x => x.Service)
            .FirstOrDefaultAsync(x => x.Id == id && x.ClientId == clientId);

        return r is null ? NotFound() : Ok(r);
    }

    // PUT: api/Requests/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] RequestUpdateDto dto)
    {
        var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(clientId)) return Unauthorized();

        var r = await _context.Requests.FirstOrDefaultAsync(x => x.Id == id && x.ClientId == clientId);
        if (r is null) return NotFound();

        r.Title = dto.Title;
        r.Description = dto.Description;
        r.ServiceId = dto.ServiceId;
        r.Notes = dto.Notes;
        r.RequestDate = dto.PreferredDate ?? r.RequestDate;
        if (!string.IsNullOrWhiteSpace(dto.Status)) r.Status = dto.Status;

        await _context.SaveChangesAsync();
        return Ok(r);
    }

    // DELETE: api/Requests/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(clientId)) return Unauthorized();

        var r = await _context.Requests.FirstOrDefaultAsync(x => x.Id == id && x.ClientId == clientId);
        if (r is null) return NotFound();

        _context.Requests.Remove(r);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Deleted" });
    }

    // GET: api/Requests/5/applications  (يتابع التقديمات على الطلب بتاعه)
    [HttpGet("{id:int}/applications")]
    public async Task<IActionResult> GetApplications(int id)
    {
        var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(clientId)) return Unauthorized();

        var exists = await _context.Requests.AnyAsync(r => r.Id == id && r.ClientId == clientId);
        if (!exists) return NotFound();

        var apps = await _context.Applications
            .Include(a => a.Provider)
            .Where(a => a.RequestId == id)
            .OrderByDescending(a => a.AppliedAt)
            .ToListAsync();

        return Ok(apps);
    }
}
