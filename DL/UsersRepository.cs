using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Bot.Models;

namespace Bot.DL
{
    public static class UsersRepository
    {
        public static async Task<List<MyBotUser>> GetOne(long chatId)
        {
            var connection = DBContext.CreateConnection();
            string queryString = $@"select * from users where chatid={chatId}";
            var dbResult = await connection.QueryAsync<MyBotUser>(queryString, commandType: CommandType.Text);
            return dbResult.ToList<MyBotUser>();
        }
    }
}