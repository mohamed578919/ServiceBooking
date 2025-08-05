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

    public RequestController(MyContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RequestDto>>> GetRequests()
    {
        var requests = await _context.Requests
            .Include(r => r.Client)
            .Include(r => r.Service)
            .Select(r => new RequestDto
            {
                Id = r.Id,
                Notes = r.Notes,
                RequestDate = r.RequestDate,
                ClientId = r.ClientId,
                ClientName = r.Client != null ? r.Client.FullName : "",
                ServiceId = r.ServiceId,
                ServiceName = r.Service != null ? r.Service.Name : ""
            }).ToListAsync();

        return Ok(requests);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RequestDto>> GetRequest(int id)
    {
        var r = await _context.Requests
            .Include(r => r.Client)
            .Include(r => r.Service)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (r == null)
            return NotFound();

        var dto = new RequestDto
        {
            Id = r.Id,
            Notes = r.Notes,
            RequestDate = r.RequestDate,
            ClientId = r.ClientId,
            ClientName = r.Client?.FullName ?? "",
            ServiceId = r.ServiceId,
            ServiceName = r.Service?.Name ?? ""
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult> CreateRequest(RequestDto dto)
    {
        var request = new Request
        {
            Notes = dto.Notes,
            RequestDate = dto.RequestDate,
            ClientId = dto.ClientId,
            ServiceId = dto.ServiceId
        };

        _context.Requests.Add(request);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRequest), new { id = request.Id }, dto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateRequest(int id, RequestDto dto)
    {
        var request = await _context.Requests.FindAsync(id);
        if (request == null)
            return NotFound();

        request.Notes = dto.Notes;
        request.RequestDate = dto.RequestDate;
        request.ClientId = dto.ClientId;
        request.ServiceId = dto.ServiceId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteRequest(int id)
    {
        var request = await _context.Requests.FindAsync(id);
        if (request == null)
            return NotFound();

        _context.Requests.Remove(request);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
