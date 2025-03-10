export interface ISubMenu {
  id: number;
  title: string;
  path: string;
  isActive: boolean;
}

export interface IMenu {
  id: number;
  title: string;
  path: string;
  isActive: boolean;
  role: string;
  subMenus: ISubMenu[];
  isDropdownOpen?: boolean;
  icon?: string;
}

export interface IMenuResponse {
  message: string;
  menu: IMenu[];
}
