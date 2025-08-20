using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiDay1.Models;
using ServiceBooking.DTOs;
using ServiceBooking.Models;
using Microsoft.AspNetCore.Authorization;
using ServiceBooking.Logics;
using System.Security.Claims;

namespace ServiceBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _service;

        public ServiceController(IServiceService service) => _service = service;

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpPost]
        [Authorize(Roles = "Provider")]
        public async Task<ActionResult<ServiceDto>> Create(ServiceCreateDto dto)
        {
            var result = await _service.CreateAsync(UserId, dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<ServiceDto>> GetById(int id)
        {
            var s = await _service.GetByIdAsync(id);
            return s is null ? NotFound() : Ok(s);
        }

        [HttpGet("mine")]
        [Authorize(Roles = "Provider")]
        public async Task<ActionResult<IEnumerable<ServiceDto>>> GetMine()
        {
            var list = await _service.GetByProviderAsync(UserId);
            return Ok(list);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Provider")]
        public async Task<ActionResult<ServiceDto>> Update(int id, ServiceUpdateDto dto)
        {
            var s = await _service.UpdateAsync(id, UserId, dto);
            return Ok(s);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id, UserId);
            return NoContent();
        }
    }
}
