using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Chat
{
    public class ChatRequestDto
    {
        public string Symbol { get; set; } = string.Empty;
        public string Question { get; set; } = string.Empty;
    }
}