using ApiDay1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceBooking.DTOs;
using ServiceBooking.Logics;
using ServiceBooking.Models;
using System.Security.Claims;

namespace ServiceBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationsController(MyContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }


     
     
        // Create Application
        [HttpPost("Create")]
        //[Authorize(Roles = "Provider")]
        public async Task<IActionResult> CreateApplication([FromBody] CreateApplicationDto dto)
        {

            var user = await _context.Users
           .FirstOrDefaultAsync(u => u.UserName == dto.ProviderUserName);

            if (user == null)
                return NotFound("User not found");

            var Provider = await _context.Providers
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (Provider == null)
                return NotFound("Client not found");

            var ProviderId = Provider.Id;

            

            var request = await _context.Requests.FindAsync(dto.ServiceRequestId);
            if (request == null) return NotFound("Request not found");

            var application = new Application
            {
                Message = dto.ProposalText,
                ProposedPrice = dto.ProposedPrice,
                RequestId = dto.ServiceRequestId,
                Notes = dto.Note,
                ProviderId = ProviderId,
                phoneNumber= dto.PhoneNumber
            };

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            return Ok(new { application.Id });
        }



        //==========================================================================================


        // Update Application (only by owner)
        [HttpPut("{id}")]
        //[Authorize(Roles = "Provider")]
        public async Task<IActionResult> UpdateApplication(int id, [FromBody] UpdateApplicationDto dto)
        {

            var user = await _context.Users
          .FirstOrDefaultAsync(u => u.UserName == dto.ProviderUserName);

            if (user == null)
                return NotFound("User not found");

            var Provider = await _context.Providers
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (Provider == null)
                return NotFound("Client not found");

            var ProviderId = Provider.Id;



            var app = await _context.Applications.FindAsync(id);

            if (app == null) return NotFound();
            if (app.ProviderId != ProviderId) return Forbid();

            app.Message = dto.ProposalText;
            app.ProposedPrice = dto.ProposedPrice;
            app.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok("Updated successfully");
        }


        //==========================================================================



        //// Delete Application (only by owner)
        //[HttpDelete("{id}")]
        ////[Authorize(Roles = "Provider")]
        //public async Task<IActionResult> DeleteApplication(int id)
        //{

        //    var user = await _context.Users
        //  .FirstOrDefaultAsync(u => u.UserName == dto.ProviderUserName);

        //    if (user == null)
        //        return NotFound("User not found");

        //    var Provider = await _context.Providers
        //        .FirstOrDefaultAsync(c => c.UserId == user.Id);

        //    if (Provider == null)
        //        return NotFound("Client not found");

        //    var ProviderId = Provider.Id;



        //    var app = await _context.Applications.FindAsync(id);

        //    if (app == null) return NotFound();
        //    if (app.ProviderId != ProviderId) return Forbid();

        //    _context.Applications.Remove(app);
        //    await _context.SaveChangesAsync();

        //    return Ok("Deleted successfully");
        //}


        //==============================================================================
        [HttpGet("GetMyApplications")]
        public async Task<IActionResult> GetMyApplications([FromQuery] string ProviderUserName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == ProviderUserName);

            if (user == null)
                return NotFound("User not found");

            var provider = await _context.Providers.FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (provider == null)
                return NotFound("Provider not found");

            var apps = await _context.Applications
                .Where(a => a.ProviderId == provider.Id)
                .Include(a => a.Request)
                .Select(a => new ApplicationDTO
                {
                    Id = a.Id,
                    Message = a.Message,
                    ProposedPrice = a.ProposedPrice,
                    CreatedAt = a.AppliedAt,
                    RequestTitle = a.Request.Title
                })
                .ToListAsync();

            return Ok(apps);
        }





        //=========================================================================


        //// Get Applications for a Request (client only)
        //[HttpGet("request/{requestId}")]
        //[Authorize(Roles = "Client")]
        //public async Task<IActionResult> GetApplicationsForRequest(int requestId)
        //{
        //    var clientId = GetUserId();

        //    var request = await _context.Requests
        //        .Include(r => r.Applications)
        //        .ThenInclude(a => a.Provider)
        //        .ThenInclude(c => c.User)
        //        .FirstOrDefaultAsync(r => r.Id == requestId);

        //    if (request == null) return NotFound();
        //    if (request.ClientId != clientId) return Forbid();

        //    var apps = request.Applications.Select(a => new
        //    {
        //        a.Id,
        //        a.Message,
        //        a.ProposedPrice,
        //        a.AppliedAt,
        //        ProviderName = a.Provider.User.Full_Name
        //    });

        //    return Ok(apps);
        //}

    }
}
