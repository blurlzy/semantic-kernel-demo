import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, RouterLink, Router } from '@angular/router';
// msal
import { MsalModule, MsalService, MsalBroadcastService, MSAL_GUARD_CONFIG, MsalGuardConfiguration, } from '@azure/msal-angular';

@Component({
	selector: 'app-main-layout',
	standalone: true,
	imports: [RouterOutlet, RouterLink, CommonModule, MsalModule],
	template: `
	<header class="navbar sticky-top flex-md-nowrap p-2 border-bottom bg-light">
		<a class="col-md-3 col-lg-2 px-3 fs-6 text-decoration-none text-black" routerLink="/">
			<i class="bi bi-flower1 icon-large me-2"></i> ZL Demo
		</a> 
		<button class="navbar-toggler position-absolute d-md-none collapsed" type="button">
			<span class="navbar-toggler-icon"></span>
		</button>
		<div class="navbar-nav">
			<div class="nav-item text-nowrap">
				{{ currentAccount?.name }}
			</div>
			<!-- <div class="nav-item text-nowrap">
				<button class="btn btn-light" (click)="signOut()">Sign out</button>
			</div> -->
		</div>
	</header>

	<div class="container-fluid">
		<div class="row">
			<nav class="col-md-3 col-lg-2 d-md-block bg-light sidebar collapse border-right" [ngClass]="sidebarCollapsed ? '' : 'show'">
				<div class="position-sticky pt-3 sidebar-sticky mt-3">
					<ul class="nav flex-column">
						<li class="nav-item">
							<a class="nav-link active" aria-current="page" routerLink="/">
								Home
							</a>
						</li>
					</ul>

					<h6 class="sidebar-heading d-flex justify-content-between align-items-center px-3 mt-4 mb-1 text-muted text-uppercase">
						<span>Settings</span>
						<a class="link-secondary" aria-label="Add a new report">
							<i class="bi bi-gear align-text-bottom"></i>
						</a>
					</h6>
					<ul class="nav flex-column mb-2">
						<li class="nav-item">
							<a class="nav-link" routerLink="/profile">
								<i class="bi bi-person-fill me-1"></i> Profile
							</a>
						</li>
					</ul>
				</div>
			</nav>
			<!-- content area -->
			<main class="col-md-9 ms-sm-auto col-lg-10 px-md-4">			
				<router-outlet></router-outlet>

				<!-- footer -->
				<div class="d-flex flex-column flex-sm-row justify-content-between py-4 my-4 border-top">
					<p>&copy; ZL 2024</p>
					<ul class="list-unstyled d-flex">
						<li class="ms-3"><a class="link-body-emphasis" href="https://github.com/blurlzy" target="_blank"><i class="bi bi-github"></i></a></li>
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
