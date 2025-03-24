import { NgModule } from '@angular/core';
import { PaymentRoutingModule } from './payment-routing.module';
import { CreatePaymentComponent } from './create-payment/create-payment.component';
import { OverduePaymentComponent } from './overdue-payment/overdue-payment.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { SharedAgGridModule } from '../shared/sharedAgGrid.module';

@NgModule({
  declarations: [CreatePaymentComponent, OverduePaymentComponent],
  imports: [CommonModule, FormsModule, PaymentRoutingModule, SharedAgGridModule]
})
export class PaymentModule {}
