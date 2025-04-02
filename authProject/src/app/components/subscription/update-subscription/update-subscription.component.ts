import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { SubscriptionService } from '../../../services/subscription.service';
import { SessionHelperService } from '../../../core/session-helper.service';
import { ISubscription, ISubscriptionDetail } from '../../../models/subscription';
import { IErrorResponse } from '../../../models/error';
import { ToastService } from '../../../services/toast.service';
import { ThemeService } from '../../../services/theme.service';

@Component({
  selector: 'app-update-subscription',
  templateUrl: './update-subscription.component.html',
  styleUrls: ['./update-subscription.component.scss'],
  standalone: false
})
export class UpdateSubscriptionComponent implements OnInit, OnDestroy {
  userId: number = -1;
  subscription!: ISubscription;
  subscriptionId!: number;
  errorMessage: string | null = null;
  isDarkMode: boolean = false;
  private themeSubscription!: Subscription;

  constructor(
    private readonly _subscriptionService: SubscriptionService,
    private readonly _sessionHelper: SessionHelperService,
    private readonly _toastService: ToastService,
    private readonly _themeService: ThemeService
  ) {
    // Get the user ID from session storage
    this.userId = this._sessionHelper.getItem<{ id: number }>('user')?.id ?? -1;
  }

  ngOnInit(): void {
    // Subscribe to theme changes
    this.themeSubscription = this._themeService.theme$.subscribe((isDark: boolean) => {
      this.isDarkMode = isDark;
    });

    // Fetch subscription details
    this.getSubscription();
  }

  ngOnDestroy(): void {
    if (this.themeSubscription) {
      this.themeSubscription.unsubscribe();
    }
  }

  private getSubscription(): void {
    this.errorMessage = null;

    this._subscriptionService.getSubscriptionByUserId(this.userId).subscribe({
      next: (response: ISubscriptionDetail) => {
        if (!response?.subscription[0]) {
          this.errorMessage = 'No active subscription found.';
          return;
        }
        this._toastService.showSuccess('subscription updated successfully');
        this.subscription = response.subscription[0];
        this.subscriptionId = this.subscription.subscriptionId;
      },
      error: (error: IErrorResponse): void => {
        this._toastService.showError(`Error fetching users: ${error.message}`);
        this.errorMessage = 'Failed to load subscription details.';
      }
    });
  }

  onSubscriptionPlanChange(): void {
    const today = new Date();
    this.subscription.planType = Number(this.subscription.planType);
    this.subscription.startDate = today.toISOString().split('T')[0];
  }

  updateSubscription(): void {
    if (!this.subscriptionId) {
      this.errorMessage = 'No subscription ID found for update.';
      return;
    }

    const endDate: Date = this.subscriptionEndDate();
    this.subscription.endDate = endDate.toISOString().split('T')[0];

    const updatedSubscriptionData: ISubscription = this.updatedSubscription();

    this._subscriptionService.updateSubscription(this.userId, updatedSubscriptionData).subscribe({
      next: (): void => {
        this.errorMessage = null;
        this.getSubscription();
      },
      error: (error: IErrorResponse): void => {
        this._toastService.showError(`Error updating subscription: ${error.message}`);
        this.errorMessage = 'Failed to update subscription. Please try again.';
      }
    });
  }

  private subscriptionEndDate(): Date {
    const newEndDate = new Date(this.subscription.startDate);
    newEndDate.setFullYear(newEndDate.getFullYear() + 1);
    return newEndDate;
  }

  private updatedSubscription(): ISubscription {
    return {
      subscriptionId: this.subscriptionId,
      userId: this.userId,
      planType: this.subscription.planType,
      startDate: this.subscription.startDate,
      endDate: this.subscription.endDate,
      subscriptionStatus: this.subscription.subscriptionStatus
    };
  }
}
