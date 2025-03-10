import { Component, OnInit } from '@angular/core';
import { HttpClientService } from '../../core/http-client.service';
import { IUser } from '../../models/user';
import { IErrorResponse } from '../../models/error';

@Component({
  selector: 'app-user',
  standalone: false,
  templateUrl: './user.component.html',
  styleUrl: './user.component.scss',
})
export class UserComponent implements OnInit {
  users: IUser[] = [];

  constructor(private readonly _httpService: HttpClientService) {}

  ngOnInit(): void {
    this.getAllUsers();
  }

  getAllUsers(): void {
    const apiPath = 'User/GetUsers';

    this._httpService.get<{ users: IUser[] }>(apiPath).subscribe({
      next: (data: { users: IUser[] }): void => {
        this.users = data.users;
        console.log(this.users);
      },
      error: (error: IErrorResponse): void => {
        console.error('Error fetching users:', error);
      },
    });
  }
}
