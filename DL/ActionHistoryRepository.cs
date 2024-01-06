using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot;
using Dapper;
using MyBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MyBot.DL
{
    public class ActionHistoryRepository
    {
        public static async Task<ActionHistoryModel?> ActionHistory_GetOne(long chatId)
        {
            var connection = DBContext.CreateConnection();
            string queryString = $@"select top 1 h.id as id,h.ChatId,h.Taskid as TaskId,h.CreatedAt,h.IsActive,a.value  as actionvalue ,a.id as actionId
                                    from dbo.ActionHistory h
                                    join dbo.Actions a on a.id=h.ActionId
                                    where chatid={chatId} order by h.id desc";
            var dbResult = await connection.QueryAsync<ActionHistoryModel>(queryString);
            return dbResult.FirstOrDefault();
        }
        public static async Task<ActionHistoryModel?> InsertOne(long chatId,long TaskId, int ActionId)
        {
            var connection = DBContext.CreateConnection();
            string queryString = $@"
                insert into dbo.ActionHistory (ChatId,TaskId,CreatedAt,ActionId)
                output inserted.*
                values ({chatId},{TaskId},getdate(),{ActionId})
            ";
            var dbResult = await connection.QueryAsync<ActionHistoryModel>(queryString);
            return dbResult.FirstOrDefault();
        }

        public static async Task<ActionHistoryModel> UpdateActionHistoryActivation(long id)
        {
            var connection = DBContext.CreateConnection();
            string queryString = $@"
                update dbo.ActionHistory 
                set IsActive=0
                output inserted.*
                where id={id}
            ";
            var dbResult = await connection.QueryAsync<ActionHistoryModel>(queryString);
            return dbResult.FirstOrDefault()!;
        }
    }
}