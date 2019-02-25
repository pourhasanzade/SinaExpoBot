using System.Threading.Tasks;
using SinaExpoBot.Domain.Entity;

namespace SinaExpoBot.Service.Interface
{
    public interface IConfigService
    {
        Task UpdateLastMessageId(string messageId);
        Task<ConfigEntity> GetConfig();
    }
}