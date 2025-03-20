import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../../core/guard/auth.guard';
import { UserSubscriptionComponent } from './user-subscription/user-subscription.component';
import { SubscriptionDetailComponent } from './subscription-detail/subscription-detail.component';
import { UpdateSubscriptionComponent } from './update-subscription/update-subscription.component';

const routes: Routes = [
  {
    path: 'get-subscription-by-user-id/:id',
    component: SubscriptionDetailComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'update-user-subscription/:id',
    component: UpdateSubscriptionComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'create-user-subscription-plan',
    component: UserSubscriptionComponent,
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SubscriptionRoutingModule {}
