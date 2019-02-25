using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
//using Fam.Core.Infrustructure.Base;
using SinaExpoBot.API.Json.Output;
using SinaExpoBot.DAL;
using SinaExpoBot.Domain.Entity;
using SinaExpoBot.Domain.Enum;
using SinaExpoBot.Domain.Model;
using SinaExpoBot.Service.Interface;
using SinaExpoBot.Utility;

namespace SinaExpoBot.Service
{
    public class GroupService : IGroupService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFestivalService _festivalService;

        public GroupService(ApplicationDbContext context, IFestivalService festivalService)
        {
            _context = context;
            _festivalService = festivalService;
        }

        public async Task<GroupEntity> GetGroupEntity(string chatId)
        {
            return await _context.Groups.FirstOrDefaultAsync(x => x.ChatId == chatId);
        }

        public async Task<GroupEntity> GetGroupInfo(string chatId)
        {
            var info = await GetGroupEntity(chatId);
            if (info == null)
            {
               info = new GroupEntity
                {
                    ChatId = chatId
                };
                _context.Groups.Add(info);
                await _context.SaveChangesAsync();               
            }
            return info;
        }

        #region Update

        public async Task<GroupEntity> SetCenterName(string chatId, string input)
        {
            var info = await GetGroupInfo(chatId);
            info.CenterName = input;
            await _context.SaveChangesAsync();
            return info;
        }

        public async Task<GroupEntity> SetCenterType(string chatId, CenterTypeEnum input)
        {
            var info = await GetGroupInfo(chatId);
            info.CenterType = input;
            await _context.SaveChangesAsync();
            return info;
        }

        public async Task<GroupEntity> SetGenderType(string chatId, CenterGenderEnum input)
        {
            var info = await GetGroupInfo(chatId);
            info.CenterGenderType = input;
            await _context.SaveChangesAsync();
            return info;
        }

        public async Task<GroupEntity> SetProvince(string chatId, ProvinceEntity province)
        {
            var info = await GetGroupInfo(chatId);
            info.ProvinceName = province.Title;
            info.ProvinceId = province.Id;
            await _context.SaveChangesAsync();
            return info;
        }

        public async Task<GroupEntity> SetAddress(string chatId, string input)
        {
            var info = await GetGroupInfo(chatId);
            info.Address = input;
            await _context.SaveChangesAsync();
            return info;
        }

        public async Task<GroupEntity> SetPostalCode(string chatId, string input)
        {
            var info = await GetGroupInfo(chatId);
            info.PostalCode = input;
            await _context.SaveChangesAsync();
            return info;
        }

        public async Task<GroupEntity> SetPhone(string chatId, string input)
        {
            var info = await GetGroupInfo(chatId);
            info.Phone = input;
            await _context.SaveChangesAsync();
            return info;
        }

        public async Task<GroupEntity> SetManagerName(string chatId, string input)
        {
            var info = await GetGroupInfo(chatId);
            info.ManagerName = input;
            await _context.SaveChangesAsync();
            return info;
        }

        //--------- Supervisor info ---------//

        public async Task<GroupEntity> SetSupervisorName(string chatId, string input)
        {
            var info = await GetGroupInfo(chatId);
            info.SupervisorName = input;
            await _context.SaveChangesAsync();
            return info;
        }

        public async Task<GroupEntity> SetSupervisorMobile(string chatId, string input)
        {
            var info = await GetGroupInfo(chatId);
            info.SupervisorMobile = input;
            await _context.SaveChangesAsync();
            return info;
        }

        public async Task<GroupEntity> SetSupervisorEmail(string chatId, string input)
        {
            var info = await GetGroupInfo(chatId);
            info.SupervisorEmail = input;
            await _context.SaveChangesAsync();
            return info;
        }

        //-------- Project Info -----------------//

        public async Task<GroupEntity> SetProjectSubject(string chatId, string input)
        {
            var info = await GetGroupInfo(chatId);
            info.ProjectSubject = input;
            await _context.SaveChangesAsync();
            return info;
        }

        public async Task<GroupEntity> SetFestivalAxes(string chatId, FestivalAxesEntity axes)
        {
            var info = await GetGroupInfo(chatId);
            info.AxTitle = axes.Title;
            info.FestivalAxId = axes.Id;
            await _context.SaveChangesAsync();
            return info;
        }

