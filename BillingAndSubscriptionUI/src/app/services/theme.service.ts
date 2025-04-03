import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private readonly themeSubject: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(
    this.getThemeFromStorage()
  );

  theme$: Observable<boolean> = this.themeSubject.asObservable();

  constructor() {
    this.initializeTheme();
  }

  private getThemeFromStorage(): boolean {
    const storedTheme = localStorage.getItem('theme');
    return storedTheme === 'dark' || storedTheme === null;
  }

  private initializeTheme(): void {
    const isDarkMode = this.getThemeFromStorage();
    document.body.classList.toggle('dark-mode', isDarkMode);
    this.themeSubject.next(isDarkMode);
  }

  setTheme(isDarkMode: boolean): void {
    localStorage.setItem('theme', isDarkMode ? 'dark' : 'light');
    document.body.classList.toggle('dark-mode', isDarkMode);
    this.themeSubject.next(isDarkMode);
  }

  getTheme(): boolean {
    return this.themeSubject.value;
  }
}
