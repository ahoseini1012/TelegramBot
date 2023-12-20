using System;
using System.Collections.Generic;

namespace RegisterBulk
{
    public class ApiResult<T>
    {
        public int? StatusCode { get; set; }
        public bool? IsSuccess { get; set; } 
        public string Message { get; set; } = String.Empty;
        public string MessageEn { get; set; } = String.Empty;
        public int? ErrorCode { get; set; }
        public List<string> ValidationErrors { get; set; }
        public T Data { get; set; }
    }

}