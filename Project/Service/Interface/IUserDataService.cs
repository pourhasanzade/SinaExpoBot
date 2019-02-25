using System.Threading.Tasks;
using SinaExpoBot.Domain.Entity;

namespace SinaExpoBot.Service.Interface
{
    public interface IUserDataService
    {
        Task<UserDataEntity> GetUserData(string chatId);
        Task<(UserDataEntity userData, bool isNew)> Update(string chatId, long stateId);
    }
}