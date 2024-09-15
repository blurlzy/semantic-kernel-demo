import { Routes } from '@angular/router';
// layout
import { MainLayoutComponent } from './layouts/main-layout.component';

// register app  routes
export const routes: Routes = [
	{
		path: '',
		component: MainLayoutComponent,
		children: [
			{ path: '', loadChildren: () => import('./modules/chat-module/chat.routes').then(m => m.public_routes) },
		]

	},
	{
		path: 'chat',
		component: MainLayoutComponent,
		children: [
			{ path: '', loadChildren: () => import('./modules/chat-module/chat.routes').then(m => m.public_routes) },
		]
	},
	{
		path: 'profile',
		component: MainLayoutComponent,
		children: [ 
			{ path: '', loadChildren: () => import('./modules/profile-module/profile.routes').then(m => m.profile_routes) },
		]
	},
	{ path: '**',   redirectTo: '', pathMatch: 'full' } // redirect to default screen
];
