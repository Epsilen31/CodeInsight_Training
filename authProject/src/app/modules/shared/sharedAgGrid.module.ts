import { NgModule } from '@angular/core';
import { ModuleRegistry, AllCommunityModule } from 'ag-grid-community';
import { TableComponent } from './table/table.component';
import { CommonModule } from '@angular/common';
import { AgGridModule } from 'ag-grid-angular';

ModuleRegistry.registerModules([AllCommunityModule]);

@NgModule({
  declarations: [TableComponent],
  exports: [TableComponent],
  imports: [CommonModule, AgGridModule]
})
export class SharedAgGridModule {}
