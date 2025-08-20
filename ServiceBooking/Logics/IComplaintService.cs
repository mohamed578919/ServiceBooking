using ServiceBooking.DTOs;

namespace ServiceBooking.Logics
{
    public interface IComplaintService
    {
        Task<ComplaintDto> CreateAsync(string filedByUserId, ComplaintCreateDto dto);
        Task<ComplaintDto?> GetByIdAsync(int id, string currentUserId, bool isAdmin);
        Task<IEnumerable<ComplaintDto>> GetMineAsync(string currentUserId);
        Task<IEnumerable<ComplaintDto>> GetAgainstMeAsync(string currentUserId);
        Task<ComplaintDto> UpdateStatusAsync(int id, ComplaintUpdateStatusDto dto); // Admin only
    }
}
