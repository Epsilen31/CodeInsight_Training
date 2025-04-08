import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SubscriptionService } from '../../../services/subscription.service';
import { SessionHelperService } from '../../../core/session-helper.service';
import { ISubscription, ISubscriptionDetail } from '../../../models/subscription';
import { Subscription } from 'rxjs';
import { ThemeService } from '../../../services/theme.service';
import { ToastService } from '../../../services/toast.service';
import { RedirectKey } from '../../../shared/constants/redirectionKey';

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
  private readonly subscriptionId: number = 0;

  constructor(
    private readonly _subscriptionService: SubscriptionService,
    private readonly _sessionHelper: SessionHelperService,
    private readonly _router: Router,
    private readonly _route: ActivatedRoute,
    private readonly _themeService: ThemeService,
    private readonly _toastService: ToastService
  ) {
    const user = this._sessionHelper.getItem<{ id: number }>('user');
    this.userId = user?.id ?? -1;

    const subscriptionId = localStorage.getItem('subscriptionId');
    this.subscriptionId = subscriptionId ? parseInt(subscriptionId) : 0;
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

  deleteUserSubcription(): void {
    if (this._route.snapshot.routeConfig?.path === 'get-subscription-by-user-id/:id') {
      const id = this.subscriptionId;
      this._subscriptionService.deleteUserSubscription(id).subscribe({
        next: () => {
          this._toastService.showSuccess('Subscription deleted successfully.');
          localStorage.removeItem('subscriptionId');
          this._router.navigate([`${RedirectKey.REDIRECT_TO_ADD_SUBSCRIPTION}`]);
        },
        error: (error: Error) => {
          this._toastService.showError(`Error deleting subscription: ${error.message}`);
        }
      });
    }
  }

  redirectToUsers(): void {
    this._router.navigate([`${RedirectKey.REDIRECT_TO_ADD_SUBSCRIPTION}`]);
  }
}
