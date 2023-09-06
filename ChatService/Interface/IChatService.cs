using Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.Interface
{
    public interface IChatService
    {
        public IActionResult SendMessage(ChatMessage chatMessage);
    }
}
