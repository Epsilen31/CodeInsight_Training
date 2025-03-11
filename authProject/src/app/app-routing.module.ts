import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './components/auth/register/register.component';
import { LoginComponent } from './components/auth/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AuthGuard } from './core/guard/auth.guard';
import { BillingSubscriptionComponent } from './main/billing-subscription.component';
import { BillingSubscriptionSystemRoutingModule } from './main/billing-subscription-system-routing';

const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: 'dashboard',
    component: BillingSubscriptionComponent,
    canActivate: [AuthGuard],
    children: [
      { path: '', component: DashboardComponent },
      {
        path: 'billing-subscription',
        loadChildren: () =>
          import('./main/billing-subscription-system-routing').then(
            (m): typeof BillingSubscriptionSystemRoutingModule =>
              m.BillingSubscriptionSystemRoutingModule
          ),
      },
    ],
  },
  { path: '**', redirectTo: 'dashboard' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
