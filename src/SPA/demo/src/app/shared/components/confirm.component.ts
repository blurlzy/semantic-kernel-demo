import { Component, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogActions, MatDialogContent, MatDialogRef, MatDialogClose } from '@angular/material/dialog';

@Component({
  selector: 'app-confirm',
  standalone: true,
  imports: [ MatDialogActions, MatDialogContent, MatDialogClose ],
  template: `
      <h5  mat-dialog-title class="text-danger ms-3 mt-2"><i class="bi bi-exclamation"></i></h5>
      <div mat-dialog-content>
            <span>Are you sure you want to continue?</span>
      </div>
      <div mat-dialog-actions>
        <button class="btn btn-light" [mat-dialog-close]="false" cdkFocusInitial>Close</button>
        <button class="btn btn-danger ms-2"  (click)="confirm()" >Delete</button>
      </div>
  `,
  styles: ``
})
export class ConfirmComponent {
  readonly dialogRef = inject(MatDialogRef<ConfirmComponent>);
  readonly data = inject<any>(MAT_DIALOG_DATA);

  // ctor
  constructor() { }

  confirm(): void {
    this.dialogRef.close(true); 
  }
}
