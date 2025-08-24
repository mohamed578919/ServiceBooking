using ApiDay1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

    //[Authorize(Roles = "Client")]
    [HttpPost("Create")]
    public async Task<IActionResult> CreateRequest(CreateServiceRequestDto dto)
    {
        //var client = await _context.Clients
        //    .Include(c => c.User)
        //    .FirstOrDefaultAsync(c => c.User.UserName == dto.ClientUserNamed);

        var user = await _context.Users
        .FirstOrDefaultAsync(u => u.UserName == dto.ClientUserNamed);

        if (user == null)
            return NotFound("User not found");

        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.UserId == user.Id);

        if (client == null)
            return NotFound("Client not found");

        var clientId = client.Id;


        var Service = await _context.Services
            .FirstOrDefaultAsync(s => s.Name == dto.Service);
        if (Service == null)
            return NotFound("Client not found");
        var serviceId = Service.Id;


        //if (client == null)
        //    return NotFound("Client not found");

        //var _clientId = client.Id; // ده الـ int ID بتاع Client

        var request = new Request
        {
            Title = dto.Title,
            Description = dto.Description,
            Category = dto.Service,
            ServiceId = serviceId,
            Budget = dto.Budget,
            ClientId = clientId
        };

        _context.Requests.Add(request);
        await _context.SaveChangesAsync();

        var resultDto = new ServiceRequestDto
        {
            Id = request.Id,
            Title = request.Title,
            Description = request.Description,
            Category = request.Category,
            Budget = request.Budget,
            CreatedAt = request.CreatedAt,
            ClientName = User.Identity.Name
        };

        return Ok(resultDto);
    }



    //========================================================================

    [HttpGet("search")]
    public async Task<IActionResult> SearchRequests([FromQuery] string keyword)
    {
        var query = _context.Requests.AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(r => r.Title.Contains(keyword) ||
                                     r.Description.Contains(keyword) ||
                                     r.Category.Contains(keyword));
        }

        var results = await query
            .Include(r => r.Client)
             .ThenInclude(c => c.User)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new ServiceRequestDto
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                Category = r.Category,
                Budget = r.Budget,
                CreatedAt = r.CreatedAt,
                ClientName = r.Client.User.Full_Name
            })
            .ToListAsync();

        return Ok(results);
    }



    //===================================================

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetRequestDetails(int id)
    {
        var request = await _context.Requests
            .Include(r => r.Client)
             .ThenInclude(c => c.User)
            .Include(r => r.Applications)
                .ThenInclude(a => a.Provider)
                .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (request == null) return NotFound();

        var dto = new ServiceRequestDetailsDto
        {
            Id = request.Id,
            Title = request.Title,
            Description = request.Description,
            Category = request.Category,
            Budget = request.Budget,
            CreatedAt = request.CreatedAt,
            ClientName = request.Client.User.Full_Name,
            Applications = request.Applications.Select(a => new ApplicationDTO
            {
                ProviderName = a.Provider.User.Full_Name,
                Message = a.Message,
                PhoneNumber = a.phoneNumber


            }).ToList()
        };

        return Ok(dto);
    }


    //===================================================================================

    [HttpPut("{id:int}")]
    //[Authorize(Roles = "Client")]
    public async Task<IActionResult> UpdateRequest(int id, [FromBody] UpdateServiceRequestDto dto)
    {
        var client = await _context.Clients
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.User.UserName == dto.ClientUserNamed);

        if (client == null)
            return NotFound("Client not found");

        var clientId = client.Id; // ده الـ int ID بتاع Client

        var request = await _context.Requests.FirstOrDefaultAsync(r => r.Id == id);

        if (request == null)
            return NotFound("Request not found");

        if (request.ClientId != clientId)
            return Forbid("You can only edit your own requests");

        request.Title = dto.Title;
        request.Description = dto.Description;
        request.Budget = dto.Budget;


        _context.Requests.Update(request);
        await _context.SaveChangesAsync();

        return Ok();
    }


    //===============================================================
    [HttpGet("{ClientUserNamed}")]
    public async Task<IActionResult> GetMyRequests(string ClientUserNamed)
    {
        var client = await _context.Clients
             .Include(c => c.User)
             .FirstOrDefaultAsync(c => c.User.UserName == ClientUserNamed);

        if (client == null)
            return NotFound("Client not found");

        var clientId = client.Id; // ده الـ int ID بتاع Client

       

        

        // نجيب الريكويستات الخاصة بالعميل ده
        var myRequests = await _context.Requests
            .Where(r => r.ClientId == client.Id)
            .Include(r => r.Client)
                .ThenInclude(c => c.User)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new ServiceRequestDto
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                Category = r.Category,
                Budget = r.Budget,
                CreatedAt = r.CreatedAt,
                ClientName = r.Client.User.Full_Name
            })
            .ToListAsync();

        return Ok(myRequests);
    }

    [HttpGet("Applications/{clientUserName}")]
    public async Task<IActionResult> GetClientApplications(string clientUserName)
    {
        // نجيب العميل من الـ username
        var client = await _context.Clients
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.User.UserName == clientUserName);

        if (client == null)
            return NotFound("Client not found");

        // نجيب كل الـ applications اللي جاية على الريكويستات بتاعته
        var applications = await _context.Applications
            .Include(a => a.Request)
                .ThenInclude(r => r.Client)
            .Include(a => a.Provider)
                .ThenInclude(p => p.User)
            .Where(a => a.Request.ClientId == client.Id)
            .Select(a => new ApplicationDTO
            {
                ProviderName = a.Provider.User.Full_Name,
                Message = a.Message,
                PhoneNumber = a.phoneNumber,
                // ممكن نضيف بيانات عن الـ Request نفسه
                RequestTitle = a.Request.Title,
                RequestId = a.Request.Id
            })
            .ToListAsync();

        return Ok(applications);
    }

    //===================================================================

    [HttpGet("All")]
    //[Authorize] // لو عايز أي يوزر  
    public async Task<IActionResult> GetAllRequests()
    {
        var requests = await _context.Requests
            .Include(r => r.Client)
                .ThenInclude(c => c.User)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new ServiceRequestDto
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                Category = r.Category,
                Budget = r.Budget,
                CreatedAt = r.CreatedAt,
                ClientName = r.Client.User.Full_Name
            })
            .ToListAsync();

        return Ok(requests);
    }


}
