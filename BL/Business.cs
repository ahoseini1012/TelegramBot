using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramBot.DL;
using TelegramBot.Models;

namespace TelegramBot.BL
{
    public static class Business
    {
        public static async Task<int> KycState(long chatId)
        {
            var dbResult = await RepositoryKyc.UsersGetOne(chatId);
            if (dbResult.Any())
            {
                if (String.IsNullOrEmpty(dbResult.FirstOrDefault()!.PhoneNumber))
                {
                    return 1; // PhoneNumber valuee is needed
                }
                if (String.IsNullOrEmpty(dbResult.FirstOrDefault()!.FName))
                {
                    return 2; // FName valuee is needed 
                }
                if (String.IsNullOrEmpty(dbResult.FirstOrDefault()!.LName))
                {
                    return 3; // LName valuee is needed
                }
                if (dbResult.FirstOrDefault()!.IsCompleted)
                {
                    return 4; // profile is not completed
                }
                if (dbResult.FirstOrDefault()!.IsRegistered)
                {
                    return 5; // profile is not registered
                }
            }

            return -1; // new chatid
        }
    }
}