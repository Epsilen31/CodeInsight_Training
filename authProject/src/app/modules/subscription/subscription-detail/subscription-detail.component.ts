import { Component, OnInit } from '@angular/core';
import { SubscriptionService } from '../../../services/subscription.service';
import { SessionHelperService } from '../../../core/session-helper.service';
import { Router } from '@angular/router';
import {
  ISubscription,
  ISubscriptionDetail,
} from '../../../models/subscription';

@Component({
  selector: 'app-subscription-detail',
  standalone: false,
  templateUrl: './subscription-detail.component.html',
  styleUrls: ['./subscription-detail.component.scss'],
})
export class SubscriptionDetailComponent implements OnInit {
  id!: number;
  subscription: ISubscription | null = null;
  isLoading: boolean = true;
  errorMessage: string | null = null;

  constructor(
    private readonly subscriptionService: SubscriptionService,
    private readonly _sessionHelper: SessionHelperService,
    private readonly router: Router
  ) {
    const storedUser = this._sessionHelper.getItem<{ id: number }>('user');
    this.id = storedUser ? storedUser.id : -1;
  }

  ngOnInit(): void {
    this.getSubscriptionDetails();
  }

  getSubscriptionDetails(): void {
    this.isLoading = true;
    this.subscriptionService.getSubscriptionByUserId(this.id).subscribe({
      next: (response: ISubscriptionDetail): void => {
        console.log('response', response);
        this.subscription = Array.isArray(response.subscription)
          ? response.subscription[0]
          : null;
        console.log('subscription', this.subscription);
      },
      error: (): void => {
        this.errorMessage = 'Failed to fetch subscription details.';
        this.subscription = null;
      },
      complete: (): void => {
        this.isLoading = false;
      },
    });
  }

  goBack(): void {
    this.router.navigate(['/billing-subscription/user/get-users']);
  }
}
