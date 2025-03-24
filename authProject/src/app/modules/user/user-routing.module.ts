import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from '../../core/guard/auth.guard';
import { AddUserComponent } from './add-user/add-user.component';
import { UpdateUserComponent } from './update-user/update-user.component';
import { UserDetailComponent } from './user-detail/user-detail.component';
import { UserComponent } from './user/user.component';

const routes: Routes = [
  { path: 'get-users', component: UserComponent, canActivate: [AuthGuard] },

  {
    path: 'get-user-by-id/:id',
    component: UserDetailComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'update-user/:id',
    component: UpdateUserComponent,
    canActivate: [AuthGuard]
  },
  { path: 'add-user', component: AddUserComponent, canActivate: [AuthGuard] }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule {}
