import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, RouterLink, Router } from '@angular/router';
// msal
import { MsalModule } from '@azure/msal-angular';
// components
import { TopNavComponent } from './top-nav.component';	
import { SidebarComponent } from './sidebar.component';	// import the sidebar component

@Component({
	selector: 'app-main-layout',
	standalone: true,
	imports: [RouterOutlet, RouterLink, CommonModule, MsalModule, 
					SidebarComponent, TopNavComponent],
	template: `
	<app-top-nav></app-top-nav>

	<div class="container-fluid" style="margin-top: 60px;">
		<div class="row">
			<nav class="d-md-block collapse" [ngClass]="sidebarCollapsed ? '' : 'show'">
				<app-sidebar></app-sidebar>
			</nav>
	
			<!-- content area -->
			<main class="col-md-9 ms-sm-auto col-lg-10 px-md-4 border-start">		
				<div class="container">
					<router-outlet></router-outlet>
				</div>	
				
				<!-- footer -->
				<div class="d-flex flex-column flex-sm-row justify-content-between py-1 my-4 border-top">
					<p>&copy; ZL 2024</p>
					<ul class="list-unstyled d-flex">
						<li class="ms-3"><a class="text-dark" href="https://github.com/blurlzy" target="_blank"><i class="bi bi-github"></i></a></li>
					</ul>
				</div>
			</main>
		</div>


	</div>

  `,
	styles: `


  `
})
export class MainLayoutComponent {
	sidebarCollapsed = false;
	// current account
	currentAccount: any = null;

	// ctor
	// constructor(@Inject(MSAL_GUARD_CONFIG) private msalGuardConfig: MsalGuardConfiguration,
	// 	private authService: MsalService,
	// 	private msalBroadcastService: MsalBroadcastService) {

	// }

	constructor(){

	}
	ngOnInit(): void {
		this.checkAndSetActiveAccount();
		//this.setLoginDisplay();
	}

	// signout
	signOut(): void {
		// this.authService.logout();
	}

	// private setLoginDisplay() {
	// 	const activeAccount = this.authService.instance.getActiveAccount();
	// 	const totalAccounts = this.authService.instance.getAllAccounts();
	// 	console.log(activeAccount);
	// 	console.log(totalAccounts[0]);
	// }

	private checkAndSetActiveAccount() {
		/**
		 * If no active account set but there are accounts signed in, sets first account to active account
		 * To use active account set here, subscribe to inProgress$ first in your component
		 * Note: Basic usage demonstrated. Your app may require more complicated account selection logic
		 */
		// let activeAccount = this.authService.instance.getActiveAccount();

		// if (!activeAccount && this.authService.instance.getAllAccounts().length > 0) {
		// 	let accounts = this.authService.instance.getAllAccounts();
		// 	this.authService.instance.setActiveAccount(accounts[0]);
		// 	this.currentAccount = accounts[0];
		// }
		// else{
		// 	this.currentAccount = activeAccount;
		// }
	}
}
