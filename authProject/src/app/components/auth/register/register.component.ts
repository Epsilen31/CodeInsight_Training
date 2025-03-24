import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { Role } from '../../../libs/enums/role';
import { IErrorResponse } from '../../../models/error';
import { ToastService } from '../../../services/toast.service';

@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;
  passwordFieldType: string = 'password';

  constructor(
    private readonly _router: Router,
    private readonly _fb: FormBuilder,
    private readonly _authService: AuthService,
    private readonly _toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.registerForm = this._fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      role: ['Admin', Validators.required]
    });
  }

  private getRoleId(): number {
    return this.registerForm.get('role')?.value === 'Admin' ? Role.Admin : Role.User;
  }

  togglePassword(): void {
    this.passwordFieldType = this.passwordFieldType === 'password' ? 'text' : 'password';
  }

  register(): void {
    if (this.registerForm.valid) {
      const { ...formData } = this.registerForm.value;
      const updatedFormData = {
        ...formData,
        roleId: this.getRoleId()
      };

      this._authService.register(updatedFormData).subscribe({
        next: (): void => {
          this._router.navigate(['/login']);
        },
        error: (error: IErrorResponse): void => {
          this._toastService.showError(`Error fetching users: ${error.message}`);
        }
      });
    }
  }
}
