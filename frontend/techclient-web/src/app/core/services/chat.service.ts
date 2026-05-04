import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpErrorResponse } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ChatMessage, ChatRequest, ChatResponse } from '../models/chat.model';

@Injectable({ providedIn: 'root' })
export class ChatService {
  private readonly apiUrl = `${environment.apiUrl}/chat`;

  messages = signal<ChatMessage[]>([]);
  isLoading = signal(false);
  sessionId = signal(crypto.randomUUID());

  constructor(private http: HttpClient) { }

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

    } catch (error) {
      const message = this.resolveErrorMessage(error);
      this.addMessage({ content: message, sender: 'bot', timestamp: new Date() });
    } finally {
      this.isLoading.set(false);
    }
  }

  private resolveErrorMessage(error: unknown): string {
    if (error instanceof HttpErrorResponse) {
      if (error.status === 0)
        return 'Não foi possível conectar ao servidor. Verifique sua conexão.';
      if (error.status === 400)
        return error.error?.message ?? 'Mensagem inválida.';
      if (error.status >= 500)
        return 'Erro interno. Nossa equipe já foi notificada.';
    }
    return 'Ocorreu um erro inesperado. Tente novamente.';
  }

  private addMessage(message: ChatMessage): void {
    this.messages.update(messages => [...messages, message]);
  }
}