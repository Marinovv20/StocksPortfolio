using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using api.Dtos.Chat;
using api.Interfaces;
using Microsoft.Extensions.Configuration;

namespace api.Service
{
    public class ChatbotService : IChatbotService
    {
        private readonly IHttpClientFactory _clientFactory; 
        private readonly IFMPService _fmpService;
        private readonly string _apiKey;
        private readonly string _model;

        public ChatbotService(IHttpClientFactory clientFactory, IFMPService fmpService, IConfiguration config)
        {
            _clientFactory = clientFactory;
            _fmpService = fmpService;
            _apiKey = config["HuggingFace:ApiKey"] ?? string.Empty;
            _model = config["HuggingFace:Model"] ?? "meta-llama/Llama-3.2-3B-Instruct";
        }

        public async Task<ChatResponseDto> GetAdviceAsync(string symbol, string question)
        {
            string stockContext = "No additional financial data available.";
            decimal currentPrice = 0;

            try
            {
                var stock = await _fmpService.FindStockBySymbolAsync(symbol);
                if (stock != null)
                {
                    currentPrice = stock.Purchase; 
                    stockContext = $"Company: {stock.CompanyName}, Industry: {stock.Industry}, Current Price: ${stock.Purchase}, Market Cap: {stock.MarketCap}, Symbol: {stock.Symbol}";
                }
            }
            catch (Exception ex)
            {
                stockContext = $"Could not load real-time metrics due to: {ex.Message}";
            }

            string systemPrompt = 
                "You are an expert financial advisor AI. You analyze stock metrics and provide data in a strict JSON format.\n" +
                "You must respond ONLY with a raw JSON object matching this structure:\n" +
                "{\n" +
                "  \"message\": \"A short conversational greeting or summary answering the user's explicit question directly\",\n" +
                "  \"recommendation\": \"BUY, SELL, or HOLD\",\n" +
                "  \"currentPrice\": 0.00,\n" +
                "  \"reasoning\": \"A single string containing your explanation, using \\n for line breaks between points. NOT an array.\"\n" +
                "}\n" +
                "Do not include any markdown styling code-blocks (like ```json) or chat pleasantries outside of the JSON block.";

            string userPrompt = $"Context data for {symbol}:\n{stockContext}\n\nUser Question: {question}";

            string endpoint = "https://router.huggingface.co/v1/chat/completions";

            var openAiStylePayload = new
            {
                model = "Qwen/Qwen2.5-7B-Instruct:fastest",
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userPrompt }
                },
                max_tokens = 500,
                temperature = 0.7
            };

            var hfClient = _clientFactory.CreateClient("HuggingFaceClient");
            string jsonPayload = JsonSerializer.Serialize(openAiStylePayload);

            using var request = new HttpRequestMessage(HttpMethod.Post, new Uri(endpoint));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            try
            {
                var response = await hfClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    string errorBody = await response.Content.ReadAsStringAsync();
                    return CreateFallbackResponse(symbol, currentPrice, $"HF error {response.StatusCode}: {errorBody}");
                }

                string rawResponse = await response.Content.ReadAsStringAsync();
                using var jsonDoc = JsonDocument.Parse(rawResponse);
                
                string aiJsonOutput = jsonDoc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString()?.Trim() ?? string.Empty;

                if (aiJsonOutput.StartsWith("```"))
                {
                    aiJsonOutput = aiJsonOutput.Replace("```json", "").Replace("```", "").Trim();
                }

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var chatResponse = JsonSerializer.Deserialize<ChatResponseDto>(aiJsonOutput, options);

                if (chatResponse != null)
                {
                    if (chatResponse.CurrentPrice == 0) chatResponse.CurrentPrice = currentPrice;
                    return chatResponse;
                }
            }
            catch (Exception ex)
            {
                return CreateFallbackResponse(symbol, currentPrice, $"Failed to analyze context metrics: {ex.Message}");
            }

            return CreateFallbackResponse(symbol, currentPrice, "Failed to parse structured AI output tokens.");
        }

        private ChatResponseDto CreateFallbackResponse(string symbol, decimal price, string genericMessage)
        {
            return new ChatResponseDto
            {
                Message = $"Reviewing details for {symbol}.",
                Recommendation = "HOLD",
                CurrentPrice = price,
                Reasoning = genericMessage
            };
        }
    }
}