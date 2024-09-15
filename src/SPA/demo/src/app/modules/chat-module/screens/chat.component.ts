import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize, Subject } from 'rxjs';
import { ChatDataService } from '../chat.data.service';
import { Loader } from '../../../core/loader.service';

@Component({
	selector: 'app-chat',
	standalone: true,
	imports: [CommonModule, FormsModule, ReactiveFormsModule],
	template: `
	<div class="chat-container container-fluid d-flex flex-column mt-3 p-3">
		<div class="chat-body flex-grow-1 overflow-auto p-3">
			<div class="message received mb-3">
				<div class="avatar me-3">
					<p><i class="bi bi-robot icon-large"></i></p>
				</div>
				<div class="text">
					<p>Hello, forecastify chatbot, how can I assist you today?</p>
				</div>
			</div>

			<div class="message mb-3" *ngFor="let message of request.messages" [ngClass]="message.role === 'user'? 'sent' : 'received'">

				<div class="text">
					<ng-container *ngIf="message.role === 'system'">
						<p class="wrap-text" >
							{{ message.content }}
						</p>
					</ng-container>
					<ng-container *ngIf="message.role === 'user'">
						<p class="wrap-text">{{ message.content }}</p>
					</ng-container>
				</div>
			</div>

		</div>
		<div class="chat-footer d-flex justify-content-center align-items-center p-3">
			<form class="d-flex w-100">
				<textarea class="form-control me-2" rows="2" [formControl]="userInput"></textarea>
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
			</form>
		</div>
	</div>
  `,
	styles: `
	.chat-container {
		background-color: #fff;
		border-radius: 15px;
		box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
	}

	.chat-header {
		height: 60px;
	}

	.chat-header h1 {
		font-size: 24px;
	}

	.message {
		display: flex;
		align-items: center;
		border-radius: 10px 10px 10px 10px;
		padding: 10px;
	}

	.avatar img {
		width: 50px;
		height: 50px;
		border-radius: 50%;
	}

	// user
	.sent {
		background-color: #0d6efd;		
		color: white;
		display: inline-block; /* Fit the div to its content */
		justify-content: flex-end; /* Aligns the inner div to the right */
	}

	// system
	.received {
		background-color:#f2f2f2;	
		
		//word-wrap: break-word;
	}

	.wrap-text {
		white-space: pre-wrap;
	}
	
	.chat-footer {
		border-radius: 10px;
	}

	.btn-light:hover {
		background-color: lightgray;

	}

`
})
export class ChatComponent {
	// init chat messages
	request: any = {
		messages: []
	};
	userInput = new FormControl('', [Validators.required]);

	constructor(private chatService: ChatDataService,
				public loader: Loader) {

	}

	ngOnInit(): void {
		// this.getItems();
	}

	send(): void {
		this.loader.isLoading.next(true);
		const req = { userMessage: this.userInput.value };
		this.chatService.getChatMessages(req)
			.pipe(finalize(() => this.loader.isLoading.next(false)))
			.subscribe((data: any) => {
					// console.log(data);
					this.request.messages.push({ role: 'user', content: this.userInput.value });
					this.request.messages.push({ role: 'system', content: data.response });
					this.userInput.reset();
				});	
	}

}
