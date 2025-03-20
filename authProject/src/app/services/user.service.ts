import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { IUser, IUserDetail, IUserResponse } from '../models/user';
import { HttpClientService } from '../core/http-client.service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private readonly httpClientService: HttpClientService) {}

  getAllUsers(): Observable<IUser[]> {
    return this.httpClientService
      .get<IUserResponse>('User/GetUsers')
      .pipe(map((response: IUserResponse): IUser[] => response.users));
  }

  getUserById(id: number): Observable<IUserDetail> {
    return this.httpClientService.get<IUserDetail>(`User/GetUserById/${id}`);
  }

  addUser(user: IUser): Observable<IUser> {
    return this.httpClientService.post<IUser>('User/AddUser', user);
  }

  updateUser(id: number, user: IUser): Observable<IUser> {
    return this.httpClientService.put<IUser>(`User/UpdateUser/${id}`, user);
  }

  deleteUser(id: number): Observable<void> {
    return this.httpClientService.delete<void>(`User/DeleteUser/${id}`);
  }
}
