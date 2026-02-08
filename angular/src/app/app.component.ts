import { Component, OnInit } from '@angular/core';
import { ConfigStateService, AuthService } from '@abp/ng.core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  template: `
    <nav class="navbar">
      <div class="nav-container">
        <a routerLink="/" class="brand">SeriesDB</a>
        <div class="nav-links">
          <button *ngIf="!isAuthenticated" (click)="login()" class="btn-login">Login</button>
          <div *ngIf="isAuthenticated" class="user-info">
            <span>{{ currentUser?.userName }}</span>
            <button (click)="logout()" class="btn-logout">Logout</button>
          </div>
        </div>
      </div>
    </nav>
    <div class="app-container">
      <router-outlet></router-outlet>
    </div>
  `,
  styles: [`
    .navbar {
      background: #1976d2;
      color: white;
      padding: 1rem 2rem;
      box-shadow: 0 2px 4px rgba(0,0,0,.1);
    }
    .nav-container {
      display: flex;
      justify-content: space-between;
      align-items: center;
      width: 100%;
    }
    .brand {
      font-size: 1.5rem;
      font-weight: bold;
      color: white;
      text-decoration: none;
      margin-right: auto;
    }
    .nav-links {
      display: flex;
      gap: 1rem;
      align-items: center;
      margin-left: auto;
    }
    .btn-login, .btn-logout {
      background: white;
      color: #1976d2;
      border: none;
      padding: 0.5rem 1rem;
      border-radius: 4px;
      cursor: pointer;
      font-weight: bold;
    }
    .btn-logout {
      background: rgba(255,255,255,0.2);
      color: white;
      margin-left: 0.5rem;
    }
    .user-info {
      display: flex;
      align-items: center;
      gap: 0.5rem;
    }
    .app-container {
      min-height: calc(100vh - 64px);
      background: #f5f5f5;
      padding: 2rem;
    }
  `]
})
export class AppComponent implements OnInit {
  isAuthenticated = false;
  currentUser: any;

  constructor(
    private authService: AuthService,
    private configState: ConfigStateService,
    private router: Router
  ) { }

  ngOnInit() {
    this.updateAuthState();

    this.configState.getAll$().subscribe(() => {
    });
  }

  updateAuthState() {
    this.isAuthenticated = this.authService.isAuthenticated;

    if (this.isAuthenticated) {
      this.currentUser = this.configState.getOne('currentUser');
    } else {
      this.currentUser = null;
    }
  }

  login() {
    this.authService.navigateToLogin();
  }

  logout() {
    this.authService.logout().subscribe(() => {
      this.updateAuthState();
      this.router.navigate(['/']);
    });
  }
}