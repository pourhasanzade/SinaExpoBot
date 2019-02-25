using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using SinaExpoBot.API.Json.Input;
using SinaExpoBot.API.Json.Output;
using SinaExpoBot.DAL;
using SinaExpoBot.Domain.Entity;
using SinaExpoBot.Domain.Enum;
using SinaExpoBot.Domain.Model;
using SinaExpoBot.Service.Interface;
using SinaExpoBot.Utility;

namespace SinaExpoBot.API
{
    [RoutePrefix("api")]
    public class HomeController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMessengerService _messengerService;
        private readonly IKeypadService _keypadService;
        private readonly IConfigService _configService;
        private readonly IUserDataService _userDataService;
        private readonly IButtonService _buttonService;
        private readonly IProvinceService _provinceService;
        private readonly IOrderService _orderService;
        private readonly IFestivalService _festivalService;
        private readonly IGroupService _groupService;
        private readonly IApiService _apiService;

        private static bool _isSendingMessage;



        public HomeController(ApplicationDbContext context,
            IMessengerService messengerService,
            IKeypadService keypadService, IConfigService configService,
            IUserDataService userDataService, IOrderService orderService,
            IButtonService buttonService, IProvinceService provinceService,
            IFestivalService festivalService, IGroupService groupService,
            IApiService apiService)
        {
            _context = context;
            _messengerService = messengerService;
            _keypadService = keypadService;
            _configService = configService;
            _userDataService = userDataService;
            _buttonService = buttonService;
            _provinceService = provinceService;
            _festivalService = festivalService;
            _groupService = groupService;
            _orderService = orderService;
            _apiService = apiService;
        }

        #region API

        [HttpGet, Route("getMessage")]
        public async Task<IHttpActionResult> GetMessages()
        {
            try
            {
                if (_isSendingMessage) return CustomResult();

                var config = await _configService.GetConfig();
                var result = await _messengerService.GetMessages(long.Parse(config.LastMessageId));

                if (result != null && result.Data.MessageList.Count > 0)
                {
                    _isSendingMessage = true;

                    var listOfMessageList = result.Data.MessageList.GroupBy(x => x.ChatId).Select(x => x.ToList()).ToList();

                    foreach (var messageList in listOfMessageList)
                    {

                        var myMessage = messageList.OrderByDescending(x => x.MessageId).FirstOrDefault();

                        if (myMessage?.ChatId == null /*|| myMessage.ChatId != "b_7149751_7"*/) continue;

                        await _configService.UpdateLastMessageId(myMessage.MessageId);
                        var tuple = await Command(myMessage);
                        await _messengerService.SendMessage(tuple.model);
                    }

                    _isSendingMessage = false;
                }
            }
            catch (Exception e)
            {
                _isSendingMessage = false;
                return CustomError(e);
            }
            return CustomResult();
        }

        [HttpPost, Route("receiveMessage")]
        public async Task<IHttpActionResult> ReceiveMessage([FromBody] GetMessagesInput json)
        {
            try
            {
                if (json.Type == MessageBehaviourTypeEnum.Message)
                    await _configService.UpdateLastMessageId(json.Mesage.MessageId);


                var tuple = await Command(json.Mesage);

                if (json.Type == MessageBehaviourTypeEnum.API)
                {
                    if (!tuple.toast) await _messengerService.SendMessage(tuple.model);
                    return Ok(new { bot_keypad = tuple.model.Keypad, text_message = tuple.toast ? tuple.model.Text : "" });
                }

                await _messengerService.SendMessage(tuple.model);
                return CustomResult();
            }
            catch (Exception exception)
            {
                return CustomError(exception);
            }
        }

