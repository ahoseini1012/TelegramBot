using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TelegramBot.Models
{
    public class Tasks
    {
        public long Id { get; set; }
        public long ChatId { get; set; }
        public string PhoneNumber { get; set; } = String.Empty;
        public DateTime CreatedAt { get; set; }
        public string Title { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public string VoicePath { get; set; } = String.Empty;
        public string VideoPath { get; set; } = String.Empty;
        public int Status { get; set; }
    }
}