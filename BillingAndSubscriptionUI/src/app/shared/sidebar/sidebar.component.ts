import { Component, Input, OnInit } from '@angular/core';
import { MenuService } from '../../services/menu.service';
import { IErrorResponse } from '../../models/error';
import { IMenu, ISubMenu } from '../../models/sidebar';
import { SessionHelperService } from '../../core/session-helper.service';
import { ToastService } from '../../services/toast.service';
import { ThemeService } from '../../services/theme.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss'],
  standalone: false
})
export class SidebarComponent implements OnInit {
  isDarkMode: boolean = false;
  @Input() isLeftSidebarCollapsed: boolean = false;

  menuItems: IMenu[] = [];
  id: number = -1;

  constructor(
    private readonly _menuService: MenuService,
    private readonly _sessionHelper: SessionHelperService,
    private readonly _toastService: ToastService,
    private readonly _themeService: ThemeService
  ) {
    const storedUser: { id: number } | null = this._sessionHelper.getItem<{ id: number }>('user');
    this.id = storedUser ? storedUser.id : -1;
  }

  ngOnInit(): void {
    this._themeService.theme$.subscribe((isDark) => {
      this.isDarkMode = isDark;
    });

    const titlesToExclude = ['View User Details', 'Edit User Profile', 'Remove User'];

    const shouldIncludeSubMenu = (sub: ISubMenu): boolean => {
      return !titlesToExclude.some(this.matchesExcludedTitle(sub.title));
    };

    this._menuService.getAllMenu().subscribe({
      next: (menu: IMenu[]): void => {
        this.menuItems = menu.map(
          (item: IMenu): IMenu => ({
            ...item,
            title: item.title,
            isDropdownOpen: false,
            subMenus: item.subMenus
              ? item.subMenus
                  .filter(shouldIncludeSubMenu)
                  .map((sub: ISubMenu): ISubMenu => ({ ...sub }))
              : []
          })
        );
      },
      error: (error: IErrorResponse): void => {
        this._toastService.showError(`Error fetching menus: ${error.message}`);
      }
    });
  }

  private matchesExcludedTitle(title: string) {
    return (excludeTitle: string): boolean => {
      return title.toLowerCase() === excludeTitle.toLowerCase();
    };
  }

  toggleDropdown(menuItem: IMenu, event: Event): void {
    event.stopPropagation();
    menuItem.isDropdownOpen = !menuItem.isDropdownOpen;
  }
}
