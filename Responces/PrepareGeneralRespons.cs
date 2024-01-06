using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Bot.DL;
using Bot.Models;
using Bot.Helpers;
using Newtonsoft.Json;

namespace Bot
{
    public static class PrepareGeneralRespons
    {
        static long chatId;
        static Contact? contact;
        static string FName = String.Empty;
        static string LName = String.Empty;
        static MobileCountryModel? mobileCountryModel;
        static string responseText = String.Empty;
        public static async Task StartMenu(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            UpdateModel.GetUpdateModel(update);
            chatId = UpdateModel.ChatId;
            List<MyBotUser> user = await AuthRepository.GetOneUser(chatId);
            List<List<InlineKeyboardButton>> list = new();
            if (user.Any())
            {
                List<InlineKeyboardButton> admin_btns = new()
                {
                    InlineKeyboardButton.WithCallbackData("Create New Task", $@"{chatId}_CreateNewTask"),
                    InlineKeyboardButton.WithCallbackData("Doing Tasks", "DoingTasks"),
                    InlineKeyboardButton.WithCallbackData("Done Tasks", "DoneTasks"),
                    InlineKeyboardButton.WithCallbackData("Rejected Tasks", "RejectedTasks")
                };
                List<InlineKeyboardButton> other_btns = new()
                {
                    InlineKeyboardButton.WithCallbackData("My New Tasks", "MyNewTasks"),
                    InlineKeyboardButton.WithCallbackData("My Doing Tasks", "MyDoingTasks")
                };

                if (user.FirstOrDefault()!.RoleId == 1) //is admin
                {
                    list.Add(admin_btns);
                }
                list.Add(other_btns);

                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "لطفا انتخاب نمایید",
                    parseMode: ParseMode.MarkdownV2,
                    disableNotification: true,
                    replyToMessageId: update.Message!.MessageId,
                    replyMarkup: new InlineKeyboardMarkup(list),
                    cancellationToken: cancellationToken
                );
            }

        }


    }
}