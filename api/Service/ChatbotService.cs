using api.Interfaces;
using api.Models;
using api.Dtos.Chat;

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

            var score = CalculateInvestmentScore(stock);
            var recommendation = DetermineRecommendation(score);
            var reasoning = GenerateReasoning(stock, score);

            return new ChatResponseDto
            {
                Symbol = symbol,
                Message = $"Analysis for {stock.CompanyName} ({symbol})",
                Recommendation = recommendation,
                CurrentPrice = stock.Purchase,
                Reasoning = reasoning
            };
        }

        private int CalculateInvestmentScore(Stock stock)
        {
            int score = 50;

            // Market Cap
            if (stock.MarketCap > 500_000_000_000)
                score += 25;
            else if (stock.MarketCap > 100_000_000_000)
                score += 20;
            else if (stock.MarketCap > 50_000_000_000)
                score += 15;
            else if (stock.MarketCap > 10_000_000_000)
                score += 10;
            else if (stock.MarketCap > 2_000_000_000)
                score += 5;
            else if (stock.MarketCap > 500_000_000)
                score -= 10;
            else if (stock.MarketCap > 100_000_000)
                score -= 15;
            else
                score -= 25;

            // Dividend
            if (stock.LastDiv > 0.50m)
                score += 15;
            else if (stock.LastDiv > 0.20m)
                score += 10;
            else if (stock.LastDiv > 0.01m)
                score += 5;
            else
                score -= 5;

            // Industry
            score += AnalyzeIndustry(stock.Industry);

            // Valuation
            score += AnalyzeValuation(stock);

            return Math.Max(0, Math.Min(100, score));
        }

        private int AnalyzeIndustry(string industry)
        {
            if (string.IsNullOrEmpty(industry))
                return 0;

            var ind = industry.ToLower();

            if (ind.Contains("utility") || ind.Contains("healthcare") || ind.Contains("pharmaceutical") || ind.Contains("consumer staples"))
                return 8;
            if (ind.Contains("technology") || ind.Contains("software") || ind.Contains("semiconductor") || ind.Contains("cloud"))
                return 5;
            if (ind.Contains("financial") || ind.Contains("banking") || ind.Contains("insurance"))
                return 3;
            if (ind.Contains("mining") || ind.Contains("oil") || ind.Contains("energy"))
                return -5;
            if (ind.Contains("crypto") || ind.Contains("gambling") || ind.Contains("penny stock"))
                return -10;

            return 0;
        }

        private int AnalyzeValuation(Stock stock)
        {
            if (stock.Purchase < 50 && stock.MarketCap > 5_000_000_000)
                return 5;
            if (stock.Purchase > 500 && stock.MarketCap < 10_000_000_000)
                return -5;
            return 0;
        }

        private string DetermineRecommendation(int score)
        {
            if (score >= 75)
                return "STRONG BUY 🚀";
            else if (score >= 60)
                return "BUY 📈";
            else if (score >= 40)
                return "HOLD ⏸️";
            else if (score >= 25)
                return "SELL 📉";
            else
                return "STRONG SELL ⛔";
        }

        private string GenerateReasoning(Stock stock, int score)
        {
            var factors = new List<string>();

            if (stock.MarketCap > 100_000_000_000)
                factors.Add("✅ Large market cap = stable company");
            else if (stock.MarketCap < 500_000_000)
                factors.Add("⚠️ Small market cap = higher risk");

            if (stock.LastDiv > 0.50m)
                factors.Add($"✅ Strong dividend yield ({stock.LastDiv:F2}) = good income");
            else if (stock.LastDiv > 0.01m)
                factors.Add($"📊 Dividend paid ({stock.LastDiv:F2}) = income stock");
            else
                factors.Add("📈 No dividend = likely a growth stock");

            if (stock.Industry.Contains("Technology", StringComparison.OrdinalIgnoreCase))
                factors.Add("🔬 Tech sector = growth potential but volatile");
            else if (stock.Industry.Contains("Healthcare", StringComparison.OrdinalIgnoreCase))
                factors.Add("🏥 Healthcare = defensive and stable");
            else if (stock.Industry.Contains("Utility", StringComparison.OrdinalIgnoreCase))
                factors.Add("⚡ Utility = low risk, steady income");

            if (stock.Purchase < 50)
                factors.Add("💰 Affordable price point = accessible for most");
            else if (stock.Purchase > 500)
                factors.Add("💎 Premium price = likely high quality");

            if (score >= 75)
                factors.Add("🎯 Overall: Excellent fundamentals detected!");
            else if (score >= 60)
                factors.Add("👍 Overall: Good investment opportunity");
            else if (score >= 40)
                factors.Add("⏸️ Overall: Maintain current position or monitor");
            else if (score >= 25)
                factors.Add("⚠️ Overall: Consider reducing exposure");
            else
                factors.Add("🚫 Overall: Significant risks identified");

            return string.Join("\n", factors);
        }
    }
}