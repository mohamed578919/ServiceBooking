using ApiDay1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceBooking.DTOs;
using ServiceBooking.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ServiceBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Client")]
    public class ClientController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClientController(MyContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        //=====================================================
        // ✅ تجيب كل الكلاينتات
        [HttpGet("GetAllClients")]
        //[Authorize] // ممكن تخليها Admin بس
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await _context.Clients
                .Include(c => c.User)
                .Include(c => c.Requests)
                .ToListAsync();

            var result = clients.Select(c => new
            {
                c.Id,
                c.UserId,
                FullName = c.User.Full_Name,
                Email = c.User.Email,
                Phone = c.User.PhoneNumber,
                Role = c.User.Role,
                Status = c.User.Status,
                RequestsCount = c.Requests.Count
            });

            return Ok(result);
        }

        //=====================================================
        //  تجيب الكلاينت الحالي
        [HttpGet("me")]
        //[Authorize(Roles = "Client")]
        public async Task<IActionResult> GetCurrentClient([FromQuery] string ClientUserNamed)
        {
            var user = await _context.Users
        .FirstOrDefaultAsync(u => u.UserName == ClientUserNamed);

            if (user == null)
                return NotFound("User not found");

            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (client == null)
                return NotFound("Client not found");

            var clientId = client.Id;

            var _client = await _context.Clients
                .Include(c => c.User)
                .Include(c => c.Requests)
                .FirstOrDefaultAsync(c => c.Id == clientId);

            if (client == null)
                return NotFound("Client profile not found");

            var result = new
            {
                client.Id,
                client.UserId,
                FullName = client.User.Full_Name,
                Email = client.User.Email,
                Phone = client.User.PhoneNumber,
                Role = client.User.Role,
                Status = client.User.Status,
                Requests = client.Requests.Select(r => new
                {
                    r.Id,
                    r.Title,
                    r.Category,
                    r.Budget,
                    r.CreatedAt
                })
            };

            return Ok(result);
        }





    }

}
