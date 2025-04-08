import { Component, OnInit } from '@angular/core';
import { ThemeService } from '../services/theme.service';

@Component({
  selector: 'app-billing-subscription',
  standalone: false,
  templateUrl: './billing-subscription.component.html',
  styleUrls: ['./billing-subscription.component.scss']
})
export class BillingSubscriptionComponent implements OnInit {
  isLeftSidebarCollapsed: boolean = false;
  isDarkMode!: boolean;

  constructor(private readonly _themeService: ThemeService) {}

  ngOnInit(): void {
    this._themeService.theme$.subscribe((isDark: boolean): void => {
      this.isDarkMode = isDark;
    });
  }

  toggleLeftSidebar(): void {
    this.isLeftSidebarCollapsed = !this.isLeftSidebarCollapsed;
  }

  toggleDarkMode(): void {
    this._themeService.setTheme(!this.isDarkMode);
  }
}
