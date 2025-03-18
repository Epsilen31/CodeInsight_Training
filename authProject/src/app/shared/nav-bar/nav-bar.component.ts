import { Component, EventEmitter, Output, OnInit } from '@angular/core';
import { SessionHelperService } from '../../core/session-helper.service';
import { IUserSession } from '../../models/UserSession ';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss'],
  standalone: false,
})
export class NavBarComponent implements OnInit {
  @Output() toggleSidebar: EventEmitter<void> = new EventEmitter<void>();
  @Output() toggleDarkMode: EventEmitter<void> = new EventEmitter<void>();
  isDropdownOpen: boolean = false;
  user: string = '';
  isDarkMode: boolean = false;

  constructor(private readonly _sessionHelper: SessionHelperService) {
    const storedUser: IUserSession | null =
      this._sessionHelper.getItem<IUserSession>('user');
    this.user = storedUser ? storedUser.name : 'Guest';
  }

  ngOnInit(): void {
    const storedTheme: string | null = localStorage.getItem('theme');
    if (storedTheme === 'dark') {
      this.isDarkMode = true;
    }
  }

  toggleUserDropdown(): void {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  logout(): void {
    sessionStorage.clear();
    window.location.reload();
  }

  onToggleSidebar(): void {
    this.toggleSidebar.emit();
  }

  ToggleDarkMode(): void {
    this.isDarkMode = !this.isDarkMode;
    this.toggleDarkMode.emit();
  }
}
