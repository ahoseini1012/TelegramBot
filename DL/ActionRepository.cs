using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot;
using Dapper;
using MyBot.Models;

namespace MyBot.DL
{
    public class ActionRepository
    {
        public static async Task<ActionModel?> Action_GetOne(long chatId)
        {
            var connection = DBContext.CreateConnection();
            string queryString = $@"select top 1 * from dbo.ActionHistory where chatid={chatId} order by id desc)";
            var dbResult = await connection.QueryAsync<ActionModel>(queryString);
            return dbResult.FirstOrDefault();
        }
    }
}