import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
// angular material
import {MatMenuModule} from '@angular/material/menu';
import {MatButtonModule} from '@angular/material/button';
import {MatFormFieldModule} from '@angular/material/form-field';

import { ChatService, ChatSession } from './chat-service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, FormsModule, MatMenuModule, MatButtonModule, MatFormFieldModule, RouterLink],
  template: `
    <div class="sidebar">
      <button class="new-chat-button" (click)="addSession()">+ New Chat</button>
      <ul>
        <li *ngFor="let session of sessions" [class.active]="session.id === currentSessionId">            
            @if(session.id === currentSessionId) {
                <input class="form-control" [(ngModel)]="session.title" (blur)="save(session)">
            }
            @else{
              <span><a [routerLink]="['/chat']" [queryParams]="{ id: session.id }" class="text-dark text-decoration-none">{{ session.title }}</a></span>
              <button mat-icon-button [matMenuTriggerFor]="menu" aria-label="Example icon-button with a menu">
                  <i class="bi bi-three-dots-vertical"></i>
              </button>
              <mat-menu #menu="matMenu">
                <button mat-menu-item (click)="selectSession(session.id)">
                  <span><i class="bi bi-pencil-square"></i> Rename</span>
                </button>
                <button mat-menu-item (click)="deleteSession(session.id, $event)">    
                  <span><i class="bi bi-trash-fill"></i> Delete</span>
                </button>
              </mat-menu>
            }

        </li>
      </ul>
    </div>
  `,
  styles: `
    .sidebar {
      display: flex;
      flex-direction: column;
    }

    .new-chat-button {
      border: none;
      padding: 15px;
      text-align: left;
      font-size: 16px;
      cursor: pointer;
    }

    .new-chat-button:hover {
      
    }

    .sidebar ul {
      list-style-type: none;
      padding: 0;
      margin: 0;
      flex: 1;
      overflow-y: auto;
    }

    .sidebar li {
      display: flex;
      align-items: center;
      padding: 15px;
      cursor: pointer;
    }

    .sidebar li:hover {
      background-color: #f5f4f0;
    }

    .sidebar li.active {

    }

    .sidebar li span {
      flex: 1;
    }

    .delete-button {
      background: none;
      border: none;
      color: #888;
      font-size: 16px;
      cursor: pointer;
    }

    .delete-button:hover {
   
    }
  `
})
export class SidebarComponent {
  sessions: ChatSession[] = [];
  currentSessionId: number | null = null;
  newSessionTitle: string = 'New Chat';

  // ctor
  constructor(private chatService: ChatService) {
    this.chatService.sessions$.subscribe((sessions) => {
      this.sessions = sessions;
    });

    this.chatService.currentSessionId$.subscribe((id) => {
      this.currentSessionId = id;
    });
  }

  addSession() {
    this.chatService.addSession(this.newSessionTitle);
  }

  selectSession(id: number) {
    this.currentSessionId = id;
    //this.chatService.setCurrentSession(id);
  }

  deleteSession(id: number, event: Event) {
    event.stopPropagation();
    this.chatService.deleteSession(id);
  }

  save(session: any) {
    // reset 
    this.currentSessionId = null;
    console.log(session);
  }

  updateSessionTitle(id: number, newTitle: string) {
    this.chatService.updateSession(id, newTitle);
  }
}
