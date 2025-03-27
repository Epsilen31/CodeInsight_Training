import { NgModule } from '@angular/core';
import { PaymentRoutingModule } from './payment-routing.module';
import { CreatePaymentComponent } from './create-payment/create-payment.component';
import { OverduePaymentComponent } from './overdue-payment/overdue-payment.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  declarations: [CreatePaymentComponent, OverduePaymentComponent],
  imports: [CommonModule, FormsModule, PaymentRoutingModule, SharedModule]
})
export class PaymentModule {}
