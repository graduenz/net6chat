using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Net6Chat.Domain.DTO;
using Net6Chat.Domain.Models;

namespace Net6Chat.Domain.Services.Impl
{
    public class DefaultChatService : IChatService
    {
        private readonly IDictionary<string, List<ConnectedClientDto>> _connectedClients = new Dictionary<string, List<ConnectedClientDto>>();
        
        private readonly ApplicationDbContext _dbContext;
        private readonly ICapPublisher _capBus;

        public DefaultChatService(ApplicationDbContext dbContext, ICapPublisher capPublisher)
        {
            _dbContext = dbContext;
            _capBus = capPublisher;
        }

        public void AddConnectedClient(string room, string connectionId, string? userName)
        {
            if (!_connectedClients.TryGetValue(room, out var list)) {
                list = new List<ConnectedClientDto>();
            }

            list.Add(new ConnectedClientDto() {
                ConnectionId = connectionId,
                UserName = userName,
            });
        }

        public void RemoveConnectedClient(string room, string connectionId)
        {
            if (!_connectedClients.TryGetValue(room, out var list)) {
                throw new InvalidOperationException($"Room '{room}' does not exist");
            }

            var entry = list.FirstOrDefault(m => m.ConnectionId == connectionId);
            if (entry == null) {
                throw new InvalidOperationException($"Connection '{connectionId}' in room '{room}' does not exist");
            }

            list.Remove(entry);
        }

        public async Task PersistMessageAsync(Message message)
        {
            using (var transaction = _dbContext.Database.BeginTransaction(_capBus, autoCommit: true))
            {
                await _capBus.PublishAsync("persistence.message", message);
            }
        }

        public async Task<IList<Message>> FetchMessagesAsync(string room, int limit = 50)
        {
            return await _dbContext.Messages
                .AsNoTracking()
                .Where(m => m.Room == room)
                .OrderBy(m => m.Created)
                .Take(limit)
                .ToListAsync();
        }
    }
}
