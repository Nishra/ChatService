using ChatService.Interface;
using Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }
        // POST api/<ChatController>
        [HttpPost]
        public IActionResult Post([FromBody] ChatMessage chatMessage)
        {
           if (!ModelState.IsValid) return BadRequest();
           return Ok(_chatService.SendMessage(chatMessage));
        }
    }
}
