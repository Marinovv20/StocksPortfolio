import axios from "axios";
import { handleError } from "../Helpers/ErrorHandler";
import { ChatRequest, ChatResponse } from "../Models/Chat";

const api = "http://localhost:5036/api/chatbot/";

export const chatbotAskAPI = async (
  symbol: string,
  question: string
): Promise<ChatResponse | undefined> => {
  try {
    const request: ChatRequest = { symbol, question };
    const response = await axios.post<ChatResponse>(api + "ask", request);
    return response.data;
  } catch (error) {
    handleError(error);
    return undefined;
  }
};