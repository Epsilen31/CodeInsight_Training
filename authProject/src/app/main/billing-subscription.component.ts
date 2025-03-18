import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { ToastrService, ToastContainerDirective } from 'ngx-toastr';
import { UserComponent } from '../modules/user/user/user.component';

@Component({
  selector: 'app-billing-subscription',
  standalone: false,
  templateUrl: './billing-subscription.component.html',
  styleUrls: ['./billing-subscription.component.scss'],
})
export class BillingSubscriptionComponent implements AfterViewInit {
  isLeftSidebarCollapsed: boolean = false;
  isDarkMode: boolean = true;
  private routerOutletComponent: any;

  @ViewChild(ToastContainerDirective, { static: true })
  toastContainer!: ToastContainerDirective;

  constructor(private readonly toastr: ToastrService) {}

  ngAfterViewInit(): void {
    this.toastr.overlayContainer = this.toastContainer;
    const storedTheme = localStorage.getItem('theme');
    if (storedTheme === 'dark') {
      this.enableDarkMode();
    }
  }

  toggleLeftSidebar(): void {
    this.isLeftSidebarCollapsed = !this.isLeftSidebarCollapsed;
  }

  toggleDarkMode(): void {
    this.isDarkMode = !this.isDarkMode;
    if (this.isDarkMode) {
      this.enableDarkMode();
    } else {
      this.disableDarkMode();
    }
    this.onActivate(this.routerOutletComponent);
  }

  private enableDarkMode(): void {
    document.body.classList.add('dark-mode');
    localStorage.setItem('theme', 'dark');
  }

  private disableDarkMode(): void {
    document.body.classList.remove('dark-mode');
    localStorage.setItem('theme', 'light');
  }

  onActivate(component: any): void {
    this.routerOutletComponent = component;
    if (component instanceof UserComponent) {
      component.setDarkMode(this.isDarkMode);
    }
  }
}
