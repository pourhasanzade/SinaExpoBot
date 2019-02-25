using System.Collections.Generic;
using System.Threading.Tasks;
//using Fam.Core.Infrustructure.Base;
using SinaExpoBot.Domain.Entity;
using SinaExpoBot.Domain.Enum;

namespace SinaExpoBot.Service.Interface
{
    public interface IGroupService
    {
        Task<GroupEntity> GetGroupInfo(string chatId);

        #region Update
       
        Task<GroupEntity> SetCenterName(string chatId, string input);
        Task<GroupEntity> SetCenterType(string chatId, CenterTypeEnum input);
        Task<GroupEntity> SetGenderType(string chatId, CenterGenderEnum input);
        Task<GroupEntity> SetProvince(string chatId, ProvinceEntity province);
        Task<GroupEntity> SetAddress(string chatId, string input);
        Task<GroupEntity> SetPostalCode(string chatId, string input);
        Task<GroupEntity> SetPhone(string chatId, string input);
        Task<GroupEntity> SetManagerName(string chatId, string input);

        //--------- Supervisor info ---------//

        Task<GroupEntity> SetSupervisorName(string chatId, string input);
        Task<GroupEntity> SetSupervisorMobile(string chatId, string input);
        Task<GroupEntity> SetSupervisorEmail(string chatId, string input);

        //-------- Project Info -----------------//

        Task<GroupEntity> SetProjectSubject(string chatId, string input);
        Task<GroupEntity> SetFestivalAxes(string chatId, FestivalAxesEntity axes);
        Task<GroupEntity> SetFestivalField(string chatId, FestivalFieldEntity field);
        Task<GroupEntity> SetFestivalMajor(string chatId, FestivalMajorEntity major);
        //--------- Members ----------------//
        Task<GroupEntity> SetMembersCount(string chatId, int input);
        Task<GroupEntity> SetMembersGrade(string chatId, MembersGradeEnum input);

        //-------- Pay ----------------//
        Task<GroupEntity> SetPrice(string chatId);
        Task<GroupEntity> SetFinished(string chatId, long orderId);

        #endregion


        Task<GroupMemberEntity> GetMember(string chatId, long groupId, int number);
        Task<List<GroupMemberEntity>> GetMembers(string chatId, long groupId);
        Task<GroupMemberEntity> SetMemberName(string chatId, long groupId, int number, string input);
        Task<GroupMemberEntity> SetMemberNationalCode(string chatId, long groupId, int number, string input);
    }
}