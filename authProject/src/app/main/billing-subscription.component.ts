import { Component, OnInit } from '@angular/core';
import { ThemeService } from '../services/theme.service';
import { NotificationService } from '../services/notification.service';

@Component({
  selector: 'app-billing-subscription',
  standalone: false,
  templateUrl: './billing-subscription.Component.html',
  styleUrls: ['./billing-subscription.component.scss'],
})
export class BillingSubscriptionComponent implements OnInit {
  isLeftSidebarCollapsed: boolean = false;

  notifications: string[] = [];

  constructor(
    private readonly _notificationService: NotificationService,
    private readonly _themeService: ThemeService
  ) {}

  ngOnInit(): void {
    this._notificationService.startConnection();
    this._notificationService.listenForMessages((message: string): void => {
      this.notifications.push(message);
    });
  }

  toggleLeftSidebar(): void {
    this.isLeftSidebarCollapsed = !this.isLeftSidebarCollapsed;
  }

  changeTheme(theme: string): void {
    this._themeService.setTheme(theme);
  }
}