        [HttpPost, Route("searchSelection")]
        public async Task<IHttpActionResult> SearchSelection([FromBody] SearchSelectionInput json)
        {
            try
            {
                if (json.SelectionId == "provinces") // استان
                {
                    var provinceList = await _provinceService.SearchProvinceAsync(json.SearchText, json.Limit);

                    var buttonSimpleList = new List<ButtonSimpleModel>();
                    foreach (var province in provinceList)
                    {
                        buttonSimpleList.Add(new ButtonSimpleModel
                        {
                            Type = ButtonSimpleTypeEnum.TextOnly,
                            Text = province.Title,
                            ImageUrl = null
                        });
                    }

                    return Ok(new { items = buttonSimpleList });
                }
                if (json.SelectionId == "axes")
                {
                    var list = await _festivalService.SearchFestivalAxesListAsync(json.SearchText.PersianToEnglish(), json.Limit);
                    //var list = await _candidateService.SearchCandidateAsync(userInfo.StateName, json.SearchText, json.Limit);
                    var buttonSimpleList = new List<ButtonSimpleModel>();
                    foreach (var item in list)
                    {
                        buttonSimpleList.Add(new ButtonSimpleModel
                        {
                            Type = ButtonSimpleTypeEnum.TextOnly,
                            Text = $"{item.Title}",
                            ImageUrl = null
                        });
                    }

                    return Ok(new { items = buttonSimpleList });
                }

                if (json.SelectionId == "fields")
                {
                    var info = await _groupService.GetGroupInfo(json.ChatId);
                    var list = await _festivalService.SearchFestivalFieldsListAsync(info.FestivalAxId.Value, json.SearchText.PersianToEnglish(), json.Limit);
                    //var list = await _candidateService.SearchCandidateAsync(userInfo.StateName, json.SearchText, json.Limit);
                    var buttonSimpleList = new List<ButtonSimpleModel>();
                    foreach (var item in list)
                    {
                        buttonSimpleList.Add(new ButtonSimpleModel
                        {
                            Type = ButtonSimpleTypeEnum.TextOnly,
                            Text = $"{item.Title}",
                            ImageUrl = null
                        });
                    }

                    return Ok(new { items = buttonSimpleList });
                }

                if (json.SelectionId == "majors")
                {
                    var info = await _groupService.GetGroupInfo(json.ChatId);
                    var list = await _festivalService.SearchFestivalMajorsListAsync(info.FestivalFieldId.Value, json.SearchText.PersianToEnglish(), json.Limit);
                    //var list = await _candidateService.SearchCandidateAsync(userInfo.StateName, json.SearchText, json.Limit);
                    var buttonSimpleList = new List<ButtonSimpleModel>();
                    foreach (var item in list)
                    {
                        buttonSimpleList.Add(new ButtonSimpleModel
                        {
                            Type = ButtonSimpleTypeEnum.TextOnly,
                            Text = $"{item.Title}",
                            ImageUrl = null
                        });
                    }

                    return Ok(new { items = buttonSimpleList });
                }

                return CustomResult();
            }
            catch (Exception exception)
            {
                return CustomError(exception);
            }
        }

        [HttpPost, Route("getSelection")]
        public async Task<IHttpActionResult> GetSelection([FromBody] GetSelectionInput json)
        {
            json.FirstIndex = json.FirstIndex - 1;

            try
            {
                if (json.SelectionId == "provinces") // استان
                {
                    var count = await _provinceService.GetProvinceCountAsync();

                    if (json.FirstIndex > count) return Ok(new { items = new List<ButtonSimpleModel>() });
                    if (json.LastIndex > count) json.LastIndex = count;

                    var provinceList = await _provinceService.GetProvinceListAsync(json.FirstIndex, json.LastIndex);

                    var buttonSimpleList = new List<ButtonSimpleModel>();
                    foreach (var province in provinceList)
                    {
                        buttonSimpleList.Add(new ButtonSimpleModel
                        {
                            Type = ButtonSimpleTypeEnum.TextOnly,
                            Text = province.Title,
                            ImageUrl = null
                        });
                    }

                    return Ok(new { items = buttonSimpleList });
                }
                if (json.SelectionId == "axes")
                {
                    var count = await _festivalService.GetFestivalAxesListCountAsync();

                    if (json.FirstIndex > count) return Ok(new { items = new List<ButtonSimpleModel>() });
                    if (json.LastIndex > count) json.LastIndex = count;

                    var list = await _festivalService.GetFestivalAxesListAsync(json.FirstIndex, json.LastIndex);

                    var buttonSimpleList = new List<ButtonSimpleModel>();
                    foreach (var item in list)
                    {
                        buttonSimpleList.Add(new ButtonSimpleModel
                        {
                            Type = ButtonSimpleTypeEnum.TextOnly,
                            Text = $"{item.Title}",
                            ImageUrl = null
                        });
                    }

                    return Ok(new { items = buttonSimpleList });
                }

                if (json.SelectionId == "fields")
                {
                    var info = await _groupService.GetGroupInfo(json.ChatId);
                    var count = await _festivalService.GetFestivalFieldsListCountAsync(info.FestivalAxId.Value);

                    if (json.FirstIndex > count) return Ok(new { items = new List<ButtonSimpleModel>() });
                    if (json.LastIndex > count) json.LastIndex = count;

                    var list = await _festivalService.GetFestivalFieldsListAsync(info.FestivalAxId.Value, json.FirstIndex, json.LastIndex);

                    var buttonSimpleList = new List<ButtonSimpleModel>();
                    foreach (var item in list)
                    {
                        buttonSimpleList.Add(new ButtonSimpleModel
                        {
                            Type = ButtonSimpleTypeEnum.TextOnly,
                            Text = $"{item.Title}",
                            ImageUrl = null
                        });
                    }

                    return Ok(new { items = buttonSimpleList });
                }
                if (json.SelectionId == "majors")
                {
                    var info = await _groupService.GetGroupInfo(json.ChatId);
                    var count = await _festivalService.GetFestivalMajorsListCountAsync(info.FestivalFieldId.Value);

                    if (json.FirstIndex > count) return Ok(new { items = new List<ButtonSimpleModel>() });
                    if (json.LastIndex > count) json.LastIndex = count;

                    var list = await _festivalService.GetFestivalMajorsListAsync(info.FestivalFieldId.Value, json.FirstIndex, json.LastIndex);

                    var buttonSimpleList = new List<ButtonSimpleModel>();
                    foreach (var item in list)
                    {
                        buttonSimpleList.Add(new ButtonSimpleModel
                        {
                            Type = ButtonSimpleTypeEnum.TextOnly,
                            Text = $"{item.Title}",
                            ImageUrl = null
                        });
                    }

                    return Ok(new { items = buttonSimpleList });
                }

                return CustomResult();
            }
            catch (Exception exception)
            {
                return CustomError(exception);
            }
        }


