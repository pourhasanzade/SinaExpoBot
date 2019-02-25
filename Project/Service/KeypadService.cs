using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SinaExpoBot.API.Json.Input;
using SinaExpoBot.API.Json.Output;
using SinaExpoBot.Domain.Entity;
using SinaExpoBot.Domain.Enum;
using SinaExpoBot.Domain.Model;
using SinaExpoBot.Service.Interface;
using SinaExpoBot.Utility;

namespace SinaExpoBot.Service
{
    public class KeypadService : IKeypadService
    {
        private readonly IButtonService _buttonService;
        private readonly IUserDataService _userDataService;
        private readonly IMessengerService _messengerService;
        private readonly IGroupService _groupService;
        private readonly IOrderService _orderService;
        private readonly IApiService _apiService;
        private const MessageBehaviourTypeEnum BehaviourType = MessageBehaviourTypeEnum.Message;

        public KeypadService(IButtonService buttonService, IUserDataService userDataService,
            IMessengerService messengerService, IGroupService groupService,
            IOrderService orderService, IApiService apiService)
        {
            _buttonService = buttonService;
            _userDataService = userDataService;
            _messengerService = messengerService;
            _groupService = groupService;
            _orderService = orderService;
            _apiService = apiService;
        }

        public async Task<KeypadModel> GenerateButtonsAsync(string chatId, long stateId, object myObject = null)
        {
            await _userDataService.Update(chatId, stateId);

            if (stateId == 6 ) return await GetKeypadAsync(stateId);

            return await FixButtonsAsync(stateId, myObject);
        }

        private async Task<KeypadModel> GetKeypadAsync(long stateId)
        {
            var buttonList = await _buttonService.GetButtonList(stateId);
            buttonList = buttonList.OrderBy(x => x.Row).ThenBy(x => x.Order).ToList();

            var keypad = new KeypadModel() { Rows = new List<KeypadRowModel>() };
            foreach (var button in buttonList)
            {
                if (keypad.Rows.Count < button.Row)
                {
                    keypad.Rows.Add(new KeypadRowModel());
                    keypad.Rows[button.Row - 1].Buttons = new List<ButtonModel>();
                }

                var newButton = new ButtonModel
                {
                    Id = button.Code,
                    Type = button.Type,
                    ButtonView = new ButtonSimpleModel
                    {
                        Type = button.ViewType,
                        Text = button.Text,
                        ImageUrl = button.ImageUrl
                    },
                    BehaviourType = button.BehaviourType
                };

                if (button.Type == ButtonTypeEnum.Selection)
                {
                    newButton.ButtonSelection = JsonConvert.DeserializeObject<ButtonSelectionModel>(button.Data);
                }
                else if (button.Type == ButtonTypeEnum.Calendar)
                {
                    newButton.ButtonCalendar = JsonConvert.DeserializeObject<ButtonCalendarModel>(button.Data);
                    var pc = new PersianCalendar();
                    var now = $"{pc.GetYear(DateTime.Now)}-{pc.GetMonth(DateTime.Now)}-{pc.GetDayOfMonth(DateTime.Now)}";
                    newButton.ButtonCalendar.DefaultValue = now;
                }
                else if (button.Type == ButtonTypeEnum.NumberPicker)
                {
                    newButton.ButtonNumberPicker = JsonConvert.DeserializeObject<ButtonNumberPickerModel>(button.Data);
                }
                else if (button.Type == ButtonTypeEnum.StringPicker)
                {
                    newButton.ButtonStringPicker = JsonConvert.DeserializeObject<ButtonStringPickerModel>(button.Data);
                }
                else if (button.Type == ButtonTypeEnum.Location)
                {
                    newButton.ButtonLocation = JsonConvert.DeserializeObject<ButtonLocationModel>(button.Data);
                }
                else if (button.Type == ButtonTypeEnum.Alert)
                {
                    newButton.ButtonAlert = JsonConvert.DeserializeObject<ButtonAlertModel>(button.Data);
                }
                else if (button.Type == ButtonTypeEnum.Textbox)
                {
                    newButton.ButtonTextBox = JsonConvert.DeserializeObject<ButtonTextBoxModel>(button.Data);
                }
                else if (button.Type == ButtonTypeEnum.Payment)
                {
                    // handled in another method 
                }
                else if (button.Type == ButtonTypeEnum.Call)
                {
                    newButton.ButtonCall = JsonConvert.DeserializeObject<ButtonCallModel>(button.Data);
                }

                keypad.Rows[button.Row - 1].Buttons.Add(newButton);
            }

            return keypad;
        }

