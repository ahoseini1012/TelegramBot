using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.Extensions.Configuration;
using TelegramBot.BL;
using TelegramBot.Models;
namespace TelegramBot
{

    internal class Program
    {
        private static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                 .AddJsonFile($"appsettings.json");

            var config = configuration.Build();
            var connectionString = config.GetSection("DB:ConnectionString");
            var Client = config.GetSection("botClient");
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
                        int KycState = await Business.KycState(chatId);
                        switch (KycState)
                        {
                            case 1:
                            case -1:
                                await PrepareKycRespons.GetPhoneNumber(botClient, update, cancellationToken);
                                break;
                            case 2:
                                await PrepareKycRespons.UpdateNames(botClient, update, cancellationToken, 2);
                                break;
                            case 3:
                                await PrepareKycRespons.UpdateNames(botClient, update, cancellationToken, 3);
                                break;
                            default:
                                await PrepareTasksRespons.ListTasks(botClient, update, cancellationToken);
                                break;
                        }
                    }
                    else if(update.CallbackQuery != null )
                    {
                        // Task
                        await PrepareTasksRespons.ListTasks(botClient, update, cancellationToken);
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