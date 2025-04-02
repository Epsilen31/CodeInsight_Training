import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { IOverduePayments, IOverduePaymentsResponse } from './../../../models/payment';
import { ColDef, GridOptions, ValueFormatterParams } from 'ag-grid-community';
import { PaymentService } from '../../../services/payment.service';
import { ToastService } from '../../../services/toast.service';
import { Subscription } from 'rxjs';
import { ThemeService } from '../../../services/theme.service';
import { TableComponent } from '../../../shared/table/table.component';

@Component({
  selector: 'app-overdue-payment',
  standalone: false,
  templateUrl: './overdue-payment.component.html',
  styleUrls: ['./overdue-payment.component.scss']
})
export class OverduePaymentComponent implements OnInit, OnDestroy {
  overduePayment: IOverduePayments[] = [];
  isDarkMode: boolean = false;

  columnDefs: ColDef[] = this.getColumnDefs();
  defaultColDef: ColDef = { flex: 1, sortable: true, filter: true };

  gridOptions: GridOptions<IOverduePayments> = {
    defaultColDef: this.defaultColDef,
    domLayout: 'autoHeight',
    ensureDomOrder: true
  };

  subscriptionId: number = parseInt(localStorage.getItem('subscriptionId') ?? '0');

  @ViewChild(TableComponent) tableComponent!: TableComponent<IOverduePayments>;

  private themeSubscription: Subscription | undefined;

  constructor(
    private readonly _paymentService: PaymentService,
    private readonly _toast: ToastService,
    private readonly _themeService: ThemeService
  ) {}

  ngOnInit(): void {
    this.themeSubscription = this._themeService.theme$.subscribe((isDark) => {
      this.isDarkMode = isDark;
      document.body.dataset['agThemeMode'] = isDark ? 'dark-red' : 'light-red';
      localStorage.setItem('theme', isDark ? 'dark' : 'light');
      if (this.tableComponent) {
        this.tableComponent.applyTheme(isDark);
      }
    });

    this.fetchOverduePayments();
  }

  ngOnDestroy(): void {
    if (this.themeSubscription) {
      this.themeSubscription.unsubscribe();
    }
  }

  private fetchOverduePayments(): void {
    this._paymentService.fetchOverduePayments(this.subscriptionId).subscribe({
      next: (response: IOverduePaymentsResponse): void => {
        this.overduePayment = response?.overduePayments;
        this._toast.showSuccess('Overdue payment has been fetched successfully');
      },
      error: (error: Error): void => {
        console.error('Error fetching overdue payments:', error);
        this._toast.showError('Error fetching overdue payments');
      }
    });
  }

  private getColumnDefs(): ColDef[] {
    return [
      { headerName: 'ID', field: 'id', width: 100 },
      { headerName: 'Subscription ID', field: 'subscriptionId', width: 100 },
      { headerName: 'Amount', field: 'amount', width: 100 },
      {
        headerName: 'Payment Date',
        field: 'paymentDate',
        width: 150,
        valueFormatter: (params: ValueFormatterParams<IOverduePayments, string>): string => {
          if (params.value) {
            const date = new Date(params.value);
            return date.toLocaleDateString('en-US', {
              year: 'numeric',
              month: 'short',
              day: '2-digit'
            });
          }
          return '';
        }
      },
      { headerName: 'Payment Status', field: 'paymentStatus', width: 150 }
    ];
  }
}
