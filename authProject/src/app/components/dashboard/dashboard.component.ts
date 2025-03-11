import { Component, Type } from '@angular/core';
import { ErrorDialogService } from '../../services/error-dialog.service';
import { SessionHelperService } from '../../core/session-helper.service';
import { IUserSession } from '../../models/UserSession ';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent {
  storedUser: IUserSession | null = null;
  user: string = '';

  activeComponent: Type<any> | null = null;

  constructor(
    private readonly _errorService: ErrorDialogService,
    private readonly _sessionService: SessionHelperService
  ) {
    this.storedUser = this._sessionService.getItem<IUserSession>('user');
    if (this.storedUser) {
      this.user = this.storedUser.name;
    }
  }

  triggerDashboardError(): void {
    console.log('[DashboardComponent] Triggering error...');
    this._errorService.showError('An error occurred in the dashboard!');
  }
}
