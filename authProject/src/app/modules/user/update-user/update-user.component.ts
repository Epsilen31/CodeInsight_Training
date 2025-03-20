import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { IUpdateUser, IUserDetail } from '../../../models/user';
import { IErrorResponse } from '../../../models/error';

@Component({
  selector: 'app-update-user',
  standalone: false,
  templateUrl: './update-user.component.html',
  styleUrls: ['./update-user.component.scss'],
})
export class UpdateUserComponent implements OnInit {
  userId!: number;
  user!: IUpdateUser;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly userService: UserService
  ) {}

  ngOnInit(): void {
    this.userId = Number(this.route.snapshot.paramMap.get('id'));
    this.getUserDetails();
  }

  getUserDetails(): void {
    this.userService.getUserById(this.userId).subscribe({
      next: (data: IUserDetail): void => {
        this.user = data.user;
        console.log('update user', this.user);
      },
      error: (error: IErrorResponse): void => {
        console.error('Error fetching user:', error);
      },
    });
  }

  updateUser(): void {
    const userToUpdate = { ...this.user, role: { roleName: this.user.role } };
    this.userService.updateUser(this.userId, userToUpdate).subscribe({
      next: (): void => {
        console.log('User updated successfully.');
        this.router.navigate(['/billing-subscription/user/this.userId']);
      },
      error: (error: IErrorResponse): void => {
        console.error('Error updating user:', error);
      },
    });
  }

  cancel(): void {
    this.router.navigate(['/billing-subscription/user']);
  }
}
