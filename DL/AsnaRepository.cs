using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace exhibition_bot.DL
{
    public class AsnaRepository
    {
        public static async Task RegisterUser(string mobile,string fname,string lname,string country)
        {
            var connection = DBContext.CreateConnection();
            string queryString = $@"if not exists(select 1 from dbo.attendees where MobileNo =N'0'+'{mobile}')
                                    begin
                                    insert into [dbo].[Attendees] (AttendeeId,FName,LName,Country,MobileNo)
                                    output inserted.*
                                    values(newid(),N'{fname}',N'{lname}',N'{country}',N'0'+'{mobile}')
                                    end
            ";
            var dbResult = await connection.QueryAsync(queryString, commandType: CommandType.Text);
        }
    }
}