import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { IUserSession } from './../models/UserSession ';

@Injectable({
  providedIn: 'root',
})
export class SessionHelperService {
  constructor(private readonly _route: Router) {}

  storeUser(
    token: string,
    user: { name: string; email: string; role: string }
  ): void {
    sessionStorage.setItem('token', token);
    sessionStorage.setItem('user', JSON.stringify(user));
  }

  getToken(): string | null {
    return sessionStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  getUser(): IUserSession | null {
    const userData: string | null = sessionStorage.getItem('user');
    return userData ? JSON.parse(userData) : null;
  }

  getUserRole(): string | null {
    const user: IUserSession | null = this.getUser();
    return user ? user.role : null;
  }

  clearSession(): void {
    sessionStorage.clear();
    this._route.navigate(['/login']).then((): void => {
      window.location.reload();
    });
  }
}