        #endregion

        #region Main Methods

        private async Task<(SendMessageInput model, bool toast)> Command(MessageModel myMessage)
        {
            var buttonId = myMessage.AuxData == null ? "" : myMessage.AuxData.ButtonId;
            var chatId = myMessage.ChatId;
            var messageText = myMessage.Text;
            var userDataEntity = await _userDataService.GetUserData(chatId);
            var model = new SendMessageInput
            {
                Keypad = new KeypadModel
                {
                    Rows = new List<KeypadRowModel>()
                },
                ReplyTimeout = Variables.ReplyTimeout,
                ChatId = chatId
            };

            //start
            if (userDataEntity == null)
            {
                await _userDataService.Update(chatId, 1);

                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 1, new GroupEntity());
                model.Text = Messages.WelcomeMessage;
                return (model, false);
            }

            if (string.IsNullOrEmpty(buttonId))
            {
                var info = await _groupService.GetGroupInfo(chatId);
                if (info.IsFinished)
                {
                    model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 6, info);
                    model.Text = null;
                    return (model, true);
                }

                return await InvalidCommand(model, userDataEntity.StateId);
            }

            if (buttonId == "centerName")
            {
                var info = await _groupService.SetCenterName(chatId, messageText);
                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 1, info);
                model.Text = null;
                return (model, true);
            }

            if (buttonId == "centerType")
            {
                var centerType = messageText.GetCenterTypeEnumByValue();
                var info = await _groupService.SetCenterType(chatId, centerType.Value);
                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 1, info);
                model.Text = null;
                return (model, true);
            }

            if (buttonId == "gender")
            {
                var gender = messageText.GetGenderEnumByValue();
                var info = await _groupService.SetGenderType(chatId, gender.Value);
                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 1, info);
                model.Text = null;
                return (model, true);
            }

            if (buttonId == "province")
            {
                var province = await _provinceService.GetProvinceAsync(messageText);
                var info = await _groupService.SetProvince(chatId, province);
                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 1, info);
                model.Text = null;
                return (model, true);
            }

            if (buttonId == "address")
            {
                var info = await _groupService.SetAddress(chatId, messageText);
                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 1, info);
                model.Text = null;
                return (model, true);
            }

            if (buttonId == "postalCode")
            {
                GroupEntity info;
                if (messageText.IsValidPostalCode())
                {
                    info = await _groupService.SetPostalCode(chatId, messageText.PersianToEnglish());
                }
                else
                {
                    info = await _groupService.GetGroupInfo(model.ChatId);
                    model.Text = Messages.InvalidPostalCode;
                }
                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 1, info);
                return (model, true);
            }

            if (buttonId == "phone")
            {
                var info = await _groupService.SetPhone(chatId, messageText.PersianToEnglish());
                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 1, info);
                model.Text = null;
                return (model, true);
            }

            if (buttonId == "managerName")
            {
                var info = await _groupService.SetManagerName(chatId, messageText);
                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 1, info);
                model.Text = null;
                return (model, true);
            }

            if (buttonId == "1-continue")
            {
                var info = await _groupService.GetGroupInfo(chatId);
                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 2, info);
                model.Text = "اطلاعات مرکز آموزشی ثبت شد.";
                return (model, true);
            }

            if (buttonId == "supervisorName")
            {
                var info = await _groupService.SetSupervisorName(chatId, messageText);
                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 2, info);
                model.Text = null;
                return (model, true);
            }

            if (buttonId == "supervisorEmail")
            {
                GroupEntity info;

                if (messageText.IsValidEmail())
                {
                    info = await _groupService.SetSupervisorEmail(chatId, messageText);
                    model.Text = null;
                }
                else
                {
                    info = await _groupService.GetGroupInfo(chatId);
                    model.Text = Messages.InvalidEmail;
                }
                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 2, info);

                return (model, true);
            }

            if (buttonId == "supervisorMobile")
            {
                GroupEntity info;
                if (messageText.IsValidMobile())
                {
                    info = await _groupService.SetSupervisorMobile(chatId, messageText);
                    model.Text = null;
                }
                else
                {
                    info = await _groupService.GetGroupInfo(chatId);
                    model.Text = Messages.InvalidMobileNumber;
                }
                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 2, info);
                return (model, true);
            }

            if (buttonId == "2-continue")
            {
                var info = await _groupService.GetGroupInfo(chatId);
                model.Text = "اطلاعات دبیر راهنما ثبت شد.";
                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 3, info);
                return (model, true);
            }

            if (buttonId == "2-return")
            {
                var info = await _groupService.GetGroupInfo(chatId);
                model.Text = null;
                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 1, info);
                return (model, true);
            }

            if (buttonId == "subject")
            {
                var info = await _groupService.SetProjectSubject(chatId, messageText);
                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 3, info);
                model.Text = null;
                return (model, true);
            }

            if (buttonId == "ax")
            {
                var ax = await _festivalService.GetFestivalAxes(messageText);
                var info = await _groupService.SetFestivalAxes(chatId, ax);
                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 3, info);
                model.Text = null;
                return (model, true);
            }

            if (buttonId == "major")
            {
                var major = await _festivalService.GetFestivalMajor(messageText);
                var info = await _groupService.SetFestivalMajor(chatId, major);
                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 3, info);
                model.Text = null;
                return (model, true);
            }

            if (buttonId == "field")
            {
                var field = await _festivalService.GetFestivalField(messageText);
                var info = await _groupService.SetFestivalField(chatId, field);
                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 3, info);
                model.Text = null;
                return (model, true);
            }

            if (buttonId == "membersCount")
            {
                var info = await _groupService.SetMembersCount(chatId, Int32.Parse(messageText));
                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 3, info);
                model.Text = null;
                return (model, true);
            }

            if (buttonId == "3-continue")
            {
                var info = await _groupService.GetGroupInfo(chatId);
                model.Text = "اطلاعات پروژه ثبت شد.";

                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 4, info);
                return (model, true);
            }

            if (buttonId == "3-return")
            {
                var info = await _groupService.GetGroupInfo(chatId);
                model.Text = null;
                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 2, info);
                return (model, true);
            }

            if (buttonId == "grade")
            {
                var grade = EnumValue.GetGradeEnumByValue(messageText);
                var info = await _groupService.SetMembersGrade(chatId, grade.Value);
                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 4, info);
                model.Text = null;
                return (model, true);
            }

            if (buttonId.StartsWith("memberName"))
            {
                var info = await _groupService.GetGroupInfo(chatId);
                var number = int.Parse(buttonId.Replace("memberName", ""));

                await _groupService.SetMemberName(chatId, info.Id, number, messageText);

                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 4, info);
                return (model, true);
            }

            if (buttonId.StartsWith("memberNationalCode"))
            {
                var info = await _groupService.GetGroupInfo(chatId);
                var number = int.Parse(buttonId.Replace("memberNationalCode", ""));

                if (messageText.IsValidNationalCode())
                {
                    await _groupService.SetMemberNationalCode(chatId, info.Id, number, messageText);
                }
                else
                {
                    model.Text = Messages.InvalidNationalCode;
                }

                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 4, info);
                return (model, true);
            }

            if (buttonId == "4-continue")
            {
                var info = await _groupService.SetPrice(chatId);

                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 5, info);

                model.Text = GetGroupInfo(info);
                await _messengerService.SendMessage(model);
                model.Text = await GetMembersInfo(info);

                return (model, true);
            }

            if (buttonId == "4-return")
            {
                var info = await _groupService.GetGroupInfo(chatId);
                model.Text = null;
                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 3, info);
                return (model, true);
            }

            //Payment
            if (buttonId == "payment")
            {
                var groupInfo = await _groupService.GetGroupInfo(model.ChatId);

                if (myMessage.AuxData == null || string.IsNullOrEmpty(myMessage.AuxData.OrderId))
                {
                    model.Text = Messages.ErrorPay;
                    model.Keypad = await _keypadService.GenerateButtonsAsync(model.ChatId, 4, groupInfo);
                    return (model, true);
                }

                var orderId = int.Parse(myMessage.AuxData.OrderId);

                var order = await _orderService.UpdateOrderStatus(orderId, myMessage.AuxData.OrderStatus);

                if (order == null || myMessage.AuxData.OrderStatus == OrderStatusEnum.Error)
                {
                    model.Text = Messages.ErrorPay;
                    model.Keypad = await _keypadService.GenerateButtonsAsync(model.ChatId, 4, groupInfo);
                    return (model, true);
                }
                
                else
                {
                    var settle = await _messengerService.SettlePayment(model.ChatId, order.Id.ToString(), order.PaymentToken);

                    await _orderService.UpdateSettleStatus(orderId, settle.Data.SettleStatus);

                    if (settle.Data.SettleStatus == SettleStatusEnum.Reject)
                    {
                        model.Text = Messages.ErrorPay;
                        model.Keypad = await _keypadService.GenerateButtonsAsync(model.ChatId, 4, groupInfo);
                        return (model, true);
                    }
                    else //(settle.Data.SettleStatus == SettleStatusEnum.Done)
                    {
                        await _groupService.SetFinished(model.ChatId, orderId);
                        model.Text = Messages.SuccessRegister;
                        model.Keypad = await _keypadService.GenerateButtonsAsync(model.ChatId, 6, groupInfo);
                        return (model, false);
                    }
                }
            }

            if (buttonId == "5-return")
            {
                var info = await _groupService.GetGroupInfo(chatId);
                model.Text = null;
                model.Keypad = await _keypadService.GenerateButtonsAsync(chatId, 4, info);
                return (model, true);
            }

            return await InvalidCommand(model, userDataEntity.StateId);
        }

        private async Task<(SendMessageInput model, bool toast)> InvalidCommand(SendMessageInput model, long stateId, string text = null)
        {
            var info = await _groupService.GetGroupInfo(model.ChatId);
            model.Keypad = await _keypadService.GenerateButtonsAsync(model.ChatId, stateId, info);
            model.Text = text ?? Messages.InvalidCommand;
            return (model, true);
        }

        private string GetGroupInfo(GroupEntity info)
        {
            var message = $"اطلاعات ثبت نام شما در سیناکسپو: \r\n" +
                          $"مرکز آموزشی {info.CenterType.GetEnumValue()} " +
                          $"{info.CenterGenderType.GetEnumValue()} " +
                          $"به آدرس {info.Address} - " +
                          $"کدپستی {info.PostalCode} " +
                          $"با شماره تلفن {info.Phone} " +
                          $"و به مدیریت: {info.ManagerName} به عنوان مرکز آموزشی ثبت شد. \r\n" +
                          $"{info.SupervisorName} " +
                          $"با شماره همراه {info.SupervisorMobile} " +
                          $"و آدرس ایمیل {info.SupervisorEmail} به عنوان دبیر راهنما تعیین شد.\r\n" +
                          $"پروژه ی شما با موضوع {info.ProjectSubject} " +
                          $"در محور {info.AxTitle} " +
                          $"در رشته {info.FieldTitle} " +
                          $" با گرایش {info.MajorTitle} " +
                          $"با گروهی {info.MembersCount} نفره " +
                          $"در مقطع {info.MembersGrade.GetEnumValue()} ثبت شد.";
            return message;
        }

        private async Task<string> GetMembersInfo(GroupEntity info)
        {
            var message = "";
            var members = await _groupService.GetMembers(info.ChatId, info.Id);

            message = $"اعضای گروه:";

            foreach (var item in members)
            {
                message = message + $"\r\n{item.MemberNumber}- {item.Name} {item.NationalCode}";
            }
            return message;
        }

        #endregion

    }
}