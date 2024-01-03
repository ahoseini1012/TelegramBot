using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using exhibition_bot.BL;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace exhibition_bot
{
    public class MyTelegram
    {
        public static void Bot()
        {
             var configuration = new ConfigurationBuilder()
                 .AddJsonFile($"appsettings.json");

            var config = configuration.Build();
            var connectionString = config.GetSection("DB:ConnectionString");
            var Client = config.GetSection("asnaClient");
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

            async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {
                try
                {

                    // Only process Message updates: https://core.telegram.org/bots/api#message
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

                        // if (update.Message.Text!=null && update.Message.Text!.ToLower()== "/start")
                        // {
                        //     await PrepareGeneralRespons.StartMenu(botClient, update, cancellationToken);
                        // }
                    }
                    // else if (update.CallbackQuery != null)
                    // {
                    //     // Task
                    //     switch (update.CallbackQuery.Data)
                    //     {
                    //         case "NewTasks":
                    //             await PrepareTasksRespons.SelectNewTasks(botClient, update, cancellationToken);
                    //             break;
                    //         case "DoingTasks":
                    //             break;
                    //         default:
                    //             break;
                    //     }
                    //     await PrepareTasksRespons.SelectTasksType(botClient, update, cancellationToken);
                    // }




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