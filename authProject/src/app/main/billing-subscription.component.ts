import { Component, ViewChild, OnInit } from '@angular/core';
import { ToastrService, ToastContainerDirective } from 'ngx-toastr';
import { UserComponent } from '../modules/user/user/user.component';

@Component({
  selector: 'app-billing-subscription',
  standalone: false,
  templateUrl: './billing-subscription.component.html',
  styleUrls: ['./billing-subscription.component.scss']
})
export class BillingSubscriptionComponent implements OnInit {
  isLeftSidebarCollapsed: boolean = false;
  isDarkMode!: boolean;
  private routerOutletComponent?: UserComponent | null;
  storedTheme!: string | null;

  @ViewChild(ToastContainerDirective, { static: true })
  toastContainer!: ToastContainerDirective;

  constructor(private readonly toastr: ToastrService) {
    this.storedTheme = localStorage.getItem('theme');

    if (this.storedTheme === 'dark') {
      this.isDarkMode = true;
      this.enableDarkMode();
    } else {
      this.isDarkMode = false;
    }
    console.log('toggle dark mode', this.isDarkMode);
  }

  ngOnInit(): void {
    this.toastr.overlayContainer = this.toastContainer;
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
    if (this.routerOutletComponent) {
      this.onActivate(this.routerOutletComponent);
    }
  }

  private enableDarkMode(): void {
    document.body.classList.add('dark-mode');
    localStorage.setItem('theme', 'dark');
  }

  private disableDarkMode(): void {
    document.body.classList.remove('dark-mode');
    localStorage.setItem('theme', 'light');
  }

  onActivate(component: UserComponent): void {
    this.routerOutletComponent = component;
    if (component instanceof UserComponent) {
      component.setDarkMode(this.isDarkMode);
    }
  }
}
