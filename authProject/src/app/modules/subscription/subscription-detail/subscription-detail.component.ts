import { Component } from '@angular/core';
import { SubscriptionService } from '../../../services/subscription.service';
import { SessionHelperService } from '../../../core/session-helper.service';
import { Router } from '@angular/router';
import { ISubscription, ISubscriptionDetail } from '../../../models/subscription';

@Component({
  selector: 'app-subscription-detail',
  standalone: false,
  templateUrl: './subscription-detail.component.html',
  styleUrls: ['./subscription-detail.component.scss']
})
export class SubscriptionDetailComponent {
  userId!: number;
  subscription: ISubscription | null = null;
  isLoading: boolean = true;
  errorMessage: string | null = null;

  constructor(
    private readonly _subscriptionService: SubscriptionService,
    private readonly _sessionHelper: SessionHelperService,
    private readonly router: Router
  ) {
    const user = this._sessionHelper.getItem<{ id: number }>('user');
    this.userId = user?.id ?? -1;
    this.getSubscriptionDetails();
  }

  private getSubscriptionDetails(): void {
    this.isLoading = true;

    this._subscriptionService.getSubscriptionByUserId(this.userId).subscribe({
      next: (response: ISubscriptionDetail): void => {
        this.subscription = response.subscription[0];
      },
      error: (): void => {
        this.errorMessage = 'Failed to fetch subscription details.';
        this.subscription = null;
      },
      complete: (): void => {
        this.isLoading = false;
      }
    });
  }

  redirectToUsers(): void {
    this.router.navigate(['/billing-subscription/user/get-users']);
  }
}
