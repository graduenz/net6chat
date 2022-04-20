using Microsoft.AspNetCore.SignalR;
using Net6Chat.Domain.DTO;
using Net6Chat.Domain.Models;
using Net6Chat.Domain.Services;

namespace Net6Chat.WebApp.Hubs
{
    public class ChatHub : Hub
    {
        public static readonly string[] KnownChatRooms = new[] { "A", "B", "C" };

        private readonly IChatService _chatService;

        public ChatHub(IChatService chatRoomService)
        {
            _chatService = chatRoomService;
        }

        public override async Task OnConnectedAsync()
        {
            var room = GetRoom();
            _chatService.AddConnectedClient(room, Context.ConnectionId, Context.User?.Identity?.Name);
            await Groups.AddToGroupAsync(Context.ConnectionId, room);

            var messages = await _chatService.FetchMessagesAsync(room);
            var messageDtos = messages.Select(m => new MessageDto() {
                User = m.UserName,
                Message = m.Text,
                Time = m.Created.ToString("G")
            }).ToList();
            await Clients.Caller.SendAsync("FetchMessages", messageDtos);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var room = GetRoom();
            _chatService.RemoveConnectedClient(room, Context.ConnectionId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string message)
        {
            var entity = new Message() {
                Room = GetRoom(),
                Text = message,
                UserName = Context.User?.Identity?.Name,
                Created = DateTime.Now,
            };

            await Clients.Group(entity.Room).SendAsync("ReceiveMessage", entity.UserName, message, entity.Created.ToString("G"));

            await _chatService.PersistMessageAsync(entity);
        }

        private string GetRoom()
        {
            var httpContext = Context.GetHttpContext();
            var room = httpContext?.Request.Query["room"].FirstOrDefault();

            if (string.IsNullOrEmpty(room) || !KnownChatRooms.Contains(room)) throw new InvalidOperationException("Missing 'room' query parameter");

            return room;
        }
    }
}
