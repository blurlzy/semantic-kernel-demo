import { Injectable, ErrorHandler, NgZone, inject } from '@angular/core';
// angular material
import {MatSnackBar} from '@angular/material/snack-bar';

@Injectable({
	providedIn: 'root'
})
export class GlobalErrorHandler implements ErrorHandler {
	private snackBar = inject(MatSnackBar);
	private zone = inject(NgZone);

	// duration
	private durationSeconds: number = 1000 * 5;

	// 
	handleError(error: unknown) {
		this.zone.run(() => {
			const message = 'It seems we are experiencing a problem at the moment.';
			// show message
			this.snackBar.open(message, 'x', {
				duration: this.durationSeconds,
				horizontalPosition: 'center'
			});
		});

		console.warn('Caught by global error handler: ', error);
	}
}