import { Component, OnInit, OnDestroy, Input, OnChanges, ViewChild } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { NotificationService } from '../../../services/notification.service';
import { IErrorResponse } from '../../../models/error';
import { IUser } from '../../../models/user';
import { ColDef, GridOptions, ValueGetterParams } from 'ag-grid-community';
import { ToastService } from '../../../services/toast.service';
import { ActionButtonComponent } from '../../../components/action-button/action-button.component';
import { TableComponent } from '../../shared/table/table.component';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss'],
  standalone: false
})
export class UserComponent implements OnInit, OnDestroy, OnChanges {
  @Input() isDarkMode: boolean = false;
  users: IUser[] = [];
  notifications: string[] = [];
  progress: number = 0;
  showProgressBar: boolean = true;

  columnDefs: ColDef[] = this.getColumnDefs();
  defaultColDef: ColDef = { flex: 1, sortable: true, filter: true };

  gridOptions: GridOptions<IUser> = {
    defaultColDef: { flex: 1, sortable: true, filter: true },
    domLayout: 'autoHeight',
    ensureDomOrder: true
  };

  @ViewChild(TableComponent) tableComponent!: TableComponent<IUser>;

  constructor(
    private readonly userService: UserService,
    private readonly toastService: ToastService,
    private readonly _notificationService: NotificationService
  ) {}

  ngOnInit(): void {
    this.fetchUsers();
    this.broadcastNotifications();
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
        if (!this.users?.length) {
          this.toastService.showInfo('No users found in the database.');
        } else {
          this.toastService.showSuccess(`Fetched ${this.users.length} users successfully.`);
        }
      },
      error: (error: IErrorResponse): void => {
        this.toastService.showError(`Error fetching users: ${error.message}`);
      }
    });
  }

  private broadcastNotifications(): void {
    this._notificationService.listenForMessages((message: string): void => {
      this.notifications.unshift(message);
      this.toastService.showInfo(` ${message}`);
    });
  }

  setDarkMode(enabled: boolean): void {
    this.isDarkMode = enabled;
    document.body.dataset['agThemeMode'] = enabled ? 'dark-red' : 'light-red';
    localStorage.setItem('theme', enabled ? 'dark' : 'light');
    if (this.tableComponent) {
      this.tableComponent.applyTheme();
    }
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
        valueGetter: (params: ValueGetterParams<IUser, string>): string | null =>
          params.data?.role?.roleName ?? null
      },
      { headerName: 'Actions', field: 'actions', width: 150, cellRenderer: ActionButtonComponent }
    ];
  }
}