        private async Task<KeypadModel> FixButtonsAsync(long stateId, object myObject)
        {
            var keypad = new KeypadModel();

            if (stateId == 1)
            {
                var isCompleted = true;

                keypad = await GetKeypadAsync(1);

                var info = (GroupEntity)myObject;

                if (!string.IsNullOrEmpty(info.CenterName)) { SetImage(keypad, "centerName"); } else isCompleted = false;

                if (info.CenterType.HasValue)
                {
                    SetImage(keypad, "centerType");
                    SetDefaultValue(keypad, "centerType", info.CenterType.GetEnumValue());
                } else isCompleted = false;

                if (info.CenterGenderType.HasValue)
                {
                    SetImage(keypad, "gender");
                    SetDefaultValue(keypad, "gender", info.CenterGenderType.GetEnumValue());
                }
                else isCompleted = false;
                if (!string.IsNullOrEmpty(info.ProvinceName)) { SetImage(keypad, "province"); } else isCompleted = false;
                if (!string.IsNullOrEmpty(info.Address)) { SetImage(keypad, "address"); } else isCompleted = false;
                if (!string.IsNullOrEmpty(info.PostalCode)) { SetImage(keypad, "postalCode"); } else isCompleted = false;
                if (!string.IsNullOrEmpty(info.Phone)) { SetImage(keypad, "phone"); } else isCompleted = false;
                if (!string.IsNullOrEmpty(info.ManagerName)) { SetImage(keypad, "managerName"); } else isCompleted = false;

                if (!isCompleted) SetAlert(keypad, "1-continue");
            }

            if (stateId == 2)
            {
                var isCompleted = true;

                keypad = await GetKeypadAsync(2);

                var info = (GroupEntity)myObject;

                if (!string.IsNullOrEmpty(info.SupervisorName)) { SetImage(keypad, "supervisorName"); } else isCompleted = false;
                if (!string.IsNullOrEmpty(info.SupervisorMobile)) { SetImage(keypad, "supervisorMobile"); } else isCompleted = false;
                if (!string.IsNullOrEmpty(info.SupervisorEmail)) { SetImage(keypad, "supervisorEmail"); } else isCompleted = false;

                if (!isCompleted) SetAlert(keypad, "2-continue");
            }

            if (stateId == 3)
            {
                var isCompleted = true;

                keypad = await GetKeypadAsync(3);

                var info = (GroupEntity)myObject;

                if (!string.IsNullOrEmpty(info.ProjectSubject)) { SetImage(keypad, "subject"); } else isCompleted = false;
                if (!string.IsNullOrEmpty(info.AxTitle))
                {
                    SetImage(keypad, "axes");
                }
                else
                {
                    isCompleted = false;
                    SetAlert(keypad, "field", Messages.AxRequired);
                }

                if (!string.IsNullOrEmpty(info.FieldTitle))
                {
                    SetImage(keypad, "field");
                }
                else
                {
                    isCompleted = false;
                    SetAlert(keypad, "major", Messages.FieldRequired);
                }

                if (!string.IsNullOrEmpty(info.MajorTitle)) { SetImage(keypad, "major"); } else isCompleted = false;

                if (info.MembersCount.HasValue)
                {
                    SetImage(keypad, "membersCount");
                    SetDefaultValue(keypad, "membersCount", info.MembersCount + "");
                } else isCompleted = false;

                if (!isCompleted) SetAlert(keypad, "3-continue");                
            }

            if (stateId == 4)
            {
                var info = (GroupEntity)myObject;
                keypad = new KeypadModel { Rows = new List<KeypadRowModel>() };
                var members = await _groupService.GetMembers(info.ChatId, info.Id);

                await AddExtraKeypadAsync(keypad, 40);

                for (int i = 1; i <= info.MembersCount; i = i + 1)
                {
                    var row = new KeypadRowModel { Buttons = new List<ButtonModel>() };
                    

                    var currentMember = members.FirstOrDefault(x => x.MemberNumber == i);
                    var button1 = new ButtonModel
                    {
                        Id = "memberName" + i,
                        Type = ButtonTypeEnum.Textbox,
                        BehaviourType = BehaviourType,
                        ButtonTextBox = new ButtonTextBoxModel
                        {
                            LineType = ButttenTextBoxLineEnum.SingleLine,
                            KeypadType = ButttenTextBoxKeypadEnum.String,

                        },
                        ButtonView = new ButtonSimpleModel
                        {
                            Type = ButtonSimpleTypeEnum.TextOnly,
                            ImageUrl = null,
                            Text = (currentMember != null && !string.IsNullOrEmpty(currentMember.Name)) ? currentMember.Name : "نام و نام خانوادگی عضو " + i,
                        }
                    };

                    row.Buttons.Add(button1);
                    var button2 = new ButtonModel
                    {
                        Id = "memberNationalCode" + i,
                        Type = ButtonTypeEnum.Textbox,
                        BehaviourType = BehaviourType,
                        ButtonTextBox = new ButtonTextBoxModel
                        {
                            LineType = ButttenTextBoxLineEnum.SingleLine,
                            KeypadType =  ButttenTextBoxKeypadEnum.Number,

                        },
                        ButtonView = new ButtonSimpleModel
                        {
                            Type = ButtonSimpleTypeEnum.TextOnly,
                            ImageUrl = null,
                            Text = (currentMember != null && !string.IsNullOrEmpty(currentMember.NationalCode)) ? currentMember.NationalCode : "کد ملی عضو " + i,
                        }
                    };
                    
                    row.Buttons.Add(button2);
                    keypad.Rows.Add(row);

                    if (currentMember != null && !string.IsNullOrEmpty(currentMember.Name)) SetImage(keypad, button1.Id);
                    if (currentMember != null && !string.IsNullOrEmpty(currentMember.NationalCode)) SetImage(keypad, button2.Id);
                }

                await AddExtraKeypadAsync(keypad, 4);

                if (info.MembersGrade.HasValue)
                {
                    SetImage(keypad, "grade");
                    SetDefaultValue(keypad, "grade", info.MembersGrade.GetEnumValue());
                }

                if (!info.MembersGrade.HasValue || members.Count()<info.MembersCount || members.Any(x=>string.IsNullOrEmpty(x.NationalCode)|| string.IsNullOrEmpty(x.Name)))
                {
                    SetAlert(keypad, "4-continue");
                }
            }

            if (stateId == 5)
            {
                keypad = await GetKeypadAsync(5);            
                var info = (GroupEntity)myObject;
                var order = await _orderService.AddPayment(info.ChatId, PaymentTypeEnum.Payment, info.Price, Variables.PlanId);
                var paymentRequestInput = new PaymentRequestInput
                {
                    ChatId = info.ChatId,
                    OrderId = order.Id.ToString(),
                    Type = PaymentTypeEnum.Payment,
                    Options = new PaymentOptions
                    {
                        Amount = info.Price,
                        PlanId = "" + Variables.PlanId,
                    }
                };
                var output = await _messengerService.PaymentRequest(paymentRequestInput);

                await _orderService.UpdatePaymentToken(order.Id, output.Data.PaymentToken);

                var button = GetButton(keypad, "payment");
                button.ButtonPayment = new ButtonPayment { ButtonPaymentToken = output.Data.PaymentToken };
            }

            return keypad;
        }


