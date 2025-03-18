import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { NotificationService } from '../../../services/notification.service';
import { IErrorResponse } from '../../../models/error';
import { IUser } from '../../../models/user';
import {
  ColDef,
  GridApi,
  GridOptions,
  GridReadyEvent,
  Theme,
  ThemeDefaultParams,
  themeQuartz,
} from 'ag-grid-community';
import { ActionButtonComponent } from '../../../components/action-button/action-button.component';
import { ToastService } from '../../../services/toast.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss'],
  standalone: false,
})
export class UserComponent implements OnInit, OnDestroy {
  users: IUser[] = [];
  notifications: string[] = [];
  @Input() isDarkMode: boolean = false;
  gridApi!: GridApi;

  columnDefs: ColDef[] = [
    { headerName: 'ID', field: 'id', sortable: true, filter: true },
    { headerName: 'Name', field: 'name', sortable: true, filter: true },
    { headerName: 'Email', field: 'email', sortable: true, filter: true },
    { headerName: 'Phone', field: 'phone', sortable: true, filter: true },
    {
      headerName: 'Role',
      field: 'role',
      sortable: true,
      filter: true,
      valueGetter: (params: any) => params.data.role.roleName,
    },
    {
      headerName: 'Actions',
      field: 'actions',
      cellRenderer: ActionButtonComponent,
    },
  ];

  defaultColDef: ColDef = {
    flex: 1,
    minWidth: 100,
    resizable: true,
  };

  constructor(
    private readonly userService: UserService,
    private readonly toastService: ToastService,
    private readonly _notificationService: NotificationService,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.fetchUsers();
    this.setupNotifications();
  }

  ngOnChanges(): void {
    this.setDarkMode(this.isDarkMode);
  }

  ngOnDestroy(): void {
    this._notificationService.stopConnection();
  }

  private fetchUsers(): void {
    this.userService.getAllUsers().subscribe({
      next: (data: any): void => {
        this.users = data.users;
        if (this.users.length === 0) {
          this.toastService.showInfo('No users found in the database.', 'Info');
        } else {
          this.toastService.showSuccess(
            `Fetched ${this.users.length} users successfully.`,
            'Success'
          );
        }
      },
      error: (error: IErrorResponse): void => {
        this.toastService.showError(
          `Error fetching users: ${error.message}`,
          'Error'
        );
      },
    });
  }

  private setupNotifications(): void {
    this._notificationService.startConnection();
    this._notificationService.listenForMessages((message: string): void => {
      this.notifications.unshift(message);
      this.toastService.showInfo(` ${message}`, 'New Notification');
    });
  }

  onGridReady(params: GridReadyEvent): void {
    this.gridApi = params.api;
  }

  myTheme: Theme<ThemeDefaultParams> = themeQuartz
    .withParams(
      {
        backgroundColor: '#FFE8E0',
        foregroundColor: '#361008CC',
        browserColorScheme: 'light',
      },
      'light-red'
    )
    .withParams(
      {
        backgroundColor: '#201008',
        foregroundColor: '#FFFFFFCC',
        browserColorScheme: 'dark',
      },
      'dark-red'
    );

  theme: Theme = this.myTheme;

  gridOptions: GridOptions<IUser> = {
    theme: this.theme,
    columnDefs: this.columnDefs,
    defaultColDef: this.defaultColDef,
    sideBar: true,
  };

  setDarkMode(enabled: any): void {
    this.isDarkMode = enabled;

    document.body.dataset['agThemeMode'] = enabled ? 'dark-red' : 'light-red';
    localStorage.setItem('theme', enabled ? 'dark' : 'light');
    this.applyTheme();
  }

  applyTheme(): void {
    this.theme = this.isDarkMode
      ? this.myTheme.withParams({ browserColorScheme: 'dark' }, 'dark-red')
      : this.myTheme.withParams({ browserColorScheme: 'light' }, 'light-red');

    this.gridOptions = { ...this.gridOptions, theme: this.theme };
  }
}
