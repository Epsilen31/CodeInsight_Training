import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { IUser } from '../models/user';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl: string =
    'http://localhost:5062/api/BillingAndSubscription/Auth';

  constructor(private http: HttpClient, private router: Router) {}

  login(data: {
    email: string;
    password: string;
  }): Observable<{ token: string; email: string; name: string; role: string }> {
    return this.http.post<{
      token: string;
      email: string;
      name: string;
      role: string;
    }>(`${this.apiUrl}/login`, data, {
      headers: { 'Content-Type': 'application/json' },
    });
  }

  register(user: IUser): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, user);
  }

  storeUser(
    token: string,
    user: { name: string; email: string; role: string }
  ): void {
    sessionStorage.setItem('token', token);
    sessionStorage.setItem('user', JSON.stringify(user));

    console.log('Token Stored:', sessionStorage.getItem('token'));
    console.log('User Data Stored:', sessionStorage.getItem('user'));
  }

  isLoggedIn(): boolean {
    return !!sessionStorage.getItem('token');
  }

  // getUser(): { name: string; email: string; role: string } | null {
  //   const userData = sessionStorage.getItem('user');
  //   return userData ? JSON.parse(userData) : null;
  // }

  // getUserRole(): string | null {
  //   const user = this.getUser();
  //   return user ? user.role : null;
  // }

  logout(): void {
    sessionStorage.clear();
    this.router.navigate(['/login']).then(() => {
      window.location.reload();
    });
  }
}
