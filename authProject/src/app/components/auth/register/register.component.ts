import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;
  passwordFieldType: string = 'password';

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      role: ['Admin', Validators.required],
    });
  }

  getRoleId(): number {
    return this.registerForm.get('role')?.value === 'Admin' ? 1 : 2;
  }

  togglePassword(): void {
    this.passwordFieldType =
      this.passwordFieldType === 'password' ? 'text' : 'password';
  }

  register(): void {
    if (this.registerForm.valid) {
      const { role, ...formData } = this.registerForm.value;
      const updatedFormData = {
        ...formData,
        roleId: this.getRoleId(),
      };

      console.log('register', updatedFormData);

      this.authService.register(updatedFormData).subscribe({
        next: () => {
          this.router.navigate(['/login']);
        },
        error: (error) => {
          console.error('Registration Failed:', error);
        },
      });
    }
  }
}
