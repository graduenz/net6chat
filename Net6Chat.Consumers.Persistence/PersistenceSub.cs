using DotNetCore.CAP;
using Net6Chat.Domain;
using Net6Chat.Domain.Models;

namespace Net6Chat.Consumers.Persistence
{
    [CapSubscribe("persistence")]
    public class PersistenceSub : ICapSubscribe
    {
        private readonly ApplicationDbContext _dbContext;

        public PersistenceSub(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [CapSubscribe("message", isPartial: true)]
        public async Task MessageAdd(Message message)
        {
            await _dbContext.Messages.AddAsync(message);
            await _dbContext.SaveChangesAsync();
        }
    }
}
