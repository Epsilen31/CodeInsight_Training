import { Component, OnInit } from '@angular/core';
import { MenuService } from '../../services/menu.service';
import { IErrorResponse } from '../../models/error';
import { IMenu, ISubMenu } from '../../models/sidebar';

@Component({
  selector: 'app-sidebar',
  standalone: false,
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss'],
})
export class SidebarComponent implements OnInit {
  menuItems: IMenu[] = [];

  constructor(private readonly _menuService: MenuService) {}

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
      },
      error: (error: IErrorResponse): void => {
        console.error('Error fetching menu items:', error);
      },
    });
  }

  toggleDropdown(menuItem: IMenu): void {
    menuItem.isDropdownOpen = !menuItem.isDropdownOpen;
  }
}
