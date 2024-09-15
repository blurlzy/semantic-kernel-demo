import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
// msal
import { MsalModule, MsalService, MSAL_GUARD_CONFIG, MsalGuardConfiguration, } from '@azure/msal-angular';
// env
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, MsalModule],
  template: `
    <div class="container mt-5">
      <div class="row justify-content-center">
        <div class="col-md-6">
          <div class="card text-center">
            <div class="card-body">
              <h4 class="card-title">{{ currentAccount?.name }} - {{ currentAccount?.username }}</h4>
              <button class="btn btn-link mt-3" (click)="signOut()">Sign Out</button>
            </div>
          </div>
        </div>
      </div>
    </div>

  `,
  styles: ``
})
export class ProfileComponent {
  currentAccount: any = null;
  // ctor
  constructor(@Inject(MSAL_GUARD_CONFIG) private msalGuardConfig: MsalGuardConfiguration,
    private authService: MsalService) {

  }

  ngOnInit() {
    this.checkAndSetActiveAccount();
    this.retreiveAccessToken();
  }

  signOut(): void {
    this.authService.logout();
  }

  private checkAndSetActiveAccount() {
    /**
     * If no active account set but there are accounts signed in, sets first account to active account
     * To use active account set here, subscribe to inProgress$ first in your component
     * Note: Basic usage demonstrated. Your app may require more complicated account selection logic
     */
    let activeAccount = this.authService.instance.getActiveAccount();

    if (!activeAccount && this.authService.instance.getAllAccounts().length > 0) {
      let accounts = this.authService.instance.getAllAccounts();
      this.authService.instance.setActiveAccount(accounts[0]);
      this.currentAccount = accounts[0];
    }
    else {
      this.currentAccount = activeAccount;
    }

    console.log(this.currentAccount);
  }

  // retreive access token
  private retreiveAccessToken() {
    var request = {
      scopes: environment.apiConfig2.scopes,
  };
  
  this.authService.acquireTokenSilent(request).subscribe(tokenResponse => {
      // Do something with the tokenResponse
      console.log(tokenResponse);
    });
  }
}
