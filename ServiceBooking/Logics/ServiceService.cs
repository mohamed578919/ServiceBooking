using ApiDay1.Models;
using Microsoft.EntityFrameworkCore;
using ServiceBooking.DTOs;
using ServiceBooking.Models;

namespace ServiceBooking.Logics
{
    public class ServiceService : IServiceService
    {
        private readonly MyContext _db;

        public ServiceService(MyContext db) => _db = db;

        public async Task<ServiceDto> CreateAsync(string providerId, ServiceCreateDto dto)
        {
            var exists = await _db.Services.AnyAsync(s => s.ProviderId == providerId && s.Name == dto.Name);
            if (exists) throw new InvalidOperationException("Service with same name already exists for this provider.");

            var entity = new Service
            {
                ProviderId = providerId,
                Name = dto.Name,
                Description = dto.Description,
                HourlyRate = dto.HourlyRate,
                ExperienceYears = dto.ExperienceYears,
                IsActive = true
            };

            _db.Services.Add(entity);
            await _db.SaveChangesAsync();

            return Map(entity);
        }

        public async Task<ServiceDto?> GetByIdAsync(int id)
        {
            var s = await _db.Services.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return s is null ? null : Map(s);
        }

        public async Task<IEnumerable<ServiceDto>> GetByProviderAsync(string providerId)
        {
            return await _db.Services.AsNoTracking()
                .Where(s => s.ProviderId == providerId)
                .Select(s => Map(s))
                .ToListAsync();
        }

        public async Task<ServiceDto> UpdateAsync(int id, string providerId, ServiceUpdateDto dto)
        {
            var s = await _db.Services.FirstOrDefaultAsync(x => x.Id == id && x.ProviderId == providerId)
                ?? throw new KeyNotFoundException("Service not found or not owned by provider.");

            // لا تسمح بتكرار الاسم لنفس البروفايدر
            var nameTaken = await _db.Services.AnyAsync(x => x.ProviderId == providerId && x.Name == dto.Name && x.Id != id);
            if (nameTaken) throw new InvalidOperationException("Another service with the same name exists.");

            s.Name = dto.Name;
            s.Description = dto.Description;
            s.HourlyRate = dto.HourlyRate;
            s.ExperienceYears = dto.ExperienceYears;
            s.IsActive = dto.IsActive;

            await _db.SaveChangesAsync();
            return Map(s);
        }

        public async Task DeleteAsync(int id, string providerId)
        {
            var s = await _db.Services.FirstOrDefaultAsync(x => x.Id == id && x.ProviderId == providerId)
                ?? throw new KeyNotFoundException("Service not found or not owned by provider.");

            _db.Services.Remove(s);
            await _db.SaveChangesAsync();
        }

        private static ServiceDto Map(Service s) => new()
        {
            Id = s.Id,
            ProviderId = s.ProviderId,
            Name = s.Name,
            Description = s.Description,
            HourlyRate = s.HourlyRate,
            ExperienceYears = s.ExperienceYears,
            IsActive = s.IsActive
        };
    }
}
