import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../../core/guard/auth.guard';
import { UpdateBillingComponent } from './update-billing/update-billing.component';
import { UserWithBillingComponent } from './user-with-billing/user-with-billing.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'update-billing',
    pathMatch: 'full'
  },

  {
    path: 'update-billing',
    component: UpdateBillingComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'get-users-with-billing/:id',
    component: UserWithBillingComponent,
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BillingRoutingModule {}
