using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceBooking.Models;
using Microsoft.EntityFrameworkCore;
using ApiDay1.Models;
using Microsoft.AspNetCore.Authorization;
using ServiceBooking.DTOs;
using ServiceBooking.Logics;
using System.Security.Claims;

namespace ServiceBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintController : ControllerBase
    {
        private readonly IComplaintService _service;
        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public ComplaintController(IComplaintService service) => _service = service;

        [HttpPost]
        [Authorize] // Client أو Provider
        public async Task<ActionResult<ComplaintDto>> Create(ComplaintCreateDto dto)
        {
            var result = await _service.CreateAsync(UserId, dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id:int}")]
        [Authorize] // يسمح فقط للأطراف المعنيين أو Admin
        public async Task<ActionResult<ComplaintDto>> GetById(int id)
        {
            var isAdmin = User.IsInRole("Admin");
            try
            {
                var c = await _service.GetByIdAsync(id, UserId, isAdmin);
                return c is null ? NotFound() : Ok(c);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpGet("mine")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ComplaintDto>>> GetMine()
        {
            var list = await _service.GetMineAsync(UserId);
            return Ok(list);
        }

        [HttpGet("against-me")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ComplaintDto>>> GetAgainstMe()
        {
            var list = await _service.GetAgainstMeAsync(UserId);
            return Ok(list);
        }

        [HttpPatch("{id:int}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ComplaintDto>> UpdateStatus(int id, ComplaintUpdateStatusDto dto)
        {
            var result = await _service.UpdateStatusAsync(id, dto);
            return Ok(result);
        }
    }
}