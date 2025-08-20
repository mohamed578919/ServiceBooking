using ApiDay1.Models;
using Microsoft.EntityFrameworkCore;
using ServiceBooking.DTOs;
using ServiceBooking.Enums;
using ServiceBooking.Models;

namespace ServiceBooking.Logics
{
    public class ComplaintService : IComplaintService
    {
        private readonly MyContext _db;

        public ComplaintService(MyContext db) => _db = db;

        public async Task<ComplaintDto> CreateAsync(string filedByUserId, ComplaintCreateDto dto)
        {
            if (filedByUserId == dto.AgainstUserId)
                throw new InvalidOperationException("You cannot file a complaint against yourself.");

            var complaint = new Complaint
            {
                Title = dto.Title,
                Description = dto.Description,
                Severity = dto.Severity,
                FiledByUserId = filedByUserId,
                AgainstUserId = dto.AgainstUserId,
                RelatedRequestId = dto.RelatedRequestId,
                RelatedServiceId = dto.RelatedServiceId,
                Status = ComplaintStatus.Open,
                CreatedAtUtc = DateTime.UtcNow
            };

            _db.Complaints.Add(complaint);
            await _db.SaveChangesAsync();

            return Map(complaint);
        }

        public async Task<ComplaintDto?> GetByIdAsync(int id, string currentUserId, bool isAdmin)
        {
            var c = await _db.Complaints.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (c is null) return null;

            if (!isAdmin && c.FiledByUserId != currentUserId && c.AgainstUserId != currentUserId)
                throw new UnauthorizedAccessException("Not allowed to view this complaint.");

            return Map(c);
        }

        public async Task<IEnumerable<ComplaintDto>> GetMineAsync(string currentUserId)
        {
            return await _db.Complaints.AsNoTracking()
                .Where(c => c.FiledByUserId == currentUserId)
                .OrderByDescending(c => c.CreatedAtUtc)
                .Select(c => Map(c)).ToListAsync();
        }

        public async Task<IEnumerable<ComplaintDto>> GetAgainstMeAsync(string currentUserId)
        {
            return await _db.Complaints.AsNoTracking()
                .Where(c => c.AgainstUserId == currentUserId)
                .OrderByDescending(c => c.CreatedAtUtc)
                .Select(c => Map(c)).ToListAsync();
        }

        public async Task<ComplaintDto> UpdateStatusAsync(int id, ComplaintUpdateStatusDto dto)
        {
            var c = await _db.Complaints.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new KeyNotFoundException("Complaint not found.");

            c.Status = dto.Status;
            c.AdminNotes = dto.AdminNotes;
            c.UpdatedAtUtc = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return Map(c);
        }

        private static ComplaintDto Map(Complaint c) => new()
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            Severity = c.Severity,
            Status = c.Status,
            CreatedAtUtc = c.CreatedAtUtc,
            UpdatedAtUtc = c.UpdatedAtUtc,
            FiledByUserId = c.FiledByUserId,
            AgainstUserId = c.AgainstUserId,
            RelatedRequestId = c.RelatedRequestId,
            RelatedServiceId = c.RelatedServiceId,
            AdminNotes = c.AdminNotes
        };
    }
}
