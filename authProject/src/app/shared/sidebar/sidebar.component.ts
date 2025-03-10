import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { MenuService } from '../../services/menu.service';
import { IErrorResponse } from '../../models/error';
import { IMenu, ISubMenu } from '../../models/sidebar';
import { DashboardService } from '../../services/dashboard.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss'],
  standalone: false,
})
export class SidebarComponent implements OnInit {
  @Input() isLeftSidebarCollapsed = false;
  @Output() toggleLeftSidebar = new EventEmitter<boolean>();
  menuItems: IMenu[] = [];

  constructor(
    private readonly _menuService: MenuService,
    private readonly _dashboardService: DashboardService
  ) {}

  ngOnInit(): void {
    this._menuService.getAllMenu().subscribe({
      next: (menu: IMenu[]): void => {
        this.menuItems = menu.map(
          (item: IMenu): IMenu => ({
            ...item,
            title:
              item.title === 'User Subscription' ? 'Subscription' : item.title,
            isDropdownOpen: false,
            subMenus: item.subMenus
              ? item.subMenus.map((sub: ISubMenu): ISubMenu => ({ ...sub }))
              : [],
          })
        );
        console.log('Menu items:', this.menuItems);
      },
      error: (error: IErrorResponse): void => {
        console.error('Error fetching menu items:', error);
      },
    });
  }

  // toggleCollapse(): void {
  //   this.isLeftSidebarCollapsed = !this.isLeftSidebarCollapsed;
  //   this.toggleLeftSidebar.emit(this.isLeftSidebarCollapsed);
  // }

  toggleDropdown(menuItem: IMenu, event: Event): void {
    event.stopPropagation();
    menuItem.isDropdownOpen = !menuItem.isDropdownOpen;
  }

  handleSubmenuClick(subMenuItem: ISubMenu): void {
    if (subMenuItem.title.includes('Get All Users')) {
      this._dashboardService.setActiveComponent('UserComponent');
    }
  }
}
