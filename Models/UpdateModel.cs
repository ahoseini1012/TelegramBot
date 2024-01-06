using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Bot.Models
{
    public static class UpdateModel
    {
        public static long ChatId { get; set; }
        public static string MessageText { get; set; } = String.Empty;
        public static Contact? Contact { get; set; }
        public static void GetUpdateModel(Update update)
        {
            if (update.Message != null)
            {
                ChatId = update.Message!.Chat.Id;
                MessageText = !String.IsNullOrEmpty(update.Message!.Text) ? update.Message!.Text : String.Empty;
                if (update.Message!.Contact != null)
                {
                    Contact = update.Message!.Contact;
                };
            }else if(update.CallbackQuery != null)
            {
                
            }
        }
    }
}