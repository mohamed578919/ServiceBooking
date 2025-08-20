using ApiDay1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceBooking.DTOs;
using ServiceBooking.Enums;
using ServiceBooking.Models;

[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly MyContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(MyContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    // get all statistics 

    // 📊 إحصائيات عامة
    [HttpGet("statistics")]
    public async Task<IActionResult> GetStatistics()
    {
        var clients = (await _userManager.GetUsersInRoleAsync("Client")).Count;
        var providers = (await _userManager.GetUsersInRoleAsync("Provider")).Count;
        var requests = await _context.Requests.CountAsync();
        var applications = await _context.Applications.CountAsync();
        var complaints = await _context.Complaints.CountAsync();

        return Ok(new
        {
            Clients = clients,
            Providers = providers,
            Requests = requests,
            Applications = applications,
            Complaints = complaints
        });
    }

    // ✅ مراجعة البروفايدرز
    [HttpGet("providers/pending")]
    public async Task<IActionResult> GetPendingProviders()
    {
        var pendingProviders = await _context.Users
            .Where(u => u.Role == "Provider" && u.Status == "Pending")
            .ToListAsync();

        return Ok(pendingProviders);
    }

    [HttpPost("providers/{providerId}/approve")]
    public async Task<IActionResult> ApproveProvider(string providerId)
    {
        var provider = await _context.Users.FindAsync(providerId);
        if (provider == null) return NotFound("Provider not found");

        provider.Status = "Approved";
        _context.Users.Update(provider);
        await _context.SaveChangesAsync();

        return Ok("Provider approved successfully.");
    }

    [HttpPost("providers/{providerId}/reject")]
    public async Task<IActionResult> RejectProvider(string providerId)
    {
        var provider = await _context.Users.FindAsync(providerId);
        if (provider == null) return NotFound("Provider not found");

        provider.Status = "Rejected";
        _context.Users.Update(provider);
        await _context.SaveChangesAsync();

        return Ok("Provider rejected.");
    }

    // 📌 مراجعة الطلبات
    [HttpGet("requests")]
    public async Task<IActionResult> GetAllRequests()
    {
        var requests = await _context.Requests
            .Include(r => r.Client)
            .ToListAsync();

        return Ok(requests);
    }

    [HttpDelete("requests/{requestId}")]
    public async Task<IActionResult> DeleteRequest(int requestId)
    {
        var request = await _context.Requests.FindAsync(requestId);
        if (request == null) return NotFound("Request not found");

        _context.Requests.Remove(request);
        await _context.SaveChangesAsync();

        return Ok("Request deleted successfully.");
    }

    // 🛠️ متابعة الشكاوي
    [HttpGet("complaints")]
    public async Task<IActionResult> GetComplaints()
    {
        var complaints = await _context.Complaints
            .Include(c => c.Client)
            .Include(c => c.Provider)
            .ToListAsync();

        return Ok(complaints);
    }

    [HttpPost("complaints/{complaintId}/update-status")]
    public async Task<IActionResult> UpdateComplaintStatus(int complaintId, [FromBody] ComplaintStatus status)
    {
        var complaint = await _context.Complaints.FindAsync(complaintId);
        if (complaint == null) return NotFound("Complaint not found");

        complaint.Status = status;
        _context.Complaints.Update(complaint);
        await _context.SaveChangesAsync();

        return Ok("Complaint status updated.");
    }

}
