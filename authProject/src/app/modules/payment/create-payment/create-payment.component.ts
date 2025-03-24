import { Component } from '@angular/core';
import { Navigation, Router } from '@angular/router';
import { PaymentService } from '../../../services/payment.service';
import { ToastService } from '../../../services/toast.service';
import { ICreditCard } from './../../../models/PaymentCard';
import { IPaymentPayload } from '../../../models/payment';

@Component({
  selector: 'app-create-payment',
  standalone: false,
  templateUrl: './create-payment.component.html',
  styleUrl: './create-payment.component.scss'
})
export class CreatePaymentComponent {
  subscriptionId!: number;
  amount!: number;
  paymentStatus: number = 2;

  newCard: ICreditCard = { name: '', number: '', expiry: '', cvv: '' };
  selectedCardType: string = '';
  cardTypes: string[] = ['Visa', 'MasterCard', 'RuPay'];
  showNewCardForm: boolean = true;
  activeTab: string = 'credit';

  qrCodeData: string = '';

  constructor(
    private readonly _paymentService: PaymentService,
    private readonly _toast: ToastService,
    private readonly _route: Router
  ) {
    const navigation: Navigation | null = this._route.getCurrentNavigation();
    this.subscriptionId = navigation?.extras.state?.subscriptionId;
  }

  selectTab(tab: string): void {
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

    const payload: IPaymentPayload = this.paymentPayload();

    this._paymentService.makePayment(payload).subscribe({
      next: (response: IPaymentPayload): void => {
        this._toast.showSuccess('Payment successful!');
        this.resetForm();
      },
      error: (error: Error): void => {
        console.error('Payment failed:', error);
        this._toast.showError('Payment failed. Try again.');
      }
    });
  }

  resetForm(): void {
    this.newCard = { name: '', number: '', expiry: '', cvv: '' };
    this.selectedCardType = '';
    this.amount = 0;
    this.showNewCardForm = false;
  }

  redirectToPayPalSite(): void {
    window.location.href = 'https://www.paypal.com/checkout';
  }

  private paymentPayload(): IPaymentPayload {
    return {
      subscriptionId: this.subscriptionId,
      amount: this.amount,
      paymentDate: new Date().toISOString(),
      paymentStatus: this.paymentStatus
    };
  }
}
