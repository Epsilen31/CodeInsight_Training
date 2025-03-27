import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserRoutingModule } from './user-routing.module';
import { UserDetailComponent } from './user-detail/user-detail.component';
import { AddUserComponent } from './add-user/add-user.component';
import { UpdateUserComponent } from './update-user/update-user.component';
import { AllUsersComponent } from './allusers/allusers.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProgressBarComponent } from '../../components/progress-bar/progress-bar.component';
import { ActionButtonComponent } from '../../components/action-button/action-button.component';
import { ProgressSpinner } from 'primeng/progressspinner';
import { DeleteUserComponent } from './delete-user/delete-user.component';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  declarations: [
    AllUsersComponent,
    UserDetailComponent,
    AddUserComponent,
    UpdateUserComponent,
    ProgressBarComponent,
    ActionButtonComponent,
    DeleteUserComponent
  ],
  imports: [
    CommonModule,
    UserRoutingModule,
    FormsModule,
    SharedModule,
    ProgressSpinner,
    ReactiveFormsModule
  ]
})
export class UserModule {}
