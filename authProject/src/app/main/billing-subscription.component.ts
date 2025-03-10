import { Component } from '@angular/core';
import { ThemeService } from '../services/theme.service';

@Component({
  selector: 'app-billing-subscription',
  standalone: false,
  templateUrl: './billing-subscription.Component.html',
  styleUrls: ['./billing-subscription.component.scss'],
})
export class BillingSubscriptionComponent {
  isLeftSidebarCollapsed: boolean = false;

  toggleLeftSidebar(): void {
    this.isLeftSidebarCollapsed = !this.isLeftSidebarCollapsed;
  }

  constructor(private readonly _themeService: ThemeService) {}

  changeTheme(theme: string): void {
    this._themeService.setTheme(theme);
  }
}
