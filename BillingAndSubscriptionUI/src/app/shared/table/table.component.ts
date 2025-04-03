import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { ColDef, GridOptions, GridReadyEvent, GridApi, Theme } from 'ag-grid-community';
import { Subscription } from 'rxjs';
import { gridTheme } from '../theme-config';
import { ThemeService } from '../../services/theme.service';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss'],
  standalone: false
})
export class TableComponent<T> implements OnInit, OnDestroy {
  @Input() rowData: T[] = [];
  @Input() columnDefs: ColDef[] = [];
  @Input() defaultColDef: ColDef = {};
  @Input() gridOptions: GridOptions = {};

  gridApi!: GridApi;
  theme: Theme = gridTheme;

  private themeSubscription: Subscription | undefined;

  constructor(private readonly _themeService: ThemeService) {}

  ngOnInit(): void {
    this.themeSubscription = this._themeService.theme$.subscribe((isDark) => {
      this.applyTheme(isDark);
    });
  }

  ngOnDestroy(): void {
    if (this.themeSubscription) {
      this.themeSubscription.unsubscribe();
    }
  }

  onGridReady(params: GridReadyEvent<T>): void {
    this.gridApi = params.api;
  }

  applyTheme(isDarkMode: boolean): void {
    this.theme = isDarkMode
      ? gridTheme.withParams({ browserColorScheme: 'dark' }, 'dark-red')
      : gridTheme.withParams({ browserColorScheme: 'light' }, 'light-red');

    this.gridOptions = { ...this.gridOptions, theme: this.theme };

    if (this.gridApi) {
      this.gridApi.redrawRows();
    }
  }
}
