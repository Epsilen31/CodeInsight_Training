import { Component, EventEmitter, Output } from '@angular/core';
import { ThemeService } from './../../services/theme.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss'],
  standalone: false,
})
export class NavBarComponent {
  @Output() toggleSidebar: EventEmitter<void> = new EventEmitter<void>();
  isDropdownOpen: boolean = false;
  isThemeDropdownOpen: boolean = false;
  storedUser: string | null = sessionStorage.getItem('user');
  user: string = this.storedUser ? JSON.parse(this.storedUser).name : 'Guest';

  constructor(public themeService: ThemeService) {}

  toggleUserDropdown(): void {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  toggleThemeDropdown(): void {
    this.isThemeDropdownOpen = !this.isThemeDropdownOpen;
  }

  changeTheme(theme: string): void {
    this.themeService.setTheme(theme);
    this.isThemeDropdownOpen = false;
  }

  logout(): void {
    sessionStorage.removeItem('user');
    sessionStorage.removeItem('token');
    window.location.reload();
  }

  onToggleSidebar(): void {
    console.log('Sidebar toggle clicked');
    this.toggleSidebar.emit();
  }
}
