//using ApiDay1.Models;
//using Microsoft.EntityFrameworkCore;
//using ServiceBooking.DTOs;
//using ServiceBooking.Models;

//namespace ServiceBooking.Logics
//{
//    public class ApplicationService
//    {
//        private readonly MyContext _db;
//        private readonly ICurrentUser _me;

//        public ApplicationService(MyContext db, ICurrentUser me)
//        {
//            _db = db; _me = me;
//        }

//        public async Task<int> ApplyAsync(int requestId, CreateApplicationDto dto)
//        {
//            var provider = await _db.Providers.SingleAsync(p => p.UserId == _me.UserId);

//            // prevent duplicate application
//            var exists = await _db.ProviderApplications
//                .AnyAsync(a => a.RequestId == requestId && a.ProviderId == provider.Id);
//            if (exists) return 0;

//            var app = new ProviderApplication
//            {
//                RequestId = requestId,
//                ProviderId = provider.Id,
//                CoverLetter = dto.CoverLetter,
//                BidAmount = dto.BidAmount
//            };
//            _db.ProviderApplications.Add(app);
//            await _db.SaveChangesAsync();
//            return app.Id;
//        }

//        public async Task<List<ApplicationItemDto>> GetMyApplicationsAsync()
//        {
//            var provider = await _db.Providers.SingleAsync(p => p.UserId == _me.UserId);

//            return await _db.ProviderApplications
//                .Where(a => a.ProviderId == provider.Id)
//                .OrderByDescending(a => a.CreatedAt)
//                .Select(a => new ApplicationItemDto
//                {
//                    Id = a.Id,
//                    RequestId = a.RequestId,
//                    ProviderName = a.ProviderProfile.DisplayName,
//                    BidAmount = a.BidAmount,
//                    Status = a.Status,
//                    CreatedAt = a.CreatedAt
//                }).ToListAsync();
//        }

//        public async Task<List<ApplicationItemDto>> GetApplicationsForMyRequestAsync(int requestId)
//        {
//            var client = await _db.Clients.SingleAsync(c => c.UserId == _me.UserId);

//            return await _db.ProviderApplications
//                .Where(a => a.RequestId == requestId && a.Request.ClientProfileId == client.Id)
//                .OrderByDescending(a => a.CreatedAt)
//                .Select(a => new ApplicationItemDto
//                {
//                    Id = a.Id,
//                    RequestId = a.RequestId,
//                    ProviderName = a.ProviderProfile.DisplayName,
//                    BidAmount = a.BidAmount,
//                    Status = a.Status,
//                    CreatedAt = a.CreatedAt
//                }).ToListAsync();
//        }
//    }
//}
