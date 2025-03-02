import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { IUser } from '../models/user';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl: string =
    'http://localhost:5062/api/BillingAndSubscription/Auth';

  constructor(private http: HttpClient, private router: Router) {}

  login(data: { email: string; password: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, data);
  }

  register(user: IUser): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, user);
  }

  storeUser(token: string, username: string): void {
    sessionStorage.setItem('token', token);
    sessionStorage.setItem('user', username);
  }

  isLoggedIn(): boolean {
    return !!sessionStorage.getItem('token');
  }

  getUser(): string | null {
    return sessionStorage.getItem('user');
  }

  logout(): void {
    sessionStorage.removeItem('token');
    sessionStorage.removeItem('user');
    this.router.navigate(['/login']);
  }
}
