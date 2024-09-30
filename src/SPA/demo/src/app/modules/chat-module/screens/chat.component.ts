import { Component} from '@angular/core';
import { CommonModule } from '@angular/common';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { FormControl, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { finalize, Subject } from 'rxjs';
import { MarkdownModule } from 'ngx-markdown';
// material
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ChatDataService } from '../chat.data.service';
import { ChatLoader } from '../../../core/chat-loader.service';
import { ChatSessionMenuService } from '../../../core/chat-session-menu.service';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, MarkdownModule, MatFormFieldModule, MatInputModule],
  template: `
  <div class="chat-container">
    <div class="messages" >
      <ng-container *ngFor="let message of messages">
        <div [ngClass]="{'message-row': true, 'user-message': message.sender === 'user', 'assistant-message': message.sender === 'assistant'}">
           <div class="message-bubble">
              <!-- User messages -->
              @if(message.sender === 'user'){
                <p>{{ message.text }}</p>
              }

              <!-- Assistant messages with icon -->
              @if(message.sender === 'assistant'){
                <!-- Assistant messages rendered as Markdown -->
                <markdown [data]="message.text" clipboard></markdown>
              }
            
            <span class="timestamp">{{ message.timestamp | date: 'shortTime' }}</span>
          </div>
        </div>
      </ng-container>

      <!-- Loading Indicator -->
      @if(loader.isLoading | async){
        <div class="message-row assistant-message">
          <div class="message-bubble loading">
            <span class="dot"></span>
            <span class="dot"></span>
            <span class="dot"></span>
          </div>
        </div>
      }

    </div>

    <div class="input-area">     
       <textarea class="form-control me-1" [formControl]="userInput" rows="2" required></textarea>  
      <button type="button" class="btn btn-light " [disabled]="userInput.invalid || (loader.isLoading | async) || !(menuService.hasChatSessions | async)" (click)="send()">
				@if (loader.isLoading | async) {
					<div class="spinner-grow text-primary" role="status">
						<span class="visually-hidden">Loading...</span>
					</div>
				}
				@else {
					<i class="bi bi-send "></i> Send
				}					
				</button>                
    </div>

    @if(!(menuService.hasChatSessions | async)){
      <p class="fw-light ms-2 text-warning">Create a new chat session to start a new conversation</p>
    }    
  </div>
  `,
  styles: `
  .chat-container {
      width: 100%;
      min-height: 80vh;
      display: flex;
      flex-direction: column;
      border: none;
      font-family: Arial, sans-serif;
    }

    .messages {
      flex: 1;
      padding: 20px;
      overflow-y: auto;
      background-color: #ffffff;
    }

    .message-row {
      display: flex;
      margin-bottom: 15px;
    }

    .user-message {
      justify-content: flex-end;
    }

    .assistant-message {
      justify-content: flex-start;
    }

    .message-bubble {
      max-width: 75%;
      padding: 15px;
      border-radius: 15px;
      position: relative;
      background-color: #f1f1f1;
    }

    .user-message .message-bubble {
      background-color: #dcf8c6;
    }

    .message-bubble p {
      margin: 0;
    }

    .timestamp {
      display: block;
      font-size: 0.75em;
      color: #999;
      margin-top: 5px;
    }

    .input-area {
      display: flex;
      padding: 10px;
      border-top: 1px solid #e0e0e0;
      background-color: #fafafa;
    }

    .input-area button {
      margin-left: 10px;
      padding: 0 20px;
      font-size: 16px;
      border: none;
      background-color: #0b93f6;
      color: #fff;
      border-radius: 15px;
      cursor: pointer;
    }

    .input-area button:hover {
      background-color: #0b84e6;
    }

    /* Loading Indicator Styles */
    .loading {
      display: flex;
      align-items: center;
    }

    .loading .dot {
      width: 8px;
      height: 8px;
      margin: 0 2px;
      background-color: #555;
      border-radius: 50%;
      animation: loading 1s infinite alternate;
    }

    .loading .dot:nth-child(2) {
      animation-delay: 0.2s;
    }

    .loading .dot:nth-child(3) {
      animation-delay: 0.4s;
    }

    @keyframes loading {
      from {
        opacity: 0.2;
      }
      to {
        opacity: 1;
      }
    }

  `
})
export class ChatComponent {
  // history message (both user and assistant)
  messages: any = [];
  userInput = new FormControl('', [Validators.required]);
  chatSessionId:string = '';

  // ctor
  constructor(private activatedRoute: ActivatedRoute,
    private sanitizer: DomSanitizer,
    private chatService: ChatDataService,
    public menuService: ChatSessionMenuService,
    public loader: ChatLoader) {

    //disable the input area if there are no chat sessions
    this.menuService.hasChatSessions.subscribe((hasChatSessions: boolean) => {
      // at lease one chat session required before sending chat messages
      if (!hasChatSessions) {
        this.userInput.disable();
      }
      else {
        this.userInput.enable();
      }

    });

    // subscribe on chat session id changes
    // query params change
    this.activatedRoute.queryParams.subscribe(params => {
      this.chatSessionId = params["id"];
      if (this.chatSessionId) {
        // load chat history
        this.loadChatHistory(this.chatSessionId);
      }
      else{
        this.chatSessionId = '';
        this.messages = [];
      }
      // console.log(chatSessionId);
    });

  }

  ngOnInit() {

  }

  private loadChatHistory(chatSessionId: string): void {
    // reset messages
    this.messages = [];
    this.chatService.getChatHistory(chatSessionId)
      .subscribe((data: any) => {
        //console.log(data);
        for (let i = 0; i < data.length; i++) {
          const message = data[i];
          const messageData: any = {
            sender: message.authorRole === 1 ? 'assistant' : 'user',
            text: message.content,
            timestamp: message.timestamp
          };
          this.messages.push(messageData);
        }

        // scroll to bottom
        setTimeout(() => {
          this.scrollToBottom();
        }, 0);
      });
  }

  send(): void {

    this.loader.isLoading.next(true);
    // chat request
    const req = { input: this.userInput.value, chatSessionId: this.chatSessionId };

    // add user message into list
    const userMessage: any = {
      sender: 'user',
      text: this.userInput.value,
      timestamp: new Date()
    };

    this.messages.push(userMessage);
    this.userInput.reset();

    // call API
    this.chatService.chat(req)
      .pipe(finalize(() => this.loader.isLoading.next(false)))
      .subscribe((data: any) => {
        //console.log(data);
        // Add assistant message
        const assistantMessage: any = {
          sender: 'assistant',
          text: data.value,
          // // Include the icon using safe HTML
          // safeText: this.sanitizer.bypassSecurityTrustHtml(
          //   `<i class="bi bi-robot"></i> ${data.value}`
          // ),
          timestamp: new Date()
        };

        // add assistant message into list
        this.messages.push(assistantMessage);
      });
  }


  private scrollToBottom(): void {
    try {
      window.scrollTo({ top: document.body.scrollHeight, behavior: 'smooth' });
    } catch (err) {
      console.error('Scroll to page bottom failed:', err);
    }
  }

  // scrollToBottom() {
  //   const messagesContainer = document.querySelector('.messages');
  //   if (messagesContainer) {
  //     setTimeout(() => {
  //       messagesContainer.scrollTop = messagesContainer.scrollHeight;
  //     }, 100);
  //   }
  // }

  // getAssistantReply(userText: string): string {
  //   // Generate assistant's reply based on user input
  //   return `You said: "${userText}"`;
  // }

}
