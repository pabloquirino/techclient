import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ChatMessage, ChatRequest, ChatResponse } from '../models/chat.model';

@Injectable({ providedIn: 'root' })
export class ChatService {
  private readonly apiUrl = `${environment.apiUrl}/chat`;

  messages = signal<ChatMessage[]>([]);
  isLoading = signal(false);
  sessionId = signal(crypto.randomUUID());

  constructor(private http: HttpClient) {}

  async sendMessage(content: string): Promise<void> {
    this.addMessage({ content, sender: 'user', timestamp: new Date() });
    this.isLoading.set(true);

    try {
      const request: ChatRequest = {
        sessionId: this.sessionId(),
        message: content
      };

      const response = await firstValueFrom(
        this.http.post<ChatResponse>(`${this.apiUrl}/message`, request)
      );

      this.addMessage({
        content: response.reply,
        sender: 'bot',
        source: response.source,
        timestamp: new Date()
      });
    } catch {
      this.addMessage({
        content: 'Desculpe, ocorreu um erro. Tente novamente.',
        sender: 'bot',
        timestamp: new Date()
      });
    } finally {
      this.isLoading.set(false);
    }
  }

  private addMessage(message: ChatMessage): void {
    this.messages.update(messages => [...messages, message]);
  }
}