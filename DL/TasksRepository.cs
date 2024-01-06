using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Bot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using MyBot.Models;

namespace Bot.DL
{
    public static class TasksRepository
    {
        public static async Task<TodoTask> InsertOne(long ChatId)
        {
            var connection = DBContext.CreateConnection();
            string queryString = $@"
                  insert into dbo.Tasks
                (ChatId,CreatedAt)
                output inserted.*
                values
                ({ChatId},GETDATE())
            ";
            var dbResult = await connection.QueryAsync<TodoTask>(queryString);
            return dbResult.FirstOrDefault()!;
        }
        public static async Task<TodoTask> GetOne(long ChatId, int type)
        {
            var connection = DBContext.CreateConnection();
            string queryString = $@"
                select * from tasks where Status={type} and ChatId={ChatId}
            ";
            var dbResult = await connection.QueryAsync<TodoTask>(queryString);
            return dbResult.FirstOrDefault()!;
        }

        public static async Task<TodoTask> UpdateTaskTitle(string title, long taskId)
        {
            var connection = DBContext.CreateConnection();
            string queryString = $@"
                update dbo.tasks 
                set title=N'{title}'
                output inserted.*
                where id={taskId}
            ";
            var dbResult = await connection.QueryAsync<TodoTask>(queryString);
            return dbResult.FirstOrDefault()!;
        }

        
    }
}