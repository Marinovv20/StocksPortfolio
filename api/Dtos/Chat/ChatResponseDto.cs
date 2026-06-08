using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Chat
{
    public class ChatResponseDto
    {
        public string Symbol { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Recommendation { get; set; } = string.Empty; 
        public decimal? CurrentPrice { get; set; }
        public string Reasoning { get; set; } = string.Empty;
    }
}