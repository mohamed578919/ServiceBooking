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


        private readonly MyContext _context;

        public ServiceController(MyContext context)
        {
            _context = context;
        }


        [HttpGet("GetServiceNames")]
        public async Task<IActionResult> GetServiceNames()
        {
            var serviceNames = await _context.Services
                .Select(s => s.Name)
                .ToListAsync();

            return Ok(serviceNames);
        }



    }
}
