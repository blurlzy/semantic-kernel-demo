import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ChatSessionMenuService {
	// menu items
	private menuItems: any = [];

	menuItems$ = new BehaviorSubject<any>(this.menuItems);

	// ctor
	constructor() {

	}

	// load menu items (chat sessions)
	// add menu item
	addItem(id:string, title: string) {
		const newItem = {
			id: id,
			title: title,
		};

		//
		this.menuItems.push(newItem);
		// publish the updated menu items
		this.publishChanges();
	}

	deleteItem(id: number) {
		this.menuItems = this.menuItems.filter((s: any) => s.id !== id);
		this.publishChanges();
	}

	// publish the updated menu items
	private publishChanges() {
		this.menuItems$.next(this.menuItems);
	}
}	