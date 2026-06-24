import React, { useState, useRef, useEffect } from "react";
import { ChatMessage, ChatResponse } from "../../Models/Chat";
import { chatbotAskAPI } from "../../Services/ChatbotService";
import "./Chatbot.css";
import { v4 as uuid } from "uuid";

interface Props {
  stockSymbol: string;
}

const Chatbot: React.FC<Props> = ({ stockSymbol }) => {
  const [messages, setMessages] = useState<ChatMessage[]>([
    {
      id: uuid(),
      role: "bot",
      content: `Hi! I'm your stock advisor. Ask me about ${stockSymbol}. What would you like to know?`,
      timestamp: new Date(),
    },
  ]);
  const [input, setInput] = useState("");
  const [loading, setLoading] = useState(false);
  const messagesEndRef = useRef<HTMLDivElement>(null);

  const scrollToBottom = () => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
  };

  useEffect(() => {
    scrollToBottom();
  }, [messages]);

  const handleSendMessage = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!input.trim()) return;

    // 1. Instantly instantiate and register user's question locally
    const userMessage: ChatMessage = {
      id: uuid(),
      role: "user",
      content: input,
      timestamp: new Date(),
    };
    
    setMessages((prev) => [...prev, userMessage]);
    setInput("");
    setLoading(true);

    try {
      const response = await chatbotAskAPI(stockSymbol, input);
      setLoading(false);

      if (response) {
        const botMessage: ChatMessage = {
          id: uuid(),
          role: "bot",
          content: `${response.message}\n\n📊 Recommendation: ${response.recommendation}\n💰 Current Price: $${response.currentPrice}\n\nReasoning:\n${response.reasoning}`,
          timestamp: new Date(),
        };
        setMessages((prev) => [...prev, botMessage]);
      } else {
        setMessages((prev) => [
          ...prev,
          {
            id: uuid(),
            role: "bot",
            content: "Sorry, I received an empty analysis payload. Please verify your asset data ticker.",
            timestamp: new Date(),
          },
        ]);
      }
    } catch (error) {
      setLoading(false);
      const errorMessage: ChatMessage = {
        id: uuid(),
        role: "bot",
        content: "Sorry, I couldn't process your request right now. Please check your network connection.",
        timestamp: new Date(),
      };
      setMessages((prev) => [...prev, errorMessage]);
    }
  };

  return (
    <div className="chatbot-container">
      <div className="chatbot-header">
        <h3>Stock Advisor Bot</h3>
      </div>
      <div className="chatbot-messages">
        {messages.map((msg) => (
          <div
            key={msg.id}
            className={`chatbot-message ${msg.role}`}
          >
            {}
            <div className="message-content" style={{ whiteSpace: "pre-line" }}>
              {msg.content}
            </div>
            <small className="message-time">
              {msg.timestamp.toLocaleTimeString()}
            </small>
          </div>
        ))}
        {loading && (
          <div className="chatbot-message bot">
            <div className="message-content">Thinking...</div>
          </div>
        )}
        <div ref={messagesEndRef} />
      </div>
      <form onSubmit={handleSendMessage} className="chatbot-input-form">
        <input
          type="text"
          value={input}
          onChange={(e) => setInput(e.target.value)}
          placeholder="Ask about this stock..."
          disabled={loading}
          className="chatbot-input"
        />
        <button type="submit" disabled={loading} className="chatbot-send-btn">
          Send
        </button>
      </form>
    </div>
  );
};

export default Chatbot;