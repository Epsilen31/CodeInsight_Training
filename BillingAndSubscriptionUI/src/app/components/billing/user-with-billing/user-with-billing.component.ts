import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { ColDef, GridOptions } from 'ag-grid-community';
import { Subscription } from 'rxjs';
import { BillingService } from '../../../services/billing.service';
import { IBillingInfo, IUserWithBilling } from '../../../models/billing';
import { TableComponent } from '../../../shared/table/table.component';
import { ThemeService } from '../../../services/theme.service';
import { SessionHelperService } from '../../../core/session-helper.service';

@Component({
  selector: 'app-user-with-billing',
  templateUrl: './user-with-billing.component.html',
  styleUrls: ['./user-with-billing.component.scss'],
  standalone: false
})
export class UserWithBillingComponent implements OnInit, OnDestroy {
  userId!: number | null;

  rowData: IUserWithBilling[] = [];
  columnDefs: ColDef[] = [];
  defaultColDef: ColDef = {
    flex: 1,
    sortable: true,
    resizable: true,
    filter: true,
    minWidth: 100
  };

  gridOptions: GridOptions = {
    defaultColDef: this.defaultColDef,
    rowHeight: 60,
    domLayout: 'autoHeight',
    ensureDomOrder: true
  };

  isDarkMode = false;
  private themeSubscription?: Subscription;

  @ViewChild(TableComponent) tableComponent!: TableComponent<IUserWithBilling>;

  constructor(
    private readonly _billingService: BillingService,
    private readonly _themeService: ThemeService,
    private readonly _sessionService: SessionHelperService
  ) {
    const storedUser: { id: number } | null = this._sessionService.getItem<{ id: number }>('user');
    this.userId = storedUser ? storedUser.id : -1;
  }

  ngOnInit(): void {
    this.setupColumns();
    this.fetchUserWithBilling();

    this.themeSubscription = this._themeService.theme$.subscribe((isDark): void => {
      this.isDarkMode = isDark;
      document.body.dataset['agThemeMode'] = isDark ? 'dark-red' : 'light-red';
      localStorage.setItem('theme', isDark ? 'dark' : 'light');
      if (this.tableComponent) {
        this.tableComponent.applyTheme(isDark);
      }
    });
  }

  ngOnDestroy(): void {
    this.themeSubscription?.unsubscribe();
  }

  setupColumns(): void {
    this.columnDefs = [
      { headerName: 'User ID', field: 'userId' },
      { headerName: 'Billing Address', field: 'billingAddress' },
      { headerName: 'Payment Method', field: 'paymentMethod' }
    ];
  }

  fetchUserWithBilling(): void {
    this._billingService.getUsersWithBilling(this.userId ?? -1).subscribe({
      next: (response: IBillingInfo): void => {
        this.rowData = response.usersWithBilling;
      },
      error: (err: Error) => console.error('Error fetching billing data:', err)
    });
  }
}
