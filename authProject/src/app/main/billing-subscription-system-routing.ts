import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../core/guard/auth.guard';
import { DashboardComponent } from '../components/dashboard/dashboard.component';

const routes: Routes = [
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'user',
    loadChildren: () =>
      import('../modules/user/user-routing.module').then(
        (m) => m.UserRoutingModule
      ),
    canActivate: [AuthGuard],
  },
  {
    path: 'subscription',
    loadChildren: () =>
      import('../modules/subscription/subscription-routing.module').then(
        (m) => m.SubscriptionRoutingModule
      ),
    canActivate: [AuthGuard],
  },
  {
    path: 'billing',
    loadChildren: () =>
      import('../modules/billing/billing-routing.module').then(
        (m) => m.BillingRoutingModule
      ),
    canActivate: [AuthGuard],
  },
  {
    path: 'payment',
    loadChildren: () =>
      import('../modules/payment/payment-routing.module').then(
        (m) => m.PaymentRoutingModule
      ),
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BillingSubscriptionSystemRoutingModule {}
