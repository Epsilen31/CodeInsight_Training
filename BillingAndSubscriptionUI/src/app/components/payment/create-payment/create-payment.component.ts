import { Component, OnInit, OnDestroy } from '@angular/core';
import { Navigation, Router } from '@angular/router';
import { PaymentService } from '../../../services/payment.service';
import { ToastService } from '../../../services/toast.service';
import { ICreditCard } from '../../../models/paymentCard';
import { IPaymentPayload } from '../../../models/payment';
import { Subscription } from 'rxjs';
import { ThemeService } from '../../../services/theme.service';

@Component({
  selector: 'app-create-payment',
  templateUrl: './create-payment.component.html',
  styleUrls: ['./create-payment.component.scss'],
  standalone: false
})
export class CreatePaymentComponent implements OnInit, OnDestroy {
  subscriptionId: number;
  amount: number = 0;
  paymentStatus: number = 2;

  newCard: ICreditCard = { name: '', number: '', expiry: '', cvv: '' };
  selectedCardType: string = '';
  cardTypes: string[] = ['Visa', 'MasterCard', 'RuPay'];
  showNewCardForm: boolean = true;
  activeTab: string = 'credit';

  qrCodeData: string = '';

  isDarkMode: boolean = false;
  private themeSubscription!: Subscription;

  constructor(
    private readonly _paymentService: PaymentService,
    private readonly _toast: ToastService,
    private readonly _router: Router,
    private readonly _themeService: ThemeService
  ) {
    const navigation: Navigation | null = this._router.getCurrentNavigation();
    const subscriptionIdFromState: string = navigation?.extras.state?.subscriptionId;
    const subscriptionIdFromLocal: string | null = localStorage.getItem('subscriptionId');
    this.subscriptionId = Number(subscriptionIdFromState || subscriptionIdFromLocal) || 0;
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

  selectPaymentType(tab: string): void {
    this.activeTab = tab;
  }

  selectCardType(type: string): void {
    this.selectedCardType = type;
    this.showNewCardForm = true;
  }

  processPayment(): void {
    if (this.amount <= 0) {
      this._toast.showWarning('Please enter a valid amount.');
      return;
    }

    const payload = this.createPaymentPayload();

    this._paymentService.makePayment(payload).subscribe({
      next: (): void => {
        this._toast.showSuccess('Payment successful!');
        this.resetCardInformation();
      },
      error: (error: Error): void => {
        this._toast.showError('Payment failed. Try again.');
      }
    });
  }

  private resetCardInformation(): void {
    this.newCard = { name: '', number: '', expiry: '', cvv: '' };
    this.selectedCardType = '';
    this.amount = 0;
    this.showNewCardForm = false;
  }

  redirectToPayPalSite(): void {
    window.location.href = 'https://www.paypal.com/checkout';
  }

  private createPaymentPayload(): IPaymentPayload {
    return {
      subscriptionId: this.subscriptionId,
      amount: this.amount,
      paymentDate: new Date().toISOString(),
      paymentStatus: this.paymentStatus
    };
  }
}
