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
using MyBot;
using MyBot.DL;

namespace Bot
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
                InlineKeyboardButton.WithCallbackData("ثبت نام", "NewTasks"),
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

        public static async Task SelectNewTasks(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            UpdateModel.GetUpdateModel(update);
            chatId = UpdateModel.ChatId;
            await TasksRepository.GetOne(chatId, 1);
        }
        public static async Task<TodoTask> CreateNewTasks(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, long chatId)
        {
            TodoTask todoTask = await TasksRepository.InsertOne(chatId);

            List<List<InlineKeyboardButton>> btns = new()
            {
                new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Title", $"{chatId}_{todoTask.Id}_Title") },
                new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Description", $"{chatId}_{todoTask.Id}_Description") },
                new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("AssginTo", $"{chatId}_{todoTask.Id}_Assgin") },
                new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Up2Date", $"{chatId}_{todoTask.Id}_Update") }
            };
            var mrkup = new InlineKeyboardMarkup(btns);

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $@"تسک جدید ایجاد شد:
                id: {todoTask.Id}
                Title: {todoTask.Title}
                Description: {todoTask.Description}
                Created At : {todoTask.CreatedAt}
                MobileNumebt: {todoTask.ChatId}
                ",
                parseMode: ParseMode.MarkdownV2,
                disableNotification: true,
                replyMarkup: mrkup,
                cancellationToken: cancellationToken
                );
            return todoTask;
        }

        //ذخیره اکشن و 
        public static async Task ActionHistory_UpdateTaskTitle(ITelegramBotClient botClient, CancellationToken cancellationToken, Update update, long chatId, long taskId, int data)
        {
            var todoTask = await ActionHistoryRepository.InsertOne(chatId, taskId, data);
            if (todoTask != null)
            {
                string txt = $@"لطفا عنوان جدید برای تسک {taskId} وارد نمایید";
                await SendResponseMessage.Send(botClient, cancellationToken, null, chatId, txt);
            }
            else
            {
                throw new System.Exception("Error on insert action history - updatetitle");
            }
        }
    }
}