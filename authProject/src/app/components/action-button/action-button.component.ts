import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { ICellRendererParams, GridApi } from 'ag-grid-community';
import { UserService } from '../../services/user.service';
import { ToastService } from '../../services/toast.service';

@Component({
  selector: 'app-action-buttons',
  templateUrl: './action-button.component.html',
  styleUrl: './action-button.component.scss',
  standalone: false
})
export class ActionButtonComponent implements ICellRendererAngularComp {
  private params!: ICellRendererParams;
  private gridApi!: GridApi;

  private readonly VIEW_URL: string = '/billing-subscription/user/get-user-by-id';
  private readonly UPDATE_URL: string = '/billing-subscription/user/update-user';

  constructor(
    private readonly _router: Router,
    private readonly _userService: UserService,
    private readonly _toastService: ToastService
  ) {}

  agInit(params: ICellRendererParams): void {
    this.params = params;
    this.gridApi = params.api;
  }

  refresh(params: ICellRendererParams): boolean {
    this.params = params;
    return true;
  }

  onView(): void {
    const userId: number = this.params.data.id;
    this._router.navigate([`${this.VIEW_URL}/${userId}`]);
  }

  onUpdate(): void {
    const userId: number = this.params.data.id;
    this._router.navigate([`${this.UPDATE_URL}/${userId}`]);
  }

  onDelete(): void {
    const userId: number = this.params.data.id;

    if (confirm('Are you sure you want to delete this user?')) {
      this._userService.deleteUser(userId).subscribe({
        next: (): void => {
          this._toastService.showSuccess('User deleted successfully.');
          this.gridApi.applyTransaction({ remove: [this.params.data] });
        },
        error: (error: { message: string }): void => {
          this._toastService.showError('Error deleting user: ' + error.message);
        }
      });
    }
  }
}
