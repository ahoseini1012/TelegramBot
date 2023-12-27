using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using TelegramBot.Models;

namespace TelegramBot.DL
{
    public static class RepositoryTasks
    {
        public static async Task<List<MyBotUser>> UsersGetOne(long chatId)
        {
            var connection = DBContext.CreateConnection();
            string queryString = $@"select * from users where chatid={chatId}";
            var dbResult = await connection.QueryAsync<MyBotUser>(queryString, commandType: CommandType.Text);
            return dbResult.ToList<MyBotUser>();
        }

        public static async Task<Tasks> GetTasks(long chatId,int type)
        {
            var connection = DBContext.CreateConnection();
            string queryString = $@"
                select * from tasks where Status={type}
            ";
            var dbResult = await connection.QueryAsync<Tasks>(queryString);
            return dbResult.FirstOrDefault()!;
        }
    }
}