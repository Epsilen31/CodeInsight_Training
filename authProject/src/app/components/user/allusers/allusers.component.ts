import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { ColDef, GridOptions, ValueGetterParams } from 'ag-grid-community';
import { Subscription } from 'rxjs';

import { UserService } from '../../../services/user.service';
import { NotificationService } from '../../../services/notification.service';
import { ToastService } from '../../../services/toast.service';
import { ThemeService } from '../../../services/theme.service';

import { IErrorResponse } from '../../../models/error';
import { IUser } from '../../../models/user';

import { ActionButtonComponent } from '../../action-button/action-button.component';
import { TableComponent } from '../../../shared/table/table.component';

@Component({
  selector: 'app-user',
  templateUrl: './allusers.component.html',
  styleUrls: ['./allusers.component.scss'],
  standalone: false
})
export class AllUsersComponent implements OnInit, OnDestroy {
  @ViewChild(TableComponent) tableComponent!: TableComponent<IUser>;

  users: IUser[] = [];
  notifications: string[] = [];

  showNotificationSection: boolean = false;
  isDarkMode: boolean = false;

  private notificationTimer: ReturnType<typeof setTimeout> | null = null;
  private themeSubscription?: Subscription;

  defaultColDef: ColDef = {
    sortable: true,
    filter: true,
    resizable: true,
    flex: 1
  };

  columnDefs: ColDef[] = this.getColumnDefs();

  gridOptions: GridOptions<IUser> = {
    defaultColDef: this.defaultColDef,
    domLayout: 'normal',
    ensureDomOrder: true,
    rowHeight: 70,
    suppressHorizontalScroll: false
  };

  constructor(
    private readonly _userService: UserService,
    private readonly _toastService: ToastService,
    private readonly _notificationService: NotificationService,
    private readonly _themeService: ThemeService
  ) {}

  ngOnInit(): void {
    this.subscribeToTheme();
    this.fetchUsers();
    this.broadcastNotifications();
  }

  ngOnDestroy(): void {
    this._notificationService.stopConnection();
    if (this.notificationTimer) clearTimeout(this.notificationTimer);
    this.themeSubscription?.unsubscribe();
  }

  private subscribeToTheme(): void {
    this.themeSubscription = this._themeService.theme$.subscribe((isDark: boolean) => {
      this.isDarkMode = isDark;
      document.body.dataset['agThemeMode'] = isDark ? 'dark-red' : 'light-red';

      if (this.tableComponent) {
        setTimeout(() => this.tableComponent.applyTheme(isDark), 0);
      }
    });
  }

  private fetchUsers(): void {
    this._userService.getAllUsers().subscribe({
      next: (users: IUser[]) => {
        this.users = users;
        if (!users?.length) {
          this._toastService.showInfo('No users found in the database.');
        } else {
          this._toastService.showSuccess(`Fetched ${users.length} users successfully.`);
        }
      },
      error: (error: IErrorResponse) => {
        this._toastService.showError(`Error fetching users: ${error.message}`);
      }
    });
  }

  private broadcastNotifications(): void {
    this._notificationService.listenForMessages((message: string) => {
      this.notifications.unshift(message);
      this.showNotificationSection = true;

      if (this.notificationTimer) clearTimeout(this.notificationTimer);

      this.notificationTimer = setTimeout(() => {
        this.showNotificationSection = false;
      }, 5000);

      this._toastService.showInfo(message);
    });
  }

  private getColumnDefs(): ColDef[] {
    return [
      { headerName: 'ID', field: 'id' },
      { headerName: 'Name', field: 'name' },
      { headerName: 'Email', field: 'email' },
      { headerName: 'Phone', field: 'phone' },
      {
        headerName: 'Role',
        field: 'role.roleName',
        valueGetter: (params: ValueGetterParams<IUser, string>) => params.data?.role?.roleName ?? ''
      },
      {
        headerName: 'Actions',
        field: 'actions',
        cellRenderer: ActionButtonComponent
      }
    ];
  }
}
