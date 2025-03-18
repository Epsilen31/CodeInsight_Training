import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { IUser } from '../../models/user';
import { ActivatedRoute, Router } from '@angular/router';
import { IErrorResponse } from '../../models/error';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss'],
  standalone: false,
})
export class UserComponent implements OnInit {
  users: IUser[] = [];

  constructor(
    private readonly userService: UserService,
    private readonly route: ActivatedRoute,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.getAllUsers();
  }

  private getAllUsers(): void {
    this.userService.getAllUsers().subscribe({
      next: (users: IUser[]): void => {
        this.users = users;
      },
      error: (error: IErrorResponse): void => {
        console.error('Error fetching users:', error);
      },
    });
  }

  viewUser(id: number): void {
    this.router.navigate([`/billing-subscription/user/get-user-by-id/${id}`]);
  }

  updateUser(id: number): void {
    this.router.navigate([`/billing-subscription/user/update-user/${id}`]);
  }

  deleteUser(id: number): void {
    this.userService.deleteUser(id).subscribe({
      next: (): void => {
        this.users = this.users.filter(
          (user: IUser): boolean => user.id !== id
        );
        console.log('User deleted successfully.');
      },
      error: (error: IErrorResponse): void => {
        console.error('Error deleting user:', error);
      },
    });
  }
}
