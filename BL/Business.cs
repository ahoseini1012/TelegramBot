using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using exhibition_bot.DL;
using exhibition_bot.Models;

namespace exhibition_bot.BL
{
    public static class KYC
    {
        public static async Task<int> State(long chatId)
        {
            var dbResult = await AuthRepository.UsersGetOne(chatId);
            if ( dbResult.Any())
            {
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
                return 1; // new chatid and PhoneNumber is needed
            

        }
    }
}