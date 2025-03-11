import { Component, EventEmitter, Output } from '@angular/core';
import { ThemeService } from './../../services/theme.service';
import { SessionHelperService } from '../../core/session-helper.service';
import { IUserSession } from '../../models/UserSession ';

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
  storedUser: IUserSession | null = null;
  user: string = '';

  constructor(
    private readonly _themeService: ThemeService,
    private readonly _sessionHelper: SessionHelperService
  ) {
    this.storedUser = this._sessionHelper.getItem<IUserSession>('user');
    if (this.storedUser) {
      this.user = this.storedUser.name;
    }
  }

  toggleUserDropdown(): void {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  toggleThemeDropdown(): void {
    this.isThemeDropdownOpen = !this.isThemeDropdownOpen;
  }

  changeTheme(theme: string): void {
    this._themeService.setTheme(theme);
    this.isThemeDropdownOpen = false;
  }

  logout(): void {
    sessionStorage.removeItem('user');
    sessionStorage.removeItem('token');
    window.location.reload();
  }

  onToggleSidebar(): void {
    this.toggleSidebar.emit();
  }
}
