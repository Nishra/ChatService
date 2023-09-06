using Domain.DTO;

namespace Application.Interface
{
    public interface IMessageValidation
    {
        public bool ValidateMessageQueue(Team currentTeam);
    }
}
