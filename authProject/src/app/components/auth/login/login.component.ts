import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { IUserSession } from '../../../models/UserSession ';
import { IErrorResponse } from '../../../models/error';
import { ILogin } from '../../../models/auth';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  passwordFieldType: string = 'password';

  constructor(
    private readonly _route: Router,
    private readonly _fb: FormBuilder,
    private readonly _authService: AuthService
  ) {}

  ngOnInit(): void {
    this.loginForm = this._fb.group({
      email: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  togglePassword(): void {
    this.passwordFieldType =
      this.passwordFieldType === 'password' ? 'text' : 'password';
  }

  isSubmitting: boolean = false;

  login(): void {
    if (this.loginForm.valid && !this.isSubmitting) {
      this.isSubmitting = true;
      console.log('Login function triggered');

      this._authService.login(this.loginForm.value).subscribe({
        next: (response: ILogin): void => {
          const userSession: IUserSession = {
            name: response.name,
            email: response.email,
            role: response.role,
          };

          this._authService.storeUserSession(response.token, userSession);

          this._route.navigate(['/dashboard']);
        },
        error: (error: IErrorResponse): void => {
          console.error('Login Failed:', error);
          this.isSubmitting = false;
        },
        complete: (): void => {
          this.isSubmitting = false;
        },
      });
    }
  }
}
