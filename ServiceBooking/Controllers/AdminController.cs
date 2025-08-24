//using ApiDay1.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using ServiceBooking.DTOs;
//using ServiceBooking.Enums;
//using ServiceBooking.Logics;
//using ServiceBooking.Models;

//[Route("api/[controller]")]
//[ApiController]
//public class AdminController : ControllerBase
//{

//    private readonly ComplaintService _complaintService;
//    private readonly ICurrentUser _currentUser;

//    public AdminController(ComplaintService complaintService, ICurrentUser currentUser)
//    {
//        _complaintService = complaintService;
//        _currentUser = currentUser;
//    }

//    [HttpPost]
//    [Authorize] // لازم المستخدم يكون مسجل دخول
//    public async Task<IActionResult> CreateComplaint([FromBody] ComplaintCreateDto dto)
//    {
//        var complaint = await _complaintService.CreateComplaintAsync(dto);
//        return Ok(complaint);
//    }

//    [HttpGet("filed")]
//    [Authorize]
//    public IActionResult GetFiledComplaints()
//    {
//        var userId = _currentUser.GetUserId();
//        var complaints = _complaintService.GetComplaintsFiledByUser(userId).ToList();
//        return Ok(complaints);
//    }

//    [HttpGet("against")]
//    [Authorize]
//    public IActionResult GetComplaintsAgainst()
//    {
//        var userId = _currentUser.GetUserId();
//        var complaints = _complaintService.GetComplaintsAgainstUser(userId).ToList();
//        return Ok(complaints);
//    }

//}
