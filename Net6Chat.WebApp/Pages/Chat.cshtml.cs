using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Net6Chat.WebApp.Hubs;

namespace Net6Chat.WebApp.Pages
{
    public class ChatModel : PageModel
    {
        private readonly ILogger<ChatModel> _logger;

        public ChatModel(ILogger<ChatModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            var room = Request.Query["room"];

            if (!ChatHub.KnownChatRooms.Contains(room.Single())) throw new InvalidOperationException($"Chat room '{room}' does not exist");

            ViewData["Room"] = room;
            ViewData["Title"] = $"Chat Room {room}";

            return Page();
        }
    }
}