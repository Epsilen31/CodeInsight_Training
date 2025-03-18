import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserRoutingModule } from './user-routing.module';
import { AgGridModule } from 'ag-grid-angular';
import { UserDetailComponent } from './user-detail/user-detail.component';
import { AddUserComponent } from './add-user/add-user.component';
import { UpdateUserComponent } from './update-user/update-user.component';
import { UserComponent } from './user/user.component';
import { FormsModule } from '@angular/forms';
import { AllCommunityModule, ModuleRegistry } from 'ag-grid-community';
import { ProgressBarComponent } from '../../components/progress-bar/progress-bar.component';
import { ActionButtonComponent } from '../../components/action-button/action-button.component';
import { ProgressSpinner } from 'primeng/progressspinner';

ModuleRegistry.registerModules([AllCommunityModule]);

@NgModule({
  declarations: [
    UserComponent,
    UserDetailComponent,
    AddUserComponent,
    UpdateUserComponent,
    ProgressBarComponent,
    ActionButtonComponent,
  ],
  imports: [
    CommonModule,
    UserRoutingModule,
    AgGridModule,
    FormsModule,
    ProgressSpinner,
  ],
})
export class UserModule {}
