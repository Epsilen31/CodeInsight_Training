import { Component, OnInit, OnDestroy, Input, OnChanges } from '@angular/core';
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
  ValueGetterParams,
} from 'ag-grid-community';
import { ActionButtonComponent } from '../../../components/action-button/action-button.component';
import { ToastService } from '../../../services/toast.service';
import { myTheme } from '../../shared/theme-config';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss'],
  standalone: false,
})
export class UserComponent implements OnInit, OnDestroy, OnChanges {
  @Input() isDarkMode: boolean = false;
  users: IUser[] = [];
  notifications: string[] = [];
  gridApi!: GridApi;
  progress: number = 0;
  showProgressBar: boolean = true;

  columnDefs: ColDef[] = this.getColumnDefs();

  defaultColDef: ColDef = {
    flex: 1,
    sortable: true,
    filter: true,
  };

  theme: Theme = myTheme;

  gridOptions: GridOptions<IUser> = {
    theme: this.theme,
    columnDefs: this.columnDefs,
    defaultColDef: this.defaultColDef,
    domLayout: 'autoHeight',
    ensureDomOrder: true,
  };

  constructor(
    private readonly userService: UserService,
    private readonly toastService: ToastService,
    private readonly _notificationService: NotificationService
  ) {}

  ngOnInit(): void {
    this.fetchUsers();
    this.bradcastNotifications();
  }

  ngOnChanges(): void {
    this.setDarkMode(this.isDarkMode);
  }

  ngOnDestroy(): void {
    this._notificationService.stopConnection();
  }

  startProgress(): void {
    this.progress = 0;
  }

  hideProgressBar(): void {
    this.showProgressBar = false;
  }

  private fetchUsers(): void {
    this.userService.getAllUsers().subscribe({
      next: (users: IUser[]): void => {
        this.users = users;
        console.log('this.users', this.users);
        if (!this.users?.length) {
          this.toastService.showInfo('No users found in the database.');
        } else {
          this.toastService.showSuccess(
            `Fetched ${this.users.length} users successfully.`
          );
        }
      },
      error: (error: IErrorResponse): void => {
        this.toastService.showError(`Error fetching users: ${error.message}`);
      },
    });
  }

  private bradcastNotifications(): void {
    this._notificationService.listenForMessages((message: string): void => {
      this.notifications.unshift(message);
      this.toastService.showInfo(` ${message}`);
    });
  }

  onGridReady(params: GridReadyEvent<IUser>): void {
    this.gridApi = params.api;
  }

  setDarkMode(enabled: boolean): void {
    this.isDarkMode = enabled;
    document.body.dataset['agThemeMode'] = enabled ? 'dark-red' : 'light-red';
    localStorage.setItem('theme', enabled ? 'dark' : 'light');
    this.applyTheme();
  }

  applyTheme(): void {
    this.theme = this.isDarkMode
      ? myTheme.withParams({ browserColorScheme: 'dark' }, 'dark-red')
      : myTheme.withParams({ browserColorScheme: 'light' }, 'light-red');

    this.gridOptions = { ...this.gridOptions, theme: this.theme };
  }

  private getColumnDefs(): ColDef[] {
    return [
      { headerName: 'ID', field: 'id', width: 100 },
      { headerName: 'Name', field: 'name', width: 200 },
      { headerName: 'Email', field: 'email', width: 250 },
      { headerName: 'Phone', field: 'phone', width: 200 },
      {
        headerName: 'Role',
        field: 'role.roleName',
        width: 200,
        valueGetter: (
          params: ValueGetterParams<IUser, string>
        ): string | null => params.data?.role?.roleName ?? null,
      },
      {
        headerName: 'Actions',
        field: 'actions',
        width: 150,
        cellRenderer: ActionButtonComponent,
      },
    ];
  }
}
