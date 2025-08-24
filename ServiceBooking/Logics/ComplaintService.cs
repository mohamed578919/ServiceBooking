//using ApiDay1.Models;
//using ServiceBooking.DTOs;
//using ServiceBooking.Models;

//namespace ServiceBooking.Logics
//{
//    public class ComplaintService
//    {
//        private readonly MyContext _db;
//        private readonly ICurrentUser _currentUser;

//        public ComplaintService(MyContext db, ICurrentUser currentUser)
//        {
//            _db = db;
//            _currentUser = currentUser;
//        }

//        public async Task<Complaint> CreateComplaintAsync(ComplaintCreateDto dto)
//        {
//            var complaint = new Complaint
//            {
//                ComplainantId = _currentUser.GetUserId(),
//                AgainstUserId = dto.AgainstUserId,
//                Title = dto.Title,
//                Description = dto.Description,
//                Severity = dto.Severity
//            };

//            _db.Complaints.Add(complaint);
//            await _db.SaveChangesAsync();

//            return complaint;
//        }

//        public IQueryable<Complaint> GetComplaintsAgainstUser(string userId)
//        {
//            return _db.Complaints.Where(c => c.AgainstUserId == userId);
//        }

//        public IQueryable<Complaint> GetComplaintsFiledByUser(string userId)
//        {
//            return _db.Complaints.Where(c => c.ComplainantId == userId);
//        }
//    }
//}
