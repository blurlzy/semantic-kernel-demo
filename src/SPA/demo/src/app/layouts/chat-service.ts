// import { Injectable } from '@angular/core';
// import { BehaviorSubject } from 'rxjs';

// export interface Message {
//   sender: 'user' | 'assistant';
//   text: string;
// }

// export interface ChatSession {
//   id: number;
//   title: string;
//   messages: Message[];
// }

// @Injectable({
// 	providedIn: 'root',
//  })
//  export class ChatService {
// 	private sessions: ChatSession[] = [];
// 	private currentSessionId = new BehaviorSubject<number | null>(null);
 
// 	sessions$ = new BehaviorSubject<ChatSession[]>(this.sessions);
// 	currentSessionId$ = this.currentSessionId.asObservable();
 
// 	// ctor
// 	constructor() {}
 
// 	addSession(title: string) {
// 	  const newSession: ChatSession = {
// 		 id: Date.now(),
// 		 title,
// 		 messages: [],
// 	  };
// 	  this.sessions.push(newSession);
// 	  this.sessions$.next(this.sessions);
// 	 // this.setCurrentSession(newSession.id);
// 	}
 
// 	updateSession(id: number, title: string) {
// 	  const session = this.sessions.find((s) => s.id === id);
// 	  if (session) {
// 		 session.title = title;
// 		 this.sessions$.next(this.sessions);
// 	  }
// 	}
 
// 	deleteSession(id: number) {
// 	  this.sessions = this.sessions.filter((s) => s.id !== id);
// 	  this.sessions$.next(this.sessions);
// 	  if (this.currentSessionId.value === id) {
// 		 this.setCurrentSession(null);
// 	  }
// 	}
 
// 	setCurrentSession(id: number | null) {
// 	  this.currentSessionId.next(id);
// 	}
 
// 	getCurrentSession(): ChatSession | undefined {
// 	  return this.sessions.find((s) => s.id === this.currentSessionId.value);
// 	}
 
// 	addMessageToCurrentSession(message: Message) {
// 	  const session = this.getCurrentSession();
// 	  if (session) {
// 		 session.messages.push(message);
// 		 this.sessions$.next(this.sessions);
// 	  }
// 	}
//  }