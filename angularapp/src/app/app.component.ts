import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './services/auth.service';
import { User } from './models/interfaces';

@Component({
  selector: 'app-root',
  template: `
    <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
      <div class="container">
        <a class="navbar-brand" href="/">Fantasy Cricket</a>
        
        <div class="navbar-nav ms-auto" *ngIf="currentUser">
          <span class="navbar-text me-3">{{currentUser.email}}</span>
          <button class="btn btn-outline-light btn-sm" (click)="logout()">Logout</button>
        </div>
        
        <div class="navbar-nav ms-auto" *ngIf="!currentUser">
          <a class="nav-link" routerLink="/login">Login</a>
        </div>
      </div>
    </nav>
    
    <router-outlet></router-outlet>
  `
})
export class AppComponent implements OnInit {
  currentUser: User | null = null;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
    });
  }

  logout(): void {
    this.authService.logout().subscribe(() => {
      this.router.navigate(['/login']);
    });
  }
}
