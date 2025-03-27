import { Component, OnInit, OnDestroy } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { SubscriptionService } from '../../../services/subscription.service';
import { SessionHelperService } from '../../../core/session-helper.service';
import { ICreateSubscriptionResponse, ISubscriptionRequest } from '../../../models/subscription';
import { ToastService } from '../../../services/toast.service';
import { ThemeService } from '../../../services/theme.service';
import { IUserSession } from '../../../models/userSession ';

@Component({
  selector: 'app-user-subscription',
  templateUrl: './user-subscription.component.html',
  styleUrls: ['./user-subscription.component.scss'],
  standalone: false
})
export class UserSubscriptionComponent implements OnInit, OnDestroy {
  userId: number;
  selectedPlanType: number | null = null;
  subscriptionId!: number;
  isDarkMode: boolean = false;
  private themeSubscription!: Subscription;

  constructor(
    private readonly _subscriptionService: SubscriptionService,
    private readonly _sessionHelper: SessionHelperService,
    private readonly _toastService: ToastService,
    private readonly _router: Router,
    private readonly _themeService: ThemeService
  ) {
    const storedUser: IUserSession | null = this._sessionHelper.getItem<IUserSession>('user');
    this.userId = storedUser ? Number(storedUser.id) : -1;
  }

  ngOnInit(): void {
    this.themeSubscription = this._themeService.theme$.subscribe((isDark: boolean) => {
      this.isDarkMode = isDark;
    });
  }

  ngOnDestroy(): void {
    if (this.themeSubscription) {
      this.themeSubscription.unsubscribe();
    }
  }

  createSubscription(): void {
    if (!this.selectedPlanType) {
      this._toastService.showWarning('No plan selected.');
      return;
    }

    const subscriptionData: ISubscriptionRequest = this.getSubscriptionData();

    this._subscriptionService.createSubscription(subscriptionData).subscribe({
      next: (response: ICreateSubscriptionResponse): void => {
        const subscriptionId: number = response?.subscription?.subscriptionId;
        this.subscriptionId = subscriptionId;

        const sendSubcriptionId: NavigationExtras = this.NavigateExtra();

        if (subscriptionId) {
          this._toastService.showSuccess('Subscription successful! Redirecting to payment...');
          localStorage.setItem('subscriptionId', subscriptionId.toString());
          this._router.navigate(
            ['/billing-subscription/payment/create-payment'],
            sendSubcriptionId
          );
        } else {
          this._toastService.showError('Subscription created but ID not received!');
        }
      },
      error: (error: Error): void => {
        this._toastService.showError(`Error: ${error.message}`);
      }
    });
  }

  private getSubscriptionData(): ISubscriptionRequest {
    const today: Date = new Date();
    const nextYear: Date = new Date(today);
    nextYear.setFullYear(today.getFullYear() + 1);

    return {
      planType: this.selectedPlanType!,
      startDate: today.toISOString(),
      endDate: nextYear.toISOString(),
      subscriptionStatus: 1,
      userId: this.userId
    };
  }

  private NavigateExtra(): NavigationExtras {
    return {
      state: {
        subscriptionId: this.subscriptionId
      }
    };
  }
}
