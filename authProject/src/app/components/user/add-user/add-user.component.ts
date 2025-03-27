import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../../../services/user.service';
import { ToastService } from '../../../services/toast.service';
import { IUser } from '../../../models/user';
import { Router } from '@angular/router';
import { ThemeService } from '../../../services/theme.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.scss'],
  standalone: false
})
export class AddUserComponent implements OnInit, OnDestroy {
  userForm: FormGroup;
  isDarkMode = false;
  private themeSub?: Subscription;

  roleOptions = [
    { label: 'Admin', value: 1 },
    { label: 'User', value: 2 }
  ];

  constructor(
    private readonly _fb: FormBuilder,
    private readonly _userService: UserService,
    private readonly _toastService: ToastService,
    private readonly _router: Router,
    private readonly _themeService: ThemeService
  ) {
    this.userForm = this._fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
      name: ['', [Validators.required]],
      phone: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],
      roleId: [null, [Validators.required]] // roleId as number
    });
  }

  ngOnInit(): void {
    this.themeSub = this._themeService.theme$.subscribe((dark) => {
      this.isDarkMode = dark;
    });
  }

  ngOnDestroy(): void {
    this.themeSub?.unsubscribe();
  }

  onSubmit(): void {
    if (this.userForm.invalid) {
      this._toastService.showError('Please fill all required fields correctly.');
      return;
    }

    const formValue = this.userForm.value;

    const user: IUser = {
      ...formValue,
      roleId: Number(formValue.roleId)
    };

    console.log('user', user); // debugging
    this._userService.addUser(user).subscribe({
      next: () => {
        this._toastService.showSuccess('User added successfully!');
        this._router.navigate(['/users']);
      },
      error: (error) => {
        this._toastService.showError(`Failed to add user: ${error.message}`);
      }
    });
  }
}
