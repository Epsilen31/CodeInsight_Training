import { Component, Input } from '@angular/core';
import { ColDef, GridOptions, GridReadyEvent, GridApi, Theme } from 'ag-grid-community';
import { myTheme } from '../theme-config';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss'],
  standalone: false
})
export class TableComponent<T> {
  @Input() rowData: T[] = [];
  @Input() columnDefs: ColDef[] = [];
  @Input() defaultColDef: ColDef = {};
  @Input() gridOptions: GridOptions = {};
  @Input() isDarkMode: boolean = false;

  gridApi!: GridApi;

  constructor() {}

  onGridReady(params: GridReadyEvent<T>): void {
    this.gridApi = params.api;
  }

  theme: Theme = myTheme;

  applyTheme(): void {
    this.theme = this.isDarkMode
      ? myTheme.withParams({ browserColorScheme: 'dark' }, 'dark-red')
      : myTheme.withParams({ browserColorScheme: 'light' }, 'light-red');

    this.gridOptions = { ...this.gridOptions, theme: this.theme };
  }
}
