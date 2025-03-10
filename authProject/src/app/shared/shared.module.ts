import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from './sidebar/sidebar.component';
import { ErrorDialogComponent } from './error-dialog/error-dialog.component';
import { Dialog } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { DrawerModule } from 'primeng/drawer';
import { ToolbarModule } from 'primeng/toolbar';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [SidebarComponent, ErrorDialogComponent],
  exports: [SidebarComponent, ErrorDialogComponent, CommonModule],
  imports: [
    CommonModule,
    Dialog,
    ButtonModule,
    DrawerModule,
    ToolbarModule,
    RouterModule,
  ],
})
export class SharedModule {}
