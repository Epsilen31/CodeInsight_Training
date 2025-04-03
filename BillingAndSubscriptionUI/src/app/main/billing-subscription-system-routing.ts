import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../core/guard/auth.guard';
import { DashboardComponent } from '../components/dashboard/dashboard.component';

const routes: Routes = [
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'user',
    loadChildren: () => import('../components/user/user.module').then((m) => m.UserModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'subscription',
    loadChildren: () =>
      import('../components/subscription/subscription.module').then((m) => m.SubscriptionModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'billing',
    loadChildren: () => import('../components/billing/billing.module').then((m) => m.BillingModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'payment',
    loadChildren: () => import('../components/payment/payment.module').then((m) => m.PaymentModule),
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BillingSubscriptionSystemRoutingModule {}
