using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using exhibition_bot.DL;
using exhibition_bot.Models;
using exhibition_bot.Helpers;

namespace exhibition_bot
{
    public static class PrepareKycRespons
    {
        static long chatId;
        static Contact? contact;
        static string FName = String.Empty;
        static string LName = String.Empty;
        static MobileCountryModel? mobileCountryModel;
        static string responseText = String.Empty;

        public static async Task GetPhoneNumber(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            UpdateModel.GetUpdateModel(update);
            chatId = UpdateModel.ChatId;
            contact = update.Message!.Contact;

            if (contact != null)
            {
                mobileCountryModel = CheckMobileNumber.GetCountryByPhoneNumber(contact.PhoneNumber);
                var model = await AuthRepository.InsertUser(new MyBotUser
                {
                    chatId = chatId,
                    PhoneNumber = mobileCountryModel.MobileNumber,
                    Country = mobileCountryModel.CountryName
                });
                if (model != null)
                {
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: $@"
لطفا نام خود را وارد فرمائید:


شماره موبایل: {model.PhoneNumber}
نام: {model.FName} 
نام خانوادگی: {model.LName} 
کشور: {model.Country}
",
                        parseMode: ParseMode.MarkdownV2,
                        disableNotification: true,
                        replyToMessageId: update.Message!.MessageId,
                        replyMarkup: new ReplyKeyboardRemove(),
                        cancellationToken: cancellationToken
                    );
                }
                else
                {
                    throw new Exception("خطا در دریافت اطلاعات از دیتابیس");
                }
            }
            else
            {
                var btn = KeyboardButton.WithRequestContact("ارسال شماره موبایل");
                var mrkup = new ReplyKeyboardMarkup(btn);
                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: $@"
به دهمین نمایشگاه نهاده های کشاورزیAgricaltech
                    لطفا بازدن روی دگمه پایین شماره موبایل خود را ارسال بفرمائید",
                    parseMode: ParseMode.MarkdownV2,
                    disableNotification: true,
                    replyToMessageId: update.Message!.MessageId,
                    replyMarkup: mrkup,
                    cancellationToken: cancellationToken
                );
            }
        }

        public static async Task UpdateNames(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, int updateType)
        {
            UpdateModel.GetUpdateModel(update);
            chatId = UpdateModel.ChatId;
            if (updateType == 2)
            {
                FName = UpdateModel.MessageText;
                LName = String.Empty;
            }
            else if (updateType == 3)
            {
                FName = String.Empty;
                LName = UpdateModel.MessageText;
            }

            var model = await AuthRepository.UpdateUser(new MyBotUser { chatId = chatId, FName = FName, LName = LName });
            if (updateType == 2)
            {
                responseText = $@"
لطفا نام خانوادگی خود را وارد فرمائید

شماره موبایل: {model.PhoneNumber}
نام: {model.FName} 
نام خانوادگی: {model.LName} 
کشور: {model.Country}
";
            }
            if (updateType == 3)
            {
                responseText = $@"فرآیند ثبت نام شما باموفقیت به پایان رسید

                
شماره موبایل: {model.PhoneNumber}
نام: {model.FName} 
نام خانوادگی: {model.LName} 
کشور: {model.Country} 
                ";
            }
            await AsnaRepository.RegisterUser(model.PhoneNumber,model.FName,model.LName,model.Country);
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: responseText,
                parseMode: ParseMode.MarkdownV2,
                disableNotification: true,
                replyToMessageId: update.Message!.MessageId,
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken
            );
        }

    }
}