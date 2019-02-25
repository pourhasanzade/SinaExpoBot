using System;
using System.Threading.Tasks;
using SinaExpoBot.Domain.Entity;

namespace SinaExpoBot.Service.Interface
{
    public interface IExceptionLogService
    {
        Task<ExceptionLogEntity> LogException(Exception exception, string chatId = null, string title = null);
    }
}
