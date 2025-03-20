import { Component, OnInit } from '@angular/core';
import { SubscriptionService } from '../../../services/subscription.service';
import { SessionHelperService } from '../../../core/session-helper.service';
import {
  ISubscription,
  ISubscriptionDetail,
} from '../../../models/subscription';

@Component({
  selector: 'app-update-subscription',
  templateUrl: './update-subscription.component.html',
  styleUrl: './update-subscription.component.scss',
  standalone: false,
})
export class UpdateSubscriptionComponent implements OnInit {
  userId: number = -1;
  subscription!: ISubscription;
  subscriptionId!: number;
  errorMessage: string | null = null;

  constructor(
    private readonly subscriptionService: SubscriptionService,
    private readonly _sessionHelper: SessionHelperService,
  ) {
    const storedUser: { id: number } | null = this._sessionHelper.getItem<{
      id: number;
    }>('user');
    this.userId = storedUser ? storedUser.id : -1;
  }

  ngOnInit(): void {
    console.log('working');
    this.getSubscription();
  }

  getSubscription(): void {
    this.errorMessage = null;

    this.subscriptionService.getSubscriptionByUserId(this.userId).subscribe({
      next: (response: ISubscriptionDetail) => {
        console.log('subscriptionResponse', response);
        if (!response?.subscription) {
          this.errorMessage = 'No active subscription found.';
          return;
        }
        this.subscription = Array.isArray(response.subscription)
          ? response.subscription[0]
          : null;
        this.subscriptionId = this.subscription.subscriptionId;
      },
      error: (): void => {
        this.errorMessage = 'Failed to load subscription details.';
      },
    });
  }

  onPlanChange(): void {
    const today = new Date();
    this.subscription.planType = Number(this.subscription.planType);
    this.subscription.startDate = today.toISOString().split('T')[0];
  }

  updateSubscription(): void {
    if (!this.subscriptionId) {
      this.errorMessage = 'No subscription ID found for update.';
      return;
    }

    const newEndDate = new Date(this.subscription.startDate);
    newEndDate.setFullYear(newEndDate.getFullYear() + 1);
    this.subscription.endDate = newEndDate.toISOString().split('T')[0];

    const updatedSubscription: ISubscription = {
      subscriptionId: this.subscriptionId,
      userId: this.userId,
      planType: this.subscription.planType,
      startDate: this.subscription.startDate,
      endDate: this.subscription.endDate,
      subscriptionStatus: this.subscription.subscriptionStatus,
    };

    this.subscriptionService
      .updateSubscription(this.userId, updatedSubscription)
      .subscribe({
        next: (): void => {
          this.errorMessage = null;
          this.getSubscription();
        },
        error: (): void => {
          this.errorMessage =
            'Failed to update subscription. Please try again.';
        },
      });
  }
}
