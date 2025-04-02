import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { IUser, IUserDetail, IUserResponse } from '../models/user';
import { HttpClientService } from '../core/http-client.service';
import { RouteKey } from '../shared/constants/routeKey';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private readonly _httpClientService: HttpClientService) {}

  getAllUsers(): Observable<IUser[]> {
    return this._httpClientService
      .get<IUserResponse>(`${RouteKey.GET_ALL_USERS_URL}`)
      .pipe(map((response: IUserResponse): IUser[] => response.users));
  }

  getUserById(id: number): Observable<IUserDetail> {
    return this._httpClientService.get<IUserDetail>(`${RouteKey.GET_USER_BY_ID_URL}/${id}`);
  }

  addUser(user: IUser): Observable<IUser> {
    return this._httpClientService.post<IUser>(`${RouteKey.CREATE_USER_URL}`, user);
  }

  updateUser(id: number, user: IUser): Observable<IUser> {
    return this._httpClientService.put<IUser>(`${RouteKey.UPDATE_USER_URL}/${id}`, user);
  }

  deleteUser(id: number): Observable<void> {
    return this._httpClientService.delete<void>(`${RouteKey.DELETE_USER_URL}/${id}`);
  }
}
