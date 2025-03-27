import { Component, EventEmitter, Output, OnInit } from '@angular/core';
import { SessionHelperService } from '../../core/session-helper.service';
import { ThemeService } from '../../services/theme.service';
import { IUserSession } from '../../models/userSession ';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss'],
  standalone: false
})
export class NavBarComponent implements OnInit {
  @Output() toggleSidebar: EventEmitter<void> = new EventEmitter<void>();
  isDropdownOpen: boolean = false;
  user: string = '';
  isDarkMode!: boolean;

  constructor(
    private readonly _sessionHelper: SessionHelperService,
    private readonly _themeService: ThemeService
  ) {
    const storedUser: IUserSession | null = this._sessionHelper.getItem<IUserSession>('user');
    this.user = storedUser ? storedUser.name : 'Guest';
  }

  ngOnInit(): void {
    this._themeService.theme$.subscribe((isDark: boolean) => {
      this.isDarkMode = isDark;
    });
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
    this._themeService.setTheme(!this.isDarkMode);
  }
}
