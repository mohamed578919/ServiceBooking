//using ApiDay1.Models;
//using Microsoft.EntityFrameworkCore;
//using ServiceBooking.DTOs;
//using ServiceBooking.Enums;
//using ServiceBooking.Models;

//namespace ServiceBooking.Logics
//{
//    public class RequestService
//    {
//        private readonly MyContext _db;
//        private readonly ICurrentUser _me;

//        public RequestService(MyContext db, ICurrentUser me)
//        {
//            _db = db; _me = me;
//        }

//        public async Task<int> CreateAsync(CreateRequestDto dto)
//        {
//            var client = await _db.Providers.SingleAsync(c => c.UserId == _me.UserId);

//            var req = new ServiceRequest
//            {
//                ClientProfileId = client.Id,
//                Title = dto.Title,
//                Description = dto.Description,
//                CategoryId = dto.CategoryId,
//                Status = RequestStatus.Open
//            };
//            _db.ServiceRequests.Add(req);
//            await _db.SaveChangesAsync();
//            return req.Id;
//        }

//        public async Task<List<RequestListItemDto>> GetMyRequestsAsync()
//        {
//            var client = await _db.Clients.SingleAsync(c => c.UserId == _me.UserId);

//            return await _db.ServiceRequests
//                .Where(r => r.ClientProfileId == client.Id)
//                .Select(r => new RequestListItemDto
//                {
//                    Id = r.Id,
//                    Title = r.Title,
//                    Category = r.Category.Name,
//                    Status = r.Status,
//                    CreatedAt = r.CreatedAt,
//                    AssignedProvider = r.AssignedProvider != null ? r.AssignedProvider.DisplayName : null,
//                    ApplicationsCount = r.Applications.Count
//                }).OrderByDescending(x => x.CreatedAt).ToListAsync();
//        }

//        public async Task<List<RequestListItemDto>> SearchOpenRequestsForProviderAsync(int? categoryId, string? keyword, int page, int pageSize)
//        {
//            var provider = await _db.Providers
//                .Include(p => p.Skills)
//                .SingleAsync(p => p.UserId == _me.UserId);

//            var providerCategoryIds = provider.Skills.Select(s => s.ServiceCategoryId).ToHashSet();

//            var q = _db.ServiceRequests
//                .AsNoTracking()
//                .Include(r => r.Category)
//                .Include(r => r.Applications)
//                .Where(r => r.Status == RequestStatus.Open);

//            // must match provider skills unless provider is searching a specific category he owns
//            q = q.Where(r => providerCategoryIds.Contains(r.CategoryId));

//            if (categoryId.HasValue)
//                q = q.Where(r => r.CategoryId == categoryId.Value);

//            if (!string.IsNullOrWhiteSpace(keyword))
//                q = q.Where(r => r.Title.Contains(keyword) || r.Description.Contains(keyword));

//            return await q.OrderByDescending(r => r.CreatedAt)
//                .Skip((page - 1) * pageSize).Take(pageSize)
//                .Select(r => new RequestListItemDto
//                {
//                    Id = r.Id,
//                    Title = r.Title,
//                    Category = r.Category.Name,
//                    Status = r.Status,
//                    CreatedAt = r.CreatedAt,
//                    AssignedProvider = r.AssignedProvider != null ? r.AssignedProvider.DisplayName : null,
//                    ApplicationsCount = r.Applications.Count
//                }).ToListAsync();
//        }

//        public async Task<bool> AssignProviderAsync(int requestId, int applicationId)
//        {
//            var client = await _db.Clients.SingleAsync(c => c.UserId == _me.UserId);

//            var app = await _db.ProviderApplications
//                .Include(a => a.Request)
//                .SingleAsync(a => a.Id == applicationId && a.RequestId == requestId);

//            if (app.Request.ClientProfileId != client.Id)
//                return false;

//            app.Status = ApplicationStatus.Accepted;
//            app.Request.AssignedProviderId = app.ProviderId;
//            app.Request.Status = RequestStatus.Assigned;

//            // Reject others
//            var others = await _db.ProviderApplications
//                .Where(a => a.RequestId == requestId && a.Id != applicationId)
//                .ToListAsync();
//            foreach (var o in others) o.Status = ApplicationStatus.Rejected;

//            await _db.SaveChangesAsync();
//            return true;
//        }
//    }
//}
