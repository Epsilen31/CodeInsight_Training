import { Component, Input, OnInit } from '@angular/core';
import { MenuService } from '../../services/menu.service';
import { IErrorResponse } from '../../models/error';
import { IMenu, ISubMenu } from '../../models/sidebar';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss'],
  standalone: false,
})
export class SidebarComponent implements OnInit {
  @Input() isLeftSidebarCollapsed: boolean = false;
  menuItems: IMenu[] = [];

  constructor(private readonly _menuService: MenuService) {}

  ngOnInit(): void {
    this._menuService.getAllMenu().subscribe({
      next: (menu: IMenu[]): void => {
        this.menuItems = menu.map(
          (item: IMenu): IMenu => ({
            ...item,
            title: item.title,
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

  toggleDropdown(menuItem: IMenu, event: Event): void {
    event.stopPropagation();
    menuItem.isDropdownOpen = !menuItem.isDropdownOpen;
  }

  handleSubmenuClick(subMenuItem: ISubMenu): void {
    console.log('Submenu clicked:', subMenuItem.title);
  }
}
