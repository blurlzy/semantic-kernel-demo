import { Routes } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular'; // msal 

// screens
import { ProfileComponent } from './screens/profile.component';

// routes
export const profile_routes: Routes = [
    { path: '', component: ProfileComponent,  canActivate: [MsalGuard] },
];