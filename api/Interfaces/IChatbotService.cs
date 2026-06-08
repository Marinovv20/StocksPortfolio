using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Chat;

namespace api.Interfaces
{
    public interface IChatbotService
    {
        Task<ChatResponseDto> GetAdviceAsync(string symbol, string question);
    }
}