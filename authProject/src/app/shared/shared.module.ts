import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from './sidebar/sidebar.component';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { DrawerModule } from 'primeng/drawer';
import { ToolbarModule } from 'primeng/toolbar';
import { RouterModule } from '@angular/router';
import { ErrorDialogComponent } from './error-dialog/error-dialog.component';

import { AgGridModule } from 'ag-grid-angular';
import { TableComponent } from './table/table.component';
import { AllCommunityModule, ModuleRegistry } from 'ag-grid-community';
import { CustomNgForDirective } from './custom-directive/custom-ng-for.directive';

ModuleRegistry.registerModules([AllCommunityModule]);

@NgModule({
  declarations: [SidebarComponent, ErrorDialogComponent, TableComponent, CustomNgForDirective],
  exports: [SidebarComponent, ErrorDialogComponent, TableComponent],
  imports: [
    CommonModule,
    DialogModule,
    ButtonModule,
    DrawerModule,
    ToolbarModule,
    RouterModule,
    AgGridModule
  ]
})
export class SharedModule {}
