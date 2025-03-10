import { Component, Type } from '@angular/core';
import { ErrorDialogService } from '../../services/error-dialog.service';
import { DashboardService } from '../../services/dashboard.service';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent {
  storedUser: string | null = sessionStorage.getItem('user');
  user: string = this.storedUser ? JSON.parse(this.storedUser).name : undefined;

  activeComponent: Type<any> | null = null;

  constructor(
    private readonly _errorService: ErrorDialogService,
    private readonly _dashboardService: DashboardService
  ) {}

  ngOnInit(): void {
    this._dashboardService.activeComponent$.subscribe((component) => {
      this.activeComponent = component;
    });
  }

  triggerDashboardError(): void {
    console.log('[DashboardComponent] Triggering error...');
    this._errorService.showError('An error occurred in the dashboard!');
  }
}
