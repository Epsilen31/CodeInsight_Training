import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../../core/guard/auth.guard';
import { CreatePaymentComponent } from './create-payment/create-payment.component';
import { OverduePaymentComponent } from './overdue-payment/overdue-payment.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'create-payment',
    pathMatch: 'full'
  },

  {
    path: 'create-payment',
    component: CreatePaymentComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'get-overdue-payments/:id',
    component: OverduePaymentComponent,
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PaymentRoutingModule {}
