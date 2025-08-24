//using ApiDay1.Models;
//using Microsoft.EntityFrameworkCore;
//using ServiceBooking.DTOs;
//using ServiceBooking.Logics;
//using ServiceBooking.Models;

//public class ServiceService 
//{
//    private readonly MyContext _db;

//    public ServiceService(MyContext db) => _db = db;

//    public async Task<ServiceDto> CreateAsync(int providerId, ServiceCreateDto dto)
//    {
//        // لو السيرفس موجودة بنفس الاسم
//        var service = await _db.Services.FirstOrDefaultAsync(s => s.Name == dto.Name);
//        if (service == null)
//        {
//            service = new Service
//            {
//                Name = dto.Name,
//                Description = dto.Description,
//                HourlyRate = dto.HourlyRate,
//                ExperienceYears = dto.ExperienceYears,
//                IsActive = true
//            };
//            _db.Services.Add(service);
//            await _db.SaveChangesAsync();
//        }

//        // اربط البروفايدر بالسيرفس (لو مش مربوط قبل كده)
//        var alreadyLinked = await _db.ProviderSkills
//            .AnyAsync(ps => ps.ProviderId == providerId && ps.ServiceCategoryId == service.Id);

//        if (!alreadyLinked)
//        {
//            _db.ProviderSkills.Add(new ProviderSkill
//            {
//                ProviderId = providerId,
//                ServiceCategoryId = service.Id
//            });
//            await _db.SaveChangesAsync();
//        }

//        return Map(service);
//    }

//    public async Task<IEnumerable<ServiceDto>> GetByProviderAsync(int providerId)
//    {
//        return await _db.ProviderSkills
//            .Where(ps => ps.ProviderId == providerId)
//            .Select(ps => Map(ps.ServiceCategory))
//            .ToListAsync();
//    }

//    public async Task<ServiceDto?> GetByIdAsync(int id)
//    {
//        var s = await _db.Services.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
//        return s is null ? null : Map(s);
//    }

//    public async Task<ServiceDto> UpdateAsync(int id, ServiceUpdateDto dto)
//    {
//        var s = await _db.Services.FirstOrDefaultAsync(x => x.Id == id)
//            ?? throw new KeyNotFoundException("Service not found.");

//        s.Name = dto.Name;
//        s.Description = dto.Description;
//        s.HourlyRate = dto.HourlyRate;
//        s.ExperienceYears = dto.ExperienceYears;
//        s.IsActive = dto.IsActive;

//        await _db.SaveChangesAsync();
//        return Map(s);
//    }

//    public async Task DeleteAsync(int serviceId, int providerId)
//    {
//        var link = await _db.ProviderSkills
//            .FirstOrDefaultAsync(ps => ps.ServiceCategoryId == serviceId && ps.ProviderId == providerId);

//        if (link == null)
//            throw new KeyNotFoundException("Service not linked to this provider.");

//        _db.ProviderSkills.Remove(link);
//        await _db.SaveChangesAsync();
//    }

//    private static ServiceDto Map(Service s)
//    {
//        return new()
//        {
//            Id = s.Id,
//            Name = s.Name,
//            Description = s.Description,
//            HourlyRate = s.HourlyRate,
//            ExperienceYears = s.ExperienceYears,
//            IsActive = s.IsActive
//        };
//    }
//}
