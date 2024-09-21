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

	<div class="container-fluid" style="margin-top: 65px;">
		<div class="row">
			<nav class="col-md-3 col-lg-2 d-md-block bg-light sidebar collapse border-right" [ngClass]="sidebarCollapsed ? '' : 'show'">
				<app-sidebar></app-sidebar>
			</nav>
	
			<!-- content area -->
			<main class="col-md-9 ms-sm-auto col-lg-10 px-md-4">			
				<router-outlet></router-outlet>

				<!-- footer -->
				<div class="d-flex flex-column flex-sm-row justify-content-between py-4 my-4 border-top">
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
  body {
	font-size: .875rem;
 }
 
 .feather {
	width: 16px;
	height: 16px;
 }
 
 /*
  * Sidebar
  */
 
 .sidebar {
	position: fixed;
	top: 0;
	/* rtl:raw:
	right: 0;
	*/
	bottom: 0;
	/* rtl:remove */
	left: 0;
	z-index: 100; /* Behind the navbar */
	padding: 48px 0 0; /* Height of navbar */
	box-shadow: inset -1px 0 0 rgba(0, 0, 0, .1);
 }
 
 @media (max-width: 767.98px) {
	.sidebar {
	  top: 5rem;
	}
 }
 
 .sidebar-sticky {
	height: calc(100vh - 48px);
	overflow-x: hidden;
	overflow-y: auto; /* Scrollable contents if viewport is shorter than content. */
 }
 
 .sidebar .nav-link {
	font-weight: 500;
	color: #333;
 }
 
 .sidebar .nav-link .feather {
	margin-right: 4px;
	color: #727272;
 }
 
 .sidebar .nav-link.active {
	color: #2470dc;
 }
 
 .sidebar .nav-link:hover .feather,
 .sidebar .nav-link.active .feather {
	color: inherit;
 }
 
 .sidebar-heading {
	font-size: .75rem;
 }
 
 /*
  * Navbar
  */
 
 .navbar-brand {
	padding-top: .75rem;
	padding-bottom: .75rem;
	background-color: rgba(0, 0, 0, .25);
	box-shadow: inset -1px 0 0 rgba(0, 0, 0, .25);
 }
 
 .navbar .navbar-toggler {
	top: .25rem;
	right: 1rem;
 }
 
 .navbar .form-control {
	padding: .75rem 1rem;
 }
 
 .form-control-dark {
	color: #fff;
	background-color: rgba(255, 255, 255, .1);
	border-color: rgba(255, 255, 255, .1);
 }
 
 .form-control-dark:focus {
	border-color: transparent;
	box-shadow: 0 0 0 3px rgba(255, 255, 255, .25);
 }
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
