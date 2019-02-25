using System.Collections.Generic;
using System.Threading.Tasks;
using SinaExpoBot.API.Json.Output;
using SinaExpoBot.Domain.Entity;
using SinaExpoBot.Domain.Enum;

namespace SinaExpoBot.Service.Interface
{
    public interface IFestivalService
    {
        //---------------------- Festival Axes ----------------------
        Task<FestivalAxesEntity> GetFestivalAxes(string title);
        Task<FestivalAxesEntity> GetFestivalAxes(long id);
        Task<List<FestivalAxesEntity>> GetFestivalAxesListAsync(int first, int last);
        Task<int> GetFestivalAxesListCountAsync();
        Task<List<FestivalAxesEntity>> SearchFestivalAxesListAsync(string searchText, int limit);     

        //---------------------- Festival Fields ----------------------

        Task<FestivalFieldEntity> GetFestivalField(string title);
        Task<FestivalFieldEntity> GetFestivalField(long id);
        Task<List<FestivalFieldEntity>> GetFestivalFieldsListAsync(long festivalAxesId, int first, int last);
        Task<int> GetFestivalFieldsListCountAsync(long festivalAxesId);
        Task<List<FestivalFieldEntity>>SearchFestivalFieldsListAsync(long festivalAxesId, string searchText, int limit);       

        //---------------------- Festival Majors ----------------------

        Task<FestivalMajorEntity> GetFestivalMajor(string title);
        Task<FestivalMajorEntity> GetFestivalMajor(long id);
        Task<List<FestivalMajorEntity>> GetFestivalMajorsListAsync(long festivalFieldId, int first, int last);
        Task<int> GetFestivalMajorsListCountAsync(long festivalFieldId);
        Task<List<FestivalMajorEntity>> SearchFestivalMajorsListAsync(long festivalFieldId, string searchText,
            int limit);

        //----------------------- Price --------------------------------
        Task<PriceEntity> GetPrice(CenterTypeEnum type, long axId);
    }
}