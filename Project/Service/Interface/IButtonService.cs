using System.Collections.Generic;
using System.Threading.Tasks;
using SinaExpoBot.Domain.Entity;

namespace SinaExpoBot.Service.Interface
{
    public interface IButtonService
    {
        Task<ButtonEntity> GetButton(string code);
        Task<List<ButtonEntity>> GetButtonList(long stateId);
    }
}