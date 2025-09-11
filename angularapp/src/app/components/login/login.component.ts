import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  template: `
    <div class="container mt-5">
      <div class="row justify-content-center">
        <div class="col-md-6">
          <div class="card">
            <div class="card-header">
              <h3>Login</h3>
            </div>
            <div class="card-body">
              <form [formGroup]="loginForm" (ngSubmit)="onSubmit()">
                <div class="form-group mb-3">
                  <label for="email">Email</label>
                  <input type="email" class="form-control" id="email" formControlName="email">
                  <div *ngIf="loginForm.get('email')?.invalid && loginForm.get('email')?.touched" class="text-danger">
                    Email is required
                  </div>
                </div>
                
                <div class="form-group mb-3">
                  <label for="password">Password</label>
                  <input type="password" class="form-control" id="password" formControlName="password">
                  <div *ngIf="loginForm.get('password')?.invalid && loginForm.get('password')?.touched" class="text-danger">
                    Password is required
                  </div>
                </div>
                
                <button type="submit" class="btn btn-primary" [disabled]="loginForm.invalid || loading">
                  {{loading ? 'Loading...' : 'Login'}}
                </button>
                
                <div *ngIf="errorMessage" class="alert alert-danger mt-3">
                  {{errorMessage}}
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  `
})
export class LoginComponent {
  loginForm: FormGroup;
  loading = false;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      this.loading = true;
      this.authService.login(this.loginForm.value).subscribe(
        response => {
          this.loading = false;
          if (this.authService.isAdmin) {
            this.router.navigate(['/admin']);
          } else {
            this.router.navigate(['/matches']);
          }
        },
        error => {
          this.loading = false;
          this.errorMessage = 'Invalid credentials';
        }
      );
    }
  }
}