        private async Task AddExtraKeypadAsync(KeypadModel keypad, long stateId)
        {
            var extraKeypad = await GetKeypadAsync(stateId);
            foreach (var row in extraKeypad.Rows)
            {
                keypad.Rows.Add(row);
            }
        }

        private void SetImage(KeypadModel keypad, string buttonCode)
        {
            var button = GetButton(keypad, buttonCode);
            if (button == null) return;

            button.ButtonView.ImageUrl = Variables.DoneImageUrl;
            button.ButtonView.Type = ButtonSimpleTypeEnum.TextImgThu;
        }

        private void SetAlert(KeypadModel keypad, string buttonCode, string text = null)
        {
            var button = GetButton(keypad, buttonCode);
            if (button == null) return;

            button.Type = ButtonTypeEnum.Alert;
            button.ButtonAlert = new ButtonAlertModel { Message = text ?? Messages.Required };
        }

        private void SetDefaultValue(KeypadModel keypad, string buttonCode, string input)
        {
            var button = GetButton(keypad, buttonCode);
            if (button == null) return;

            if (button.Type == ButtonTypeEnum.Calendar)
                button.ButtonCalendar.DefaultValue = input;
            else if (button.Type == ButtonTypeEnum.Calendar)
                button.ButtonCalendar.DefaultValue = input;
            else if (button.Type == ButtonTypeEnum.StringPicker)
                button.ButtonStringPicker.DefaultValue = input;
            else if (button.Type == ButtonTypeEnum.NumberPicker)
                button.ButtonNumberPicker.DefaultValue = input;
        }


        private ButtonModel GetButton(KeypadModel keypad, string buttonCode)
        {
            return keypad.Rows.SelectMany(x => x.Buttons).ToList().FirstOrDefault(x => x.Id == buttonCode);
        }

    }
}