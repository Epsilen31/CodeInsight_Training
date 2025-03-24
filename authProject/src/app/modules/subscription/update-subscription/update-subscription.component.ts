import { Component } from '@angular/core';
import { SubscriptionService } from '../../../services/subscription.service';
import { SessionHelperService } from '../../../core/session-helper.service';
import { ISubscription, ISubscriptionDetail } from '../../../models/subscription';
import { IErrorResponse } from '../../../models/error';
import { ToastService } from '../../../services/toast.service';

@Component({
  selector: 'app-update-subscription',
  templateUrl: './update-subscription.component.html',
  styleUrls: ['./update-subscription.component.scss'],
  standalone: false
})
export class UpdateSubscriptionComponent {
  userId: number = -1;
  subscription!: ISubscription;
  subscriptionId!: number;
  errorMessage: string | null = null;

  constructor(
    private readonly _subscriptionService: SubscriptionService,
    private readonly _sessionHelper: SessionHelperService,
    private readonly _toastService: ToastService
  ) {
    this.userId = this._sessionHelper.getItem<{ id: number }>('user')?.id ?? -1;
    this.getSubscription();
  }

  private getSubscription(): void {
    this.errorMessage = null;

    this._subscriptionService.getSubscriptionByUserId(this.userId).subscribe({
      next: (response: ISubscriptionDetail) => {
        if (!response?.subscription[0]) {
          this.errorMessage = 'No active subscription found.';
          return;
        }
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

    const EndDate: Date = this.subscriptionEndDate();
    this.subscription.endDate = EndDate.toISOString().split('T')[0];

    const updatedSubscriptionData: ISubscription = this.updatedSubscription();

    this._subscriptionService.updateSubscription(this.userId, updatedSubscriptionData).subscribe({
      next: (): void => {
        this.errorMessage = null;
        this.getSubscription();
      },
      error: (error: IErrorResponse): void => {
        this._toastService.showError(`Error fetching users: ${error.message}`);
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
