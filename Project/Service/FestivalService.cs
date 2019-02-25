using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
//using Fam.Core.Infrustructure.Base;
using SinaExpoBot.API.Json.Output;
using SinaExpoBot.DAL;
using SinaExpoBot.Domain.Entity;
using SinaExpoBot.Domain.Enum;
using SinaExpoBot.Service.Interface;
using SinaExpoBot.Utility;

namespace SinaExpoBot.Service
{
    public class FestivalService : IFestivalService
    {

        private readonly ApplicationDbContext _context;
        private readonly ICacheService _cacheService;
        private const string AxesCacheKey = "AxesList";
        private const string FieldsCacheKey = "FieldsList";
        private const string MajorCacheKey = "MajorsList";
        private const string PriceCacheKey = "PricesList";

        public FestivalService(ApplicationDbContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        //---------------------- Festival Axes ----------------------
        private async Task<List<FestivalAxesEntity>> GetAxesListAsync()
        {
            return await _context.FestivalAxes.ToListAsync();
        }

        public async Task<FestivalAxesEntity> GetFestivalAxes(string title)
        {
            var axes = await _cacheService.GetOrSet(AxesCacheKey, GetAxesListAsync);
            return axes.FirstOrDefault(x => x.Title == title);
        }

        public async Task<FestivalAxesEntity> GetFestivalAxes(long id)
        {
            var axes = await _cacheService.GetOrSet(AxesCacheKey, GetAxesListAsync);
            return axes.FirstOrDefault(x => x.Id == id);
        }

        public async Task<List<FestivalAxesEntity>> GetFestivalAxesListAsync(int first, int last)
        {
            var axes = await _cacheService.GetOrSet(AxesCacheKey, GetAxesListAsync);
            return axes.OrderBy(x => x.Title).Skip(first).Take(last - first).ToList();
        }

        public async Task<int> GetFestivalAxesListCountAsync()
        {
            var axes = await _cacheService.GetOrSet(AxesCacheKey, GetAxesListAsync);
            return axes.Count();
        }

        public async Task<List<FestivalAxesEntity>> SearchFestivalAxesListAsync(string searchText, int limit)
        {
            var list = await _cacheService.GetOrSet(AxesCacheKey, GetAxesListAsync);
            return list.Where(x=>x.Title == searchText).OrderBy(x => x.Title).Take(limit).ToList();
        }

        //---------------------- Festival Fields ----------------------

        private async Task<List<FestivalFieldEntity>> GetFieldsListAsync()
        {
            return await _context.Fields.ToListAsync();
        }

        public async Task<FestivalFieldEntity> GetFestivalField(string title)
        {
            var fields = await _cacheService.GetOrSet(FieldsCacheKey, GetFieldsListAsync);
            return fields.FirstOrDefault(x => x.Title == title);
        }

        public async Task<FestivalFieldEntity> GetFestivalField(long id)
        {
            var fields = await _cacheService.GetOrSet(FieldsCacheKey, GetFieldsListAsync);
            return fields.FirstOrDefault(x => x.Id == id);
        }

        public async Task<List<FestivalFieldEntity>> GetFestivalFieldsListAsync(long festivalAxesId,int first, int last)
        {
            var axes = await _cacheService.GetOrSet(FieldsCacheKey, GetFieldsListAsync);
            return axes.Where(x=> x.FestivalAxesId == festivalAxesId).OrderBy(x => x.Title).Skip(first).Take(last - first).ToList();
        }

        public async Task<int> GetFestivalFieldsListCountAsync(long festivalAxesId)
        {
            var axes = await _cacheService.GetOrSet(FieldsCacheKey, GetFieldsListAsync);
            return axes.Count(x => x.FestivalAxesId == festivalAxesId);
        }

        public async Task<List<FestivalFieldEntity>> SearchFestivalFieldsListAsync(long festivalAxesId , string searchText, int limit)
        {
            var list = await _cacheService.GetOrSet(FieldsCacheKey, GetFieldsListAsync);
            return list.Where(x =>x.FestivalAxesId == festivalAxesId &&  x.Title == searchText).OrderBy(x => x.Title).Take(limit).ToList();
        }

        //---------------------- Festival Majors ----------------------

        private async Task<List<FestivalMajorEntity>> GetMajorsListAsync()
        {
            return await _context.Majors.ToListAsync();
        }

        public async Task<FestivalMajorEntity> GetFestivalMajor(string title)
        {
            var fields = await _cacheService.GetOrSet(FieldsCacheKey, GetMajorsListAsync);
            return fields.FirstOrDefault(x => x.Title == title);
        }

        public async Task<FestivalMajorEntity> GetFestivalMajor(long id)
        {
            var fields = await _cacheService.GetOrSet(FieldsCacheKey, GetMajorsListAsync);
            return fields.FirstOrDefault(x => x.Id == id);
        }

        public async Task<List<FestivalMajorEntity>> GetFestivalMajorsListAsync(long festivalFieldId, int first, int last)
        {
            var axes = await _cacheService.GetOrSet(MajorCacheKey, GetMajorsListAsync);
            return axes.Where(x => x.FieldId == festivalFieldId).OrderBy(x => x.Title).Skip(first).Take(last - first).ToList();
        }

        public async Task<int> GetFestivalMajorsListCountAsync(long festivalFieldId)
        {
            var axes = await _cacheService.GetOrSet(MajorCacheKey, GetMajorsListAsync);
            return axes.Count(x => x.FieldId == festivalFieldId);
        }

        public async Task<List<FestivalMajorEntity>> SearchFestivalMajorsListAsync(long festivalFieldId, string searchText, int limit)
        {
            var list = await _cacheService.GetOrSet(MajorCacheKey, GetMajorsListAsync);
            return list.Where(x => x.FieldId == festivalFieldId && x.Title == searchText).OrderBy(x => x.Title).Take(limit).ToList();
        }
        // -------------------------PRICE ---------------------------------------

        private async Task<List<PriceEntity>> GetPricesListAsync()
        {
            return await _context.Prices.ToListAsync();
        }

        public async Task<PriceEntity> GetPrice(CenterTypeEnum type, long axId)
        {
            var list = await _cacheService.GetOrSet(PriceCacheKey, GetPricesListAsync);
            return list.FirstOrDefault(x => x.FestivalAxesId == axId && x.CenterType == type);
        }

       
    }
}