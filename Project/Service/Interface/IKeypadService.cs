using System.Threading.Tasks;
using SinaExpoBot.Domain.Model;

namespace SinaExpoBot.Service.Interface
{
    public interface IKeypadService
    {
        Task<KeypadModel> GenerateButtonsAsync(string chatId, long stateId, object myObject = null);
    }
}
