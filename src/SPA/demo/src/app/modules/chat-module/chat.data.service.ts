import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ChatDataService {
  // ctor
  constructor(private http: HttpClient) {}

  // create a new chat session
  createChatSession(chatSession:any): Observable<any> {
    return this.http.post(`${environment.copilotApiConfig.uri}/ChatSessions`, chatSession);
  }

  // get chat sessions
  getChatSessions(): Observable<any> {
    return this.http.get(`${environment.copilotApiConfig.uri}/ChatSessions`);
  } 
  
  getChatMessages(request:any): Observable<any> {
    return this.http.post(`${environment.copilotApiConfig.uri}/Chat/Messages`, request);
  }

  // // temp test
  // getItems(): Observable<any> {
  //   return this.http.get(`${environment.apiConfig2.uri}/items/private`);
  // }

  // // test
  // getProfile(): Observable<any> { 
  //   return this.http.get(`${environment.copilotApiConfig.uri}/Chat/profile`);
  // }
}
