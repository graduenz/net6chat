using Net6Chat.Domain.Models;

namespace Net6Chat.Domain.Services
{
    public interface IChatService
    {
        void AddConnectedClient(string room, string connectionId, string? userName);
        void RemoveConnectedClient(string room, string connectionId);
        Task PersistMessageAsync(Message message);
        Task<IList<Message>> FetchMessagesAsync(string room, int limit = 50);
    }
}
