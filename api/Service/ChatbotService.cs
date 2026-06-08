using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Chat;
using api.Interfaces;

namespace api.Service
{
   public class ChatbotService : IChatbotService
    {
        private readonly IStockRepository _stockRepo;
        private readonly IFMPService _fmpService;

        public ChatbotService(IStockRepository stockRepo, IFMPService fmpService)
        {
            _stockRepo = stockRepo;
            _fmpService = fmpService;
        }

        public async Task<ChatResponseDto> GetAdviceAsync(string symbol, string question)
        {
            var stock = await _stockRepo.GetBySymbolAsync(symbol);
            
            if (stock == null)
            {
                stock = await _fmpService.FindStockBySymbolAsync(symbol);
            }

            if (stock == null)
            {
                return new ChatResponseDto
                {
                    Symbol = symbol,
                    Message = $"Stock {symbol} not found.",
                    Recommendation = "N/A",
                    Reasoning = "Stock not found in database"
                };
            }

            // Simple rule-based logic
            var recommendation = "HOLD";
            var reasoning = "";

            if (stock.MarketCap > 100000000000) // > 100B = large cap
            {
                recommendation = "BUY";
                reasoning = "Large market cap indicates stable company.";
            }
            else if (stock.MarketCap < 1000000000) // < 1B = small cap
            {
                recommendation = "SELL";
                reasoning = "Small market cap may indicate higher risk.";
            }
            else
            {
                recommendation = "HOLD";
                reasoning = "Mid-cap company - maintain current position.";
            }

            return new ChatResponseDto
            {
                Symbol = symbol,
                Message = $"Analysis for {stock.CompanyName} ({symbol})",
                Recommendation = recommendation,
                CurrentPrice = stock.Purchase,
                Reasoning = reasoning
            };
        }
    }
}