export interface ChatRequest {
  sessionId: string;
  message: string;
}

export interface ChatResponse {
  sessionId: string;
  reply: string;
  source: 'dialogflow' | 'generative-ai';
}

export interface ChatMessage {
  content: string;
  sender: 'user' | 'bot';
  source?: 'dialogflow' | 'generative-ai';
  timestamp: Date;
}