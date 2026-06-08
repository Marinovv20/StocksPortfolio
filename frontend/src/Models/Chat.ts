export type ChatMessage = {
  id: string;
  role: "user" | "bot"; 
  content: string;
  timestamp: Date;
};

export type ChatRequest = {
  symbol: string;
  question: string;
};

export type ChatResponse = {
  symbol: string;
  message: string;
  recommendation: string; 
  currentPrice?: number;
  reasoning: string;
};