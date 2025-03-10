import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  private currentTheme: string = localStorage.getItem('theme') ?? 'light';

  constructor() {
    this.applyTheme();
  }

  setTheme(theme: string): void {
    this.currentTheme = theme;
    console.log('setting the theme', this.currentTheme);
    localStorage.setItem('theme', theme);
    this.applyTheme();
  }

  getTheme(): string {
    console.log(this.currentTheme);
    return this.currentTheme;
  }

  private applyTheme(): void {
    const body: HTMLElement = document.body;

    if (this.currentTheme === 'dark') {
      body.classList.add('dark');
    } else {
      body.classList.remove('dark');
    }
  }
}
