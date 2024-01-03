using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exhibition_bot.Models
{
    public class MyBotUser
    {
        public int Id { get; set; }
        public string FName { get; set; } = String.Empty;
        public string LName { get; set; } = String.Empty;
        public string PhoneNumber { get; set; } = String.Empty;
        public long chatId { get; set; }
        public string Country { get; set; } = String.Empty;
        public int? CityId { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsRegistered { get; set; }
        public long RoleId { get; set; }
        public string RoleName { get; set; }=String.Empty;
    }
}