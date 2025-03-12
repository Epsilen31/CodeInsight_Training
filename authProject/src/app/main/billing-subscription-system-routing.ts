import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../core/guard/auth.guard';
import { DashboardComponent } from '../components/dashboard/dashboard.component';
import { BillingComponent } from '../components/billing/billing.component';
import { SubscriptionComponent } from '../components/subscription/subscription.component';
import { PaymentsComponent } from '../components/payments/payments.component';
import { UserComponent } from '../components/user/user.component';

const routes: Routes = [
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [AuthGuard],
  },
  { path: 'billing', component: BillingComponent, canActivate: [AuthGuard] },
  {
    path: 'subscription',
    component: SubscriptionComponent,
    canActivate: [AuthGuard],
  },
  { path: 'payment', component: PaymentsComponent, canActivate: [AuthGuard] },
  { path: 'user', component: UserComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BillingSubscriptionSystemRoutingModule {}
