using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBot.Models
{
    public class ActionHistoryModel
    {
        public long Id { get; set; }
        public long ChatId { get; set; }
        public long TaskId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ActionId { get; set; }
        public bool IsActive { get; set; }
        public string ActionValue { get; set; }=String.Empty;
    }
}