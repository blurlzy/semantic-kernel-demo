import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, RouterLink, Router } from '@angular/router';
// angular material
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-top-nav',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink, MatMenuModule, MatButtonModule],
  template: `
    <nav class="navbar navbar-expand-lg navbar-light bg-light fixed-top border-bottom" style="z-index: 99;">
      <div class="container-fluid">
        	<a  class="navbar-brand" routerLink="/">
            <i class="bi bi-flower1 icon-large me-2"></i> ZL Demo
          </a> 
        <button class="navbar-toggler" type="button" (click)="toggleNav()">
          <span class="navbar-toggler-icon"></span>
        </button>
        <div [ngClass]="collapsed ? 'collapse navbar-collapse' : 'collapse navbar-collapse show'">
          <!--
            <ul class="navbar-nav">
                <li class="nav-item">
                  <a class="nav-link" routerLink="/profile">Profile</a>
                </li>
            </ul>
           -->

            <div class="nav me-auto my-2 my-lg-0 navbar-nav-scroll"></div>
            <div class="d-flex">
                   <button mat-button [matMenuTriggerFor]="menu">
                      <i class="bi bi-person-fill h5 text-dark"></i>
                    </button>
                    <mat-menu #menu="matMenu">
                      <button mat-menu-item routerLink="/profile">Profile</button>
                      <button mat-menu-item>Logout</button>                        
                    </mat-menu>
            </div>
        </div>
      </div>
    </nav>
  `,
  styles: ``
})
export class TopNavComponent {
  collapsed = true;

  constructor() { }

  // public methods
  // toggle menu when it's in mobile view
  toggleNav(): void {
    this.collapsed = !this.collapsed;
  }

}
