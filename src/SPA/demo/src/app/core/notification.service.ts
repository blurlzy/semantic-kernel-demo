import { Injectable } from '@angular/core';
// angular material
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
	providedIn: 'root'
})
export class SnackbarService {
	// duration
	private durationSeconds: number = 1000 * 5;

	// ctor
	constructor(private snackBar: MatSnackBar) {

	}

	public success(message: string) {
		// show message                    
		this.snackBar.open(message, 'x', {
			duration: this.durationSeconds,
			// panelClass: ['text-white', 'bg-success'],
			horizontalPosition: 'center'
		});
	}

	public error(message: string) {
		this.snackBar.open(message, 'x', {
			duration: this.durationSeconds,
			// panelClass: ['text-white', 'bg-danger'],
			horizontalPosition: 'center'
		});
	}

	public warn(message: string) {
		this.snackBar.open(message, 'x', {
			duration: this.durationSeconds,
			horizontalPosition: 'center'
		});
	}

	public dismiss() : void {
		this.snackBar.dismiss();
	}
}