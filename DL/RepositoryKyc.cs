using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using TelegramBot.Models;

namespace TelegramBot.DL
{
    public static class RepositoryKyc
    {
        public static async Task<List<MyBotUser>> UsersGetOne(long chatId)
        {
            var connection = DBContext.CreateConnection();
            string queryString = $@"select * from users where chatid={chatId}";
            var dbResult = await connection.QueryAsync<MyBotUser>(queryString, commandType: CommandType.Text);
            return dbResult.ToList<MyBotUser>();
        }

        public static async Task<MyBotUser> InsertUser(MyBotUser user)
        {
            var connection = DBContext.CreateConnection();
            string queryString = $@"if not exists (select 1 from dbo.Users where ChatId={user.chatId})
                begin
                insert into dbo.users 
                (ChatId,PhoneNumber,Country) 
                output inserted.*
                values ({user.chatId},{user.PhoneNumber},N'{user.Country}')
                end
            else
                begin
                    SELECT TOP 1 * FROM [TelegramBot].[dbo].[Users] where ChatId={user.chatId}
                end
                                        ";
            var dbResult = await connection.QueryAsync<MyBotUser>(queryString);
            return dbResult.FirstOrDefault()!;
        }

        public static async Task<MyBotUser> UpdateUser(MyBotUser user)
        {
            var connection = DBContext.CreateConnection();
            string queryString = $@"update dbo.Users 
            set ";
            if (!String.IsNullOrEmpty(user.FName))
                queryString += $@"FName = N'{user.FName}' ";

            if (!String.IsNullOrEmpty(user.LName))
                queryString += $@"LName = N'{user.LName}' , IsCompleted=1,IsRegistered=1 ";

            queryString += $@"output inserted.* where ChatId = { user.chatId }";

            var dbResult = await connection.QueryAsync<MyBotUser>(queryString);
            return dbResult.FirstOrDefault()!;
        }
    }
}