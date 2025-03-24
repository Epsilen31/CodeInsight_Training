import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private readonly themeSubject: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(
    localStorage.getItem('theme') === 'dark'
  );

  theme$: Observable<boolean> = this.themeSubject.asObservable();

  setTheme(isDarkMode: boolean): void {
    localStorage.setItem('theme', isDarkMode ? 'dark' : 'light');
    document.body.classList.toggle('dark-mode', isDarkMode);
    this.themeSubject.next(isDarkMode);
  }

  getTheme(): boolean {
    return this.themeSubject.value;
  }
}
