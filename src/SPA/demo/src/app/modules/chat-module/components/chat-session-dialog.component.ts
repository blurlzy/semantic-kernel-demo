import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormControl, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MAT_DIALOG_DATA, MatDialogActions, MatDialogContent, MatDialogRef } from '@angular/material/dialog';
// services
import { ChatDataService } from '../chat.data.service';
import { ChatSessionMenuService } from '../../../core/chat-session-menu.service';
import { Loader } from '../../../core/loader.service';

@Component({
  selector: 'app-chat-session-dialog',
  standalone: true,
  imports: [ReactiveFormsModule, MatButtonModule, MatInputModule, MatDialogActions, MatDialogContent,],
  template: `
    <mat-dialog-content>
      <mat-form-field class="full-width">
        <mat-label>Chat Title </mat-label>
        <input matInput [formControl]="chatTitleCtrl" />
      </mat-form-field>
    </mat-dialog-content>
    <mat-dialog-actions>
      <button type="button" class="btn btn-light" (click)="onNoClick()" cdkFocusInitial>Cancel</button>
      <button type="button" class="btn btn-primary ms-2" (click)="create()" [disabled] = "chatTitleCtrl.invalid">Create</button>
    </mat-dialog-actions>
  `,
  styles: ``
})
export class ChatSessionDialogComponent {
  readonly dialogRef = inject(MatDialogRef<ChatSessionDialogComponent>);
  readonly data = inject<any>(MAT_DIALOG_DATA);
  readonly chatDataService = inject(ChatDataService);
  readonly menuService = inject(ChatSessionMenuService);
  readonly loader = inject(Loader);

  // form control
  chatTitleCtrl = new FormControl('', [Validators.required]);

  // ctor
  constructor() { }

  // public methods
  onNoClick(): void {
    this.dialogRef.close();
  }

  create(): void {
    this.loader.isLoading.next(true); // show loading indicator
    // create a new chat session
    this.chatDataService.createChatSession({ title: this.chatTitleCtrl.value })
      .pipe(finalize(() => this.loader.isLoading.next(false)))
      .subscribe((response) => {
        // add into menu        
        //console.log(response);
        this.menuService.addItem(response.id, response.title);    
        this.dialogRef.close();  
      }); 
  }
}
