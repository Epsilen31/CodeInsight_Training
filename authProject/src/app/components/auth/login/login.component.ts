import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { IUserSession } from '../../../models/userSession ';
import { IErrorResponse } from '../../../models/error';
import { ILogin } from '../../../models/auth';
import { ToastService } from '../../../services/toast.service';
import { RedirectKey } from '../../../shared/constants/redirectionKey';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  passwordFieldType: string = 'password';

  constructor(
    private readonly _route: Router,
    private readonly _fb: FormBuilder,
    private readonly _authService: AuthService,
    private readonly _toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.loginForm = this._fb.group({
      email: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  togglePassword(): void {
    this.passwordFieldType = this.passwordFieldType === 'password' ? 'text' : 'password';
  }

  isSubmitting: boolean = false;

  login(): void {
    if (this.loginForm.valid && !this.isSubmitting) {
      this.isSubmitting = true;

      this._authService.login(this.loginForm.value).subscribe({
        next: (response: ILogin): void => {
          this._toastService.showSuccess('Login Successfully');
          const userSession: IUserSession = {
            name: response.name,
            email: response.email,
            role: response.role,
            id: response.id
          };

          this._authService.storeUserSession(response.token, userSession);

          this._route.navigate([`${RedirectKey.REDIRECT_TO_DASHBOARD}`]);
        },
        error: (error: IErrorResponse): void => {
          this._toastService.showError(`Error fetching users: ${error.message}`);
          this.isSubmitting = false;
        },
        complete: (): void => {
          this.isSubmitting = false;
        }
      });
    }
  }
}
