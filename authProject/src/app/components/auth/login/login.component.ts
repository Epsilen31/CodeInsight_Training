import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

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
    private router: Router,
    private fb: FormBuilder,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
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

      this.authService.login(this.loginForm.value).subscribe({
        next: (response) => {
          console.log('Login Successful:', response);
          this.authService.storeUser(response.token, {
            name: response.name,
            email: response.email,
            role: response.role,
          });

          this.router.navigate(['/dashboard']);
        },
        error: (error) => {
          console.error('Login Failed:', error);
          this.isSubmitting = false;
        },
        complete: () => {
          this.isSubmitting = false;
        },
      });
    }
  }
}
