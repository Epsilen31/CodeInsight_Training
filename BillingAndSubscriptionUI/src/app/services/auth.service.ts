import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from '../core/http-client.service';
import { SessionHelperService } from '../core/session-helper.service';
import { ILogin, IRegisterUser } from '../models/auth';
import { IUserSession } from '../models/userSession ';
import { RouteKey } from '../shared/constants/routeKey';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(
    private readonly _httpClient: HttpClientService,
    private readonly _sessionService: SessionHelperService
  ) {}

  login(data: ILogin): Observable<ILogin> {
    return this._httpClient.post<ILogin>(`${RouteKey.AUTH_LOGIN_URL}`, data);
  }

  register(user: IRegisterUser): Observable<IRegisterUser> {
    return this._httpClient.post<IRegisterUser>(`${RouteKey.AUTH_REGISTER_URL}`, user);
  }

  storeUserSession(token: string, user: IUserSession): void {
    this._sessionService.storeItem<string>('token', token);
    this._sessionService.storeItem<IUserSession>('user', user);
  }

  isLoggedIn(): boolean {
    return this._sessionService.isLoggedIn();
  }
}
