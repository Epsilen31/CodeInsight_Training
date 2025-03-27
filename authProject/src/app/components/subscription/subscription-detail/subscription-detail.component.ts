import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { SubscriptionService } from '../../../services/subscription.service';
import { SessionHelperService } from '../../../core/session-helper.service';
import { ISubscription, ISubscriptionDetail } from '../../../models/subscription';
import { Subscription } from 'rxjs';
import { ThemeService } from '../../../services/theme.service';

@Component({
  selector: 'app-subscription-detail',
  standalone: false,
  templateUrl: './subscription-detail.component.html',
  styleUrls: ['./subscription-detail.component.scss']
})
export class SubscriptionDetailComponent implements OnInit, OnDestroy {
  userId!: number;
  subscription: ISubscription | null = null;
  isLoading: boolean = true;
  errorMessage: string | null = null;
  isDarkMode: boolean = false;
  private themeSubscription!: Subscription;

  constructor(
    private readonly _subscriptionService: SubscriptionService,
    private readonly _sessionHelper: SessionHelperService,
    private readonly _router: Router,
    private readonly _themeService: ThemeService
  ) {
    const user = this._sessionHelper.getItem<{ id: number }>('user');
    this.userId = user?.id ?? -1;
  }

  ngOnInit(): void {
    this.themeSubscription = this._themeService.theme$.subscribe((isDark: boolean) => {
      this.isDarkMode = isDark;
    });
    this.getSubscriptionDetails();
  }

  ngOnDestroy(): void {
    if (this.themeSubscription) {
      this.themeSubscription.unsubscribe();
    }
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
    this._router.navigate(['/billing-subscription/user/get-users']);
  }
}
