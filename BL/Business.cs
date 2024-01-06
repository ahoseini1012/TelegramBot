using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.DL;
using Bot.Models;

namespace Bot.BL
{
    public static class KYC
    {
        public static async Task<int> State(long chatId)
        {
            var dbResult = await AuthRepository.GetOneUser(chatId);
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

    public static class ActionBased
    {
        public static async Task GetLastAction()
        {

        }
    }
}