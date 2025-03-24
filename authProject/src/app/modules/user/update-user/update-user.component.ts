import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { IUpdateUser, IUserDetail } from '../../../models/user';
import { IErrorResponse } from '../../../models/error';
import { ToastService } from '../../../services/toast.service';

@Component({
  selector: 'app-update-user',
  standalone: false,
  templateUrl: './update-user.component.html',
  styleUrls: ['./update-user.component.scss']
})
export class UpdateUserComponent implements OnInit {
  userId!: number;
  user!: IUpdateUser;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly userService: UserService,
    private readonly _toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.userId = Number(this.route.snapshot.paramMap.get('id'));
    this.getUserDetails();
  }

  getUserDetails(): void {
    this.userService.getUserById(this.userId).subscribe({
      next: (data: IUserDetail): void => {
        this.user = data.user;
      },
      error: (error: IErrorResponse): void => {
        this._toastService.showError(`Error fetching user: ${error.message}`);
      }
    });
  }

  updateUser(): void {
    const userToUpdate = { ...this.user, role: { roleName: this.user.role } };
    this.userService.updateUser(this.userId, userToUpdate).subscribe({
      next: (): void => {
        ('User updated successfully.');
        this.router.navigate(['/billing-subscription/user/this.userId']);
      },
      error: (error: IErrorResponse): void => {
        this._toastService.showError(`Error fetching users: ${error.message}`);
      }
    });
  }

  cancel(): void {
    this.router.navigate(['/billing-subscription/user']);
  }
}
