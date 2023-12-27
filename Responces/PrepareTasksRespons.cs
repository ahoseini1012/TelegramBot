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
        public static async Task SelectTasksType(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            UpdateModel.GetUpdateModel(update);
            chatId = UpdateModel.ChatId;
            List<InlineKeyboardButton> btns = new();
            btns.Add(InlineKeyboardButton.WithCallbackData("New Tasks", "NewTasks"));
            btns.Add(InlineKeyboardButton.WithCallbackData("Doing Tasks", "Doing Tasks"));
            var mrkup = new InlineKeyboardMarkup(btns);
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "لطفا انتخاب نمایید",
                parseMode: ParseMode.MarkdownV2,
                disableNotification: true,
                replyToMessageId: update.Message!.MessageId,
                replyMarkup: mrkup,
                cancellationToken: cancellationToken
            );
        }
        public static async Task ShowTasksList(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            UpdateModel.GetUpdateModel(update);
            chatId = UpdateModel.ChatId;
            List<InlineKeyboardButton> btns = new()
            {
                InlineKeyboardButton.WithCallbackData("New Tasks", "NewTasks"),
                InlineKeyboardButton.WithCallbackData("Doing Tasks", "DoingTasks")
            };
            var mrkup = new InlineKeyboardMarkup(btns);
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "لطفا انتخاب نمایید",
                parseMode: ParseMode.MarkdownV2,
                disableNotification: true,
                replyToMessageId: update.Message!.MessageId,
                replyMarkup: mrkup,
                cancellationToken: cancellationToken
            );
        }

        internal static Task SelectNewTasks(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            UpdateModel.GetUpdateModel(update);
            chatId = UpdateModel.ChatId;
            RepositoryTasks.GetTasks(chatId,1);
        }
    }
}