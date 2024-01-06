using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyBot
{
    public static class SendResponseMessage
    {
        public static async Task Send(ITelegramBotClient botClient, CancellationToken cancellationToken, InlineKeyboardMarkup? markup, long chatId, string text)
        {
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: text,
                parseMode: ParseMode.MarkdownV2,
                disableNotification: true,
                replyMarkup: markup,
                cancellationToken: cancellationToken
                );
        }

        public static async Task DeleteRecentMessage(ITelegramBotClient botClient, CancellationToken cancellationToken,int MessageId, long chatId)
        {
            await botClient.DeleteMessageAsync(chatId,MessageId);
        }


    }
}