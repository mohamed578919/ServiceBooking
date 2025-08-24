//using Microsoft.AspNetCore.SignalR;
//using ServiceBooking.DTOs;

//namespace ServiceBooking.Logics
//{
//    public class ChatHub : Hub
//    {
//        private readonly ChatService _chat;
//        public ChatHub(ChatService chat) => _chat = chat;

//        public async Task SendMessage(SendMessageDto dto)
//        {
//            var msgId = await _chat.SendAsync(dto);
//            // broadcast داخل جروب المحادثة (اسم الجروب = request:{id})
//            await Clients.Group($"request:{dto.RequestId}")
//                .SendAsync("ReceiveMessage", new { messageId = msgId, dto.RequestId, dto.Text, sentAt = DateTime.UtcNow });
//        }

//        public async Task JoinRequestGroup(int requestId)
//        {
//            // لو لسه متعملتش Conversation هتتعمل تلقائي عند أول رسالة
//            await Groups.AddToGroupAsync(Context.ConnectionId, $"request:{requestId}");
//        }

//        public async Task LeaveRequestGroup(int requestId)
//        {
//            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"request:{requestId}");
//        }
//    }
//}
