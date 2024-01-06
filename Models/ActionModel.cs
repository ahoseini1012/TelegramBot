using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBot.Models
{
    public class ActionModel
    {
        public long Id { get; set; }
        public string Name { get; set; }=String.Empty;
        public string values { get; set; }=String.Empty;
        public string description { get; set; }=String.Empty;
    }
}