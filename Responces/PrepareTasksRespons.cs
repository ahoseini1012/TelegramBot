using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.DL;
using TelegramBot.Models;
using TelegramBot.Helpers;

namespace TelegramBot
{
    public static class PrepareTasksRespons
    {
        static long chatId;
        static Contact? contact;
        static string FName = String.Empty;
        static string LName = String.Empty;
        static MobileCountryModel? mobileCountryModel;
        static string responseText = String.Empty;
        public static async Task ListTasks(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            UpdateModel.GetUpdateModel(update);
            chatId = UpdateModel.ChatId;

            var btn = InlineKeyboardButton.WithCallbackData("a","b");
            var mrkup = new InlineKeyboardMarkup(btn);
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "لطفا شماره موبایل خود را ارسال بفرمائید",
                parseMode: ParseMode.MarkdownV2,
                disableNotification: true,
                replyToMessageId: update.Message!.MessageId,
                replyMarkup: mrkup,
                cancellationToken: cancellationToken
            );
        }

    }
}