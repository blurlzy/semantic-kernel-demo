import { Routes } from '@angular/router';
// msal 
import { MsalGuard } from '@azure/msal-angular';

// screens
import { ChatComponent } from './screens/chat.component';


// routes for chat module screens
export const public_routes: Routes = [
	{ path: '', component: ChatComponent },
];
