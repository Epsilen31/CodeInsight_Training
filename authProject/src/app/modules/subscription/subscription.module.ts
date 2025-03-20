import { NgModule } from '@angular/core';
import { SubscriptionRoutingModule } from './subscription-routing.module';
import { UserSubscriptionComponent } from './user-subscription/user-subscription.component';
import { UpdateSubscriptionComponent } from './update-subscription/update-subscription.component';
import { SubscriptionDetailComponent } from './subscription-detail/subscription-detail.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [
    UserSubscriptionComponent,
    UpdateSubscriptionComponent,
    SubscriptionDetailComponent,
  ],
  imports: [SubscriptionRoutingModule, FormsModule, CommonModule],
})
export class SubscriptionModule {}
