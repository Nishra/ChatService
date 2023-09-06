using Application.Interface;
using ChatService.Interface;
using Common.SeedData;
using Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.Services
{
    public class ChatService : IChatService
    {
        private readonly ILogger<ChatService> _logger;
        private readonly IMessageProducer _messageProducer;
        private readonly IMessageValidation _messageValidation;

        public ChatService(ILogger<ChatService> logger, IMessageProducer messageProducer,
                           IMessageValidation messageValidation)
        {
            _logger = logger;
            _messageProducer = messageProducer;
            _messageValidation = messageValidation;
        }

        public IActionResult SendMessage(ChatMessage chatMessage)
        {
            try
            {
                _logger.LogInformation($"{nameof(ChatService)} : {nameof(SendMessage)} Started");

                //TODO : This should come from a Live DB
                Team currentTeam = SeedAgentTeamData.GetTeamADetails();
                bool isQueueAccept = _messageValidation.ValidateMessageQueue(currentTeam);

                if (isQueueAccept)
                {
                    _messageProducer.SendMessage(chatMessage);
                    _logger.LogInformation($"{nameof(ChatService)} : {nameof(SendMessage)} End");
                    return new OkObjectResult("Received Successfully, Please hold on for the next available agent who will assist you shortly.");
                }
                else
                {
                    return new BadRequestObjectResult("No Agent available at the moment");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ChatService)} : {nameof(SendMessage)}", ex.Message);
                throw;
            }
            
        }
    }
}
