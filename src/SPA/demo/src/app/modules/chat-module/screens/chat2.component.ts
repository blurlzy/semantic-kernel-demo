import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { FormControl, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize, Subject } from 'rxjs';
import { ChatDataService } from '../chat.data.service';
import { Loader } from '../../../core/loader.service';

@Component({
  selector: 'app-chat2',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  template: `
  <div class="chat-container">
    <div class="messages">
      <ng-container *ngFor="let message of messages">
        <div [ngClass]="{'message-row': true, 'user-message': message.sender === 'user', 'assistant-message': message.sender === 'assistant'}">
           <div class="message-bubble">
              <!-- User messages -->
              @if(message.sender === 'user'){
                <p>{{ message.text }}</p>
              }

              <!-- Assistant messages with icon -->
              @if(message.sender === 'assistant'){
                <p [innerHTML]="message.safeText"></p>
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
      <input type="text" [formControl]="userInput" placeholder="Type your message..." />
      <button type="button" class="btn btn-light" [disabled]="userInput.invalid || (loader.isLoading | async)" (click)="send()">
				@if (loader.isLoading | async) {
					<div class="spinner-grow text-primary" role="status">
						<span class="visually-hidden">Loading...</span>
					</div>
				}
				@else {
					<i class="bi bi-send"></i> Send
				}
					
				</button>
    </div>
  </div>
  `,
  styles: `
  .chat-container {
      width: 100%;
      height: 80vh;
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
      padding: 15px;
      border-top: 1px solid #e0e0e0;
      background-color: #fafafa;
    }

    .input-area input {
      flex: 1;
      padding: 12px;
      font-size: 16px;
      border: 1px solid #ccc;
      border-radius: 25px;
      outline: none;
    }

    .input-area button {
      margin-left: 10px;
      padding: 0 20px;
      font-size: 16px;
      border: none;
      background-color: #0b93f6;
      color: #fff;
      border-radius: 25px;
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
export class Chat2Component {
  messages: any = [];
  // request: any = {
	// 	messages: []
	// };
  userInput = new FormControl('', [Validators.required]);
  // userInput: string = '';
  // isLoading: boolean = false;
  // messageId: number = 0;

  // ctor
  constructor(private sanitizer: DomSanitizer,
            private chatService: ChatDataService,
				    public loader: Loader) {

  }

  send(): void {
		this.loader.isLoading.next(true);
		const req = { input: this.userInput.value };

    const userMessage: any = {
      sender: 'user',
      text: this.userInput.value,
      timestamp: new Date()
    };
    this.messages.push(userMessage);
    this.userInput.reset();
    
    // call API
		this.chatService.getChatMessages(req)
			.pipe(finalize(() => this.loader.isLoading.next(false)))
			.subscribe((data: any) => {
					console.log(data);
          // Add assistant message
          const assistantMessage: any = {
            sender: 'assistant',
            text: data.response,
            // Include the icon using safe HTML
            safeText: this.sanitizer.bypassSecurityTrustHtml(
              `<i class="bi bi-robot"></i> ${data.value}`
            ),
            timestamp: new Date()
          };
          this.messages.push(assistantMessage);

					// this.request.messages.push({ role: 'system', content: data.response });
					// this.userInput.reset();
				});	
	}

  // sendMessage() {
  //   if (this.userInput.trim()) {
  //     // Add user message
  //     const userMessage: any = {
  //       id: this.messageId++,
  //       sender: 'user',
  //       text: this.userInput,
  //       timestamp: new Date()
  //     };
  //     this.messages.push(userMessage);
  //     const userText = this.userInput;
  //     this.userInput = '';

  //     // Show loading indicator
  //     this.isLoading = true;

  //     // Simulate assistant response
  //     setTimeout(() => {
  //       const assistantReply = this.getAssistantReply(userText);
  //       const assistantMessage: any = {
  //         id: this.messageId++,
  //         sender: 'assistant',
  //         text: assistantReply,
  //         // Include the icon using safe HTML
  //         safeText: this.sanitizer.bypassSecurityTrustHtml(
  //           `<i class="bi bi-robot"></i> ${assistantReply}`
  //         ),
  //         timestamp: new Date()
  //       };
  //       this.messages.push(assistantMessage);
  //       this.isLoading = false;

  //       // Auto-scroll to the latest message
  //       this.scrollToBottom();
  //     }, 1000);
  //   }
  // }

  scrollToBottom() {
    const messagesContainer = document.querySelector('.messages');
    if (messagesContainer) {
      setTimeout(() => {
        messagesContainer.scrollTop = messagesContainer.scrollHeight;
      }, 100);
    }
  }

  getAssistantReply(userText: string): string {
    // Generate assistant's reply based on user input
    return `You said: "${userText}"`;
  }

}
