import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BillingRoutingModule } from './billing-routing.module';
import { UpdateBillingComponent } from './update-billing/update-billing.component';
import { UserWithBillingComponent } from './user-with-billing/user-with-billing.component';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  declarations: [UpdateBillingComponent, UserWithBillingComponent],
  imports: [CommonModule, BillingRoutingModule, ReactiveFormsModule, SharedModule]
})
export class BillingModule {}
