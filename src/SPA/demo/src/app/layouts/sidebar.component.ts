import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink, ActivatedRoute } from '@angular/router';
// angular material
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDialog } from '@angular/material/dialog';
// services
import { ChatSessionMenuService } from '../core/chat-session-menu.service';
import { ChatDataService } from '../modules/chat-module/chat.data.service';
// components
import { ChatSessionDialogComponent } from '../modules/chat-module/components/chat-session-dialog.component';
import { ConfirmComponent } from '../shared/components/confirm.component';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, FormsModule, MatMenuModule, MatButtonModule, MatFormFieldModule, RouterLink,
    ChatSessionDialogComponent, ConfirmComponent],
  template: `
    <div class="sidebar">
      <button class="new-chat-button" (click)="openChatSessionDialog()">+ New Chat</button>
      <ul class="border-top">
        <li *ngFor="let session of menuItems" [class.active]="session.id === currentSessionId">            
            @if(session.id === editSessionId) {
                <input class="form-control" [(ngModel)]="session.title" (blur)="updateChatSession(session)">
            }
            @else{
              <a [routerLink]="['/chat']" [queryParams]="{ id: session.id }" class="text-dark text-decoration-none">
                {{ session.title }} <i class="bi bi-check-lg ms-2" *ngIf="session.id == currentSessionId"></i>
              </a>
              
              <button mat-icon-button [matMenuTriggerFor]="menu" class="menu-option-button">
                  <i class="bi bi-three-dots-vertical"></i>
              </button>
              <mat-menu #menu="matMenu">
                <button mat-menu-item (click)="selectSession(session.id)">
                  <span><i class="bi bi-pencil-square"></i> Rename</span>
                </button>
                <button mat-menu-item (click)="deleteChatSession(session)">    
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
      position: fixed;
      top: 60px; /* Adjust based on your top navigation bar height */
      bottom: 0;
      left: 0;
      min-width: 220px;
      display: flex;
      flex-direction: column;
    }

    .new-chat-button {
      border: none;
      padding: 15px;
      text-align: left;
      font-size: 16px;
      cursor: pointer;
      background-color: transparent;
    }

    .sidebar li a {
        display: block;
        width: 100%;
        height: 100%;
        text-decoration: none;
        display: flex;
        align-items: center;
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
      background-color: #f5f4f0;
    }

    .menu-option-button {
        visibility: hidden;  /* Hidden by default */
    }

    .sidebar li:hover .menu-option-button {
         visibility: visible; /* Show on hover */
    }
  `
})
export class SidebarComponent {
  menuItems: any = [];
  editSessionId: number | null = null;
  currentSessionId: string | null = null;


  readonly dialog = inject(MatDialog);
  readonly menuService = inject(ChatSessionMenuService);
  readonly chatDataService = inject(ChatDataService);

  // ctor
  constructor(private activatedRoute: ActivatedRoute) {
    // subscribe to the menu item changes
    this.menuService.menuItems$.subscribe((menuItems) => {
      //console.log("menu item changes", menuItems);
      this.menuItems = menuItems;
    });
  }

  ngOnInit() {
    // query params change
    this.activatedRoute.queryParams.subscribe(params => {
      //console.log(params);
      this.currentSessionId = params["id"];
    });

    // load chat sessions
    this.loadChatSessions();
  }

  private loadChatSessions() {
    // get chat sessions
    this.chatDataService.getChatSessions().subscribe(response => {
      this.menuItems = response;
      this.menuService.init(this.menuItems);
    });
  }

  // open new chat session dialog
  openChatSessionDialog() {
    const dialogRef = this.dialog.open(ChatSessionDialogComponent, {
      data: {},
      width: '390px'
    });
  }

  deleteChatSession(chatSession: any) {
    //  open confirm dialog
    const dialogRef = this.dialog.open(ConfirmComponent, {
      data: { message: `Are you sure you want to delete chat: ${chatSession.title}?` },
      width: '390px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === true) {
        // delete chat session

        // remove from menu
      }
    });
  }

  updateChatSession(chatSession: any) {
    const req = { id: chatSession.id, title: chatSession.title };
    // update chat session
    this.chatDataService.updateChatSession(req).subscribe(response => {
      // reset the edit session id
      this.editSessionId = null;
    });
  }

  // addSession() {
  //   this.menuService.addItem(this.newSessionTitle);
  // }

  selectSession(id: number) {
    this.editSessionId = id;
  }

  deleteSession(id: number, event: Event) {
    event.stopPropagation();
    this.menuService.deleteItem(id);
  }

  save(session: any) {
    // reset 
    this.editSessionId = null;
    console.log(session);
  }

}
