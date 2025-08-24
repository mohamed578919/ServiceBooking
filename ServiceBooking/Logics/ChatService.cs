//using ApiDay1.Models;
//using Microsoft.EntityFrameworkCore;
//using ServiceBooking.DTOs;
//using ServiceBooking.Models;

//namespace ServiceBooking.Logics
//{
//    public class ChatService
//    {
//        private readonly MyContext _db;
//        private readonly ICurrentUser _me;

//        public ChatService(MyContext db, ICurrentUser me)
//        {
//            _db = db; _me = me;
//        }

//        public async Task<int> EnsureConversationAsync(int requestId)
//        {
//            var req = await _db.ServiceRequests
//                .Include(r => r.ClientProfile)
//                .Include(r => r.AssignedProvider)
//                .SingleAsync(r => r.Id == requestId);

//            if (req.AssignedProviderId == null)
//                throw new InvalidOperationException("Request not assigned yet.");

//            var conv = await _db.Conversations
//                .FirstOrDefaultAsync(c => c.RequestId == requestId);

//            if (conv != null) return conv.Id;

//            conv = new Conversation
//            {
//                RequestId = req.Id,
//                ClientProfileId = req.ClientProfileId,
//                ProviderProfileId = req.AssignedProviderId.Value
//            };
//            _db.Conversations.Add(conv);
//            await _db.SaveChangesAsync();
//            return conv.Id;
//        }

//        public async Task<int> SendAsync(SendMessageDto dto)
//        {
//            var convId = await EnsureConversationAsync(dto.RequestId);

//            var msg = new ChatMessage
//            {
//                ConversationId = convId,
//                SenderUserId = _me.UserId,
//                Text = dto.Text
//            };
//            _db.ChatMessages.Add(msg);
//            await _db.SaveChangesAsync();
//            return msg.Id;
//        }

//        public async Task<List<(DateTime sentAt, string senderId, string text)>> GetMessagesAsync(int requestId, int take = 50, int skip = 0)
//        {
//            var conv = await _db.Conversations.SingleAsync(c => c.RequestId == requestId);
//            return await _db.ChatMessages
//                .Where(m => m.ConversationId == conv.Id)
//                .OrderByDescending(m => m.SentAt)
//                .Skip(skip).Take(take)
//                .Select(m => new ValueTuple<DateTime, string, string>(m.SentAt, m.SenderUserId, m.Text))
//                .ToListAsync();
//        }
//    }
//}
