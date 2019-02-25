using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SinaExpoBot.DAL;
using SinaExpoBot.Domain.Entity;
using SinaExpoBot.Service.Interface;

namespace SinaExpoBot.Service
{
    public class ProvinceService : IProvinceService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICacheService _cacheService;

        public ProvinceService(ApplicationDbContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        #region Province

        private const string ProvinceCacheKey = "ProvinceList";

        private async Task<List<ProvinceEntity>> GetProvinceListAsync()
        {
            return await _context.Provinces.ToListAsync();
        }
        
        public async Task<List<ProvinceEntity>> GetProvinceListAsync(int first, int last)
        {
            var provinces = await _cacheService.GetOrSet(ProvinceCacheKey, GetProvinceListAsync);
            return provinces.OrderBy(x => x.Title).Skip(first).Take(last - first).ToList();
        }

        public async Task<ProvinceEntity> GetProvinceAsync(string province)
        {
            var provinces = await _cacheService.GetOrSet(ProvinceCacheKey, GetProvinceListAsync);
            return provinces.FirstOrDefault(x => x.Title == province);
        }

        public async Task<int> GetProvinceCountAsync()
        {
            var provinces = await _cacheService.GetOrSet(ProvinceCacheKey, GetProvinceListAsync);
            return provinces.Count();
        }

        public async Task<List<ProvinceEntity>> SearchProvinceAsync(string searchText, int limit)
        {
            var provinces = await _cacheService.GetOrSet(ProvinceCacheKey, GetProvinceListAsync);
            return provinces.Where(x => x.Title.Contains(searchText)).Take(limit).ToList();
        }

        #endregion
    }
}