import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../core/guard/auth.guard';
import { DashboardComponent } from '../components/dashboard/dashboard.component';
import { UserRoutingModule } from '../modules/user/user-routing.module';
import { SubscriptionRoutingModule } from '../modules/subscription/subscription-routing.module';
import { BillingRoutingModule } from '../modules/billing/billing-routing.module';
import { PaymentRoutingModule } from '../modules/payment/payment-routing.module';

const routes: Routes = [
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'user',
    loadChildren: () =>
      import('../modules/user/user-routing.module').then(
        (m): typeof UserRoutingModule => m.UserRoutingModule
      ),
    canActivate: [AuthGuard]
  },
  {
    path: 'subscription',
    loadChildren: () =>
      import('../modules/subscription/subscription-routing.module').then(
        (m): typeof SubscriptionRoutingModule => m.SubscriptionRoutingModule
      ),
    canActivate: [AuthGuard]
  },
  {
    path: 'billing',
    loadChildren: () =>
      import('../modules/billing/billing-routing.module').then(
        (m): typeof BillingRoutingModule => m.BillingRoutingModule
      ),
    canActivate: [AuthGuard]
  },
  {
    path: 'payment',
    loadChildren: () =>
      import('../modules/payment/payment-routing.module').then(
        (m): typeof PaymentRoutingModule => m.PaymentRoutingModule
      ),
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BillingSubscriptionSystemRoutingModule {}
