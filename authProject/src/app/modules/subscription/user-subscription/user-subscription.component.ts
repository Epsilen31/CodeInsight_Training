import { Component, OnInit } from '@angular/core';
import { SubscriptionService } from '../../../services/subscription.service';
import { SessionHelperService } from '../../../core/session-helper.service';
import { ISubscriptionRequest } from '../../../models/subscription';
import { IUserSession } from '../../../models/UserSession ';

@Component({
  selector: 'app-user-subscription',
  standalone: false,
  templateUrl: './user-subscription.component.html',
  styleUrl: './user-subscription.component.scss',
})
export class UserSubscriptionComponent implements OnInit {
  userId: number;
  selectedPlanType: number | null = null;
  subscriptionSuccess: boolean = false;
  subscriptionError: boolean = false;

  constructor(
    private readonly subscriptionService: SubscriptionService,
    private readonly _sessionHelper: SessionHelperService
  ) {
    const storedUser: IUserSession | null =
      this._sessionHelper.getItem<IUserSession>('user');
    this.userId = storedUser ? Number(storedUser.id) : -1;
  }

  ngOnInit(): void {
    console.log(
      'UserSubscriptionComponent initialized with userId:',
      this.userId
    );
  }

  createSubscription(): void {
    if (!this.selectedPlanType) {
      console.error('No plan selected.');
      return;
    }

    console.log('User ID before sending request:', this.userId);

    const today: Date = new Date();
    const nextYear: Date = new Date(today);
    nextYear.setFullYear(today.getFullYear() + 1);

    const subscriptionData: ISubscriptionRequest = {
      planType: this.selectedPlanType,
      startDate: today.toISOString(),
      endDate: nextYear.toISOString(),
      subscriptionStatus: 1,
      userId: this.userId,
    };

    console.log('Creating subscription with:', subscriptionData);

    this.subscriptionService.createSubscription(subscriptionData).subscribe({
      next: (response: ISubscriptionRequest): void => {
        console.log('Subscription created successfully:', response);
        this.subscriptionSuccess = true;
        this.subscriptionError = false;
      },
      error: (error: Error): void => {
        console.error('Error creating subscription:', error);
        this.subscriptionError = true;
        this.subscriptionSuccess = false;
      },
    });
  }
}
