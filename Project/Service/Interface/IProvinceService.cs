using System.Collections.Generic;
using System.Threading.Tasks;
using SinaExpoBot.Domain.Entity;

namespace SinaExpoBot.Service.Interface
{
    public interface IProvinceService
    {
        Task<List<ProvinceEntity>> GetProvinceListAsync(int first, int last);
        Task<ProvinceEntity> GetProvinceAsync(string province);
        Task<int> GetProvinceCountAsync();
        Task<List<ProvinceEntity>> SearchProvinceAsync(string searchText, int limit);
    }
}
