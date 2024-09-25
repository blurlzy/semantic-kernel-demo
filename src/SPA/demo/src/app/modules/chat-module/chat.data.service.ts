import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ChatDataService {
  // endpoints
  private readonly chatSessionsEndpoint = `${environment.copilotApiConfig.uri}/ChatSessions`;
  private readonly chatMessagesEndpoint = `${environment.copilotApiConfig.uri}/Chat`;
  // ctor
  constructor(private http: HttpClient) {}

  // create a new chat session
  createChatSession(chatSession:any): Observable<any> {
    return this.http.post(this.chatSessionsEndpoint, chatSession);
  }

  // get chat session list
  getChatSessions(): Observable<any> {
    return this.http.get(this.chatSessionsEndpoint);
  } 
  
  updateChatSession(chatSession:any): Observable<any> { 
    return this.http.put(this.chatSessionsEndpoint + `/${chatSession.id}`, chatSession);
  }

  /* chat messages */
  getChatHistory(chatSessionId:string): Observable<any> {
    return this.http.get(`${this.chatMessagesEndpoint}/${chatSessionId}/history-messages`);
  }

  chat(request:any): Observable<any> {
    return this.http.post(`${environment.copilotApiConfig.uri}/Chat/Messages`, request);
  }

}
