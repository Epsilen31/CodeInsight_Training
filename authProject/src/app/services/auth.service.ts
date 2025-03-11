import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SessionHelperService } from '../core/session-helper.service';
import { IUserSession } from '../models/UserSession ';
import { ILogin, IRegisterUser } from '../models/auth';
import { HttpClientService } from '../core/http-client.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(
    private readonly _httpClient: HttpClientService,
    private readonly _sessionService: SessionHelperService
  ) {}

  login(data: ILogin): Observable<ILogin> {
    return this._httpClient.post<ILogin>('Auth/login', data);
  }

  register(user: IRegisterUser): Observable<IRegisterUser> {
    return this._httpClient.post<IRegisterUser>('Auth/register', user);
  }

  storeUserSession(token: string, user: IUserSession): void {
    this._sessionService.storeItem<string>('token', token);
    this._sessionService.storeItem<IUserSession>('user', user);
  }

  isLoggedIn(): boolean {
    return this._sessionService.isLoggedIn();
  }
}
