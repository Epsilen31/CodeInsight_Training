import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guard/auth.guard';
import { BillingSubscriptionComponent } from './main/billing-subscription.component';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { BillingSubscriptionSystemRoutingModule } from './main/billing-subscription-system-routing';

const routes: Routes = [
  { path: '', redirectTo: 'billing-subscription/dashboard', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  {
    path: 'billing-subscription',
    component: BillingSubscriptionComponent,
    canActivate: [AuthGuard],
    loadChildren: () =>
      import('./main/billing-subscription-system-routing').then(
        (m): typeof BillingSubscriptionSystemRoutingModule =>
          m.BillingSubscriptionSystemRoutingModule
      )
  },

  { path: '**', redirectTo: 'billing-subscription/dashboard' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
