import { Component, ViewChild, OnInit } from '@angular/core';
import { ToastrService, ToastContainerDirective } from 'ngx-toastr';
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

  @ViewChild(ToastContainerDirective, { static: true })
  toastContainer!: ToastContainerDirective;

  constructor(
    private readonly _toastService: ToastrService,
    private readonly _themeService: ThemeService
  ) {}

  ngOnInit(): void {
    this._toastService.overlayContainer = this.toastContainer;
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