        public async Task<GroupEntity> SetFestivalField(string chatId, FestivalFieldEntity field)
        {
            var info = await GetGroupInfo(chatId);
            info.FieldTitle = field.Title;
            info.FestivalFieldId = field.Id;
            await _context.SaveChangesAsync();
            return info;
        }

        public async Task<GroupEntity> SetFestivalMajor(string chatId, FestivalMajorEntity major)
        {
            var info = await GetGroupInfo(chatId);
            info.MajorTitle = major.Title;
            info.FestivalMajorId = major.Id;
            await _context.SaveChangesAsync();
            return info;
        }
        //--------- Members ----------------//
        public async Task<GroupEntity> SetMembersCount(string chatId, int input)
        {
            var info = await GetGroupInfo(chatId);
            info.MembersCount = input;
            await _context.SaveChangesAsync();

            await DeleteMembers(chatId, info.Id, input);
            return info;
        }

        public async Task<GroupEntity> SetMembersGrade(string chatId, MembersGradeEnum input)
        {
            var info = await GetGroupInfo(chatId);
            info.MembersGrade = input;
            await _context.SaveChangesAsync();
            return info;
        }

        //-------- Pay ----------------//
        public async Task<GroupEntity> SetPrice (string chatId)
        {
            var info = await GetGroupInfo(chatId);

            var priceInfo = await _festivalService.GetPrice(info.CenterType.Value, info.FestivalAxId.Value);
            info.Price = priceInfo.PriceAmount;
            await _context.SaveChangesAsync();
            return info;
        }

        public async Task<GroupEntity> SetFinished(string chatId, long orderId)
        {
            var info = await GetGroupInfo(chatId);
            info.OrderId = orderId;
            info.PayDate = DateTime.Now;
            info.IsFinished = true;
            await _context.SaveChangesAsync();
            return info;
        }

        #endregion


        #region Members

        private async Task<GroupMemberEntity> GetMemberEntiy(string chatId,long groupId,  int number)
        {
             return await _context.GroupMembers.FirstOrDefaultAsync(x => x.ChatId == chatId && x.GroupId == groupId && x.MemberNumber == number && !x.IsDeleted);
        }

        public async Task<GroupMemberEntity> GetMember(string chatId, long groupId, int number)
        {
            var info = await GetMemberEntiy(chatId, groupId, number);
            if (info == null)
            {
                info = new GroupMemberEntity
                {
                    ChatId = chatId,
                    GroupId = groupId,
                    MemberNumber =  number,
                };
                _context.GroupMembers.Add(info);
                await _context.SaveChangesAsync();
            }
            return info;
        }

        public async Task<List<GroupMemberEntity>> GetMembers(string chatId, long groupId)
        {
            return await _context.GroupMembers.Where(x => x.ChatId == chatId && x.GroupId == groupId && !x.IsDeleted).OrderBy(x=>x.MemberNumber).ToListAsync();
        }

        public async Task<GroupMemberEntity> SetMemberName(string chatId, long groupId, int number, string input)
        {
            var info = await GetMember(chatId,groupId, number);
            info.Name = input;
            await _context.SaveChangesAsync();
            return info;
        }

        public async Task<GroupMemberEntity> SetMemberNationalCode(string chatId,long groupId, int number, string input)
        {
            var info = await GetMember(chatId,groupId, number);
            info.NationalCode = input;
            await _context.SaveChangesAsync();
            return info;
        }

        public async Task<bool> DeleteMembers(string chatId, long groupId, int number)
        {
            var query = @"
UPDATE GroupMembers
SET IsDeleted = 1
WHERE ChatId = @ChatId AND GroupId = @GroupId AND IsDeleted = 0 AND MemberNumber> @Number
";

            using (var connection = new SqlConnection(Utility.Variables.ConnectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@ChatId", chatId);
                command.Parameters.AddWithValue("@GroupId", groupId);
                command.Parameters.AddWithValue("@Number", number);
                command.CommandTimeout = 3600;

                await connection.OpenAsync();

                try
                {
                    using (var reader = await command.ExecuteReaderAsync()){}
                }
                catch(Exception e){ throw e; }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

                return true;
            }
        }
        #endregion
    }
}