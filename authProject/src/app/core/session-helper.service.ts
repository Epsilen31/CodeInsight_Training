import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class SessionHelperService {
  constructor(private readonly _router: Router) {}

  storeItem<T>(key: string, data: T): void {
    const value: string = typeof data === 'string' ? data : JSON.stringify(data);
    sessionStorage.setItem(key, value);
  }

  getItem<T>(key: string): T | null {
    const storedData: string | null = sessionStorage.getItem(key);
    const data: T = storedData ? JSON.parse(storedData) : null;
    return data;
  }

  removeItem(key: string): void {
    sessionStorage.removeItem(key);
  }

  clearSession(redirectUrl?: string): void {
    sessionStorage.clear();
    if (redirectUrl) {
      this._router.navigate([redirectUrl]).then((): void => {
        window.location.reload();
      });
    }
  }

  isLoggedIn(tokenKey: string = 'token'): boolean {
    return !!sessionStorage.getItem(tokenKey);
  }
}
