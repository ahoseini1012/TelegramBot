using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.BL;
using Bot.DL;
using Bot.Models;
using Microsoft.Extensions.Configuration;
using MyBot;
using MyBot.DL;
using MyBot.Models;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot
{
    public class MyTelegram
    {
        public static void Bot()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json");

            var config = configuration.Build();
            var connectionString = config.GetSection("DB:ConnectionString");
            var Client = config.GetSection("todoClient");
            var botClient = new TelegramBotClient(Client.Value!);

            using CancellationTokenSource cts = new();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
            };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );
            System.Console.ReadLine();
            Console.WriteLine("MyBot is running");

            async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {
                try
                {
                    ///////////////////////////////////////
                    // Only process Text messages
                    ///////////////////////////////////////
                    if (update.Message != null)
                    {

                        // KYC                
                        long chatId = update.Message.Chat.Id;
                        int KycState = await KYC.State(chatId);
                        switch (KycState)
                        {
                            case 1:
                            case -1:
                                await PrepareKycRespons.GetPhoneNumber(botClient, update, cancellationToken);
                                return;
                            case 2:
                                await PrepareKycRespons.UpdateNames(botClient, update, cancellationToken, 2);
                                return;
                            case 3:
                                await PrepareKycRespons.UpdateNames(botClient, update, cancellationToken, 3);
                                return;
                        }



                        ///////////////////////////////////////
                        // start to tasks
                        ///////////////////////////////////////
                        if (update.Message.Text != null && update.Message.Text!.ToLower() == "/start")
                        {
                            await PrepareGeneralRespons.StartMenu(botClient, update, cancellationToken);
                            return;
                        }
                        else
                        {
                            // check last action of user when sending text
                            ActionHistoryModel? lastAction = await ActionHistoryRepository.ActionHistory_GetOne(chatId);
                            if (lastAction != null)
                            {
                                if (lastAction!.IsActive)
                                {
                                    switch (lastAction.ActionId)
                                    {
                                        case 1:
                                            //حذف پیام قبلی
                                            await SendResponseMessage.DeleteRecentMessage(botClient,cancellationToken,update.Message.MessageId,chatId);
                                            await SendResponseMessage.DeleteRecentMessage(botClient,cancellationToken,update.Message.MessageId-1,chatId);
                                            // عنوان تسک به روز میشود
                                            await TasksRepository.UpdateTaskTitle(update.Message.Text!,lastAction.TaskId);

                                            // اکشن غیرفعال میشود
                                            await ActionHistoryRepository.UpdateActionHistoryActivation(lastAction.Id);

                                            // اطلاعرسانی جهت به روز شدن عنوان تسک
                                            await SendResponseMessage.Send(botClient, cancellationToken,null,lastAction.ChatId ,"عملیات با موفقیت انجام شد");
                                            return;

                                        default:
                                        return;
                                    }

                                }
                                else
                                {
                                    return;
                                }
                            }
                        }
                    }
                    ///////////////////////////////////////
                    // only porcess InlineKeyboard messages
                    ///////////////////////////////////////
                    else if (update.CallbackQuery != null)
                    {
                        long chid = -1;
                        long tskid = -1;
                        // Task
                        if (update.CallbackQuery.Data!.Contains("CreateNewTask"))
                        {
                            chid = Convert.ToInt64(update.CallbackQuery.Data.Split('_')[0]);
                            await PrepareTasksRespons.CreateNewTasks(botClient, update, cancellationToken, chid);
                            return;
                        }
                        else if (update.CallbackQuery.Data!.Contains("Title"))
                        {
                            chid = Convert.ToInt64(update.CallbackQuery.Data.Split('_')[0]);
                            tskid = Convert.ToInt64(update.CallbackQuery.Data.Split('_')[1]);
                            await PrepareTasksRespons.ActionHistory_UpdateTaskTitle(botClient, cancellationToken, update,chid,tskid, 1);
                        }
                        else if (update.CallbackQuery.Data!.Contains("Description"))
                        {
                            //await PrepareTasksRespons.Action_UpdateTaskTitle(botClient,cancellationToken, update, Convert.ToInt64(update.CallbackQuery.Data.Split('_')[0]),update.CallbackQuery.Data.Split('_')[1]);
                        }
                        else if (update.CallbackQuery.Data!.Contains("Assign"))
                        {
                            //await PrepareTasksRespons.Action_UpdateTaskTitle(botClient,cancellationToken, update, Convert.ToInt64(update.CallbackQuery.Data.Split('_')[0]),update.CallbackQuery.Data.Split('_')[1]);
                        }
                        else if (update.CallbackQuery.Data!.Contains("Up2Date"))
                        {
                            // await PrepareTasksRespons.Action_UpdateTaskTitle(botClient,cancellationToken, update, Convert.ToInt64(update.CallbackQuery.Data.Split('_')[0]),update.CallbackQuery.Data.Split('_')[1]);
                        }

                        await PrepareTasksRespons.SelectTasksType(botClient, update, cancellationToken);
                    }

                }
                catch (System.Exception exception)
                {
                    var ErrorMessage = exception switch
                    {
                        ApiRequestException apiRequestException
                            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                        _ => exception.ToString()
                    };

                    Console.WriteLine(ErrorMessage);
                }

            }

            Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
            {
                var ErrorMessage = exception switch
                {
                    ApiRequestException apiRequestException
                        => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };

                Console.WriteLine(ErrorMessage);
                return Task.CompletedTask;
            }
        }
    }
}