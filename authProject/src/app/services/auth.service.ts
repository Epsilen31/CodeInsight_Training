import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IUser } from '../models/user';
import { SessionHelperService } from '../core/session-helper.service';
import { AuthHubService } from '../components/auth-hub.service';
import { IUserSession } from '../models/UserSession ';
import { ILogin } from '../models/login';
import { IRegisterResponse } from '../models/Register';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(
    private http: HttpClient,
    private sessionService: SessionHelperService,
    private authHubService: AuthHubService
  ) {}
  login(data: { email: string; password: string }): Observable<ILogin> {
    return this.http.post<ILogin>(`${this.authHubService.apiUrl}/login`, data, {
      headers: { 'Content-Type': 'application/json' },
    });
  }

  register(user: IUser): Observable<IRegisterResponse> {
    return this.http.post<IRegisterResponse>(
      `${this.authHubService.apiUrl}/register`,
      user
    );
  }

  storeUserSession(token: string, user: IUserSession): void {
    this.sessionService.storeUser(token, user);
  }
  isLoggedIn(): boolean {
    return this.sessionService.isLoggedIn();
  }
}
