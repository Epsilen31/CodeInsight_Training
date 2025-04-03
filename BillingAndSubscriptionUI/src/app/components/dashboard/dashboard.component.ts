import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { ThemeService } from '../../services/theme.service';
import { DashboardService } from '../../services/dashboard.service';
import { IDashboardResponseData } from './../../models/dashboard';
import { AgChartOptions, AgPolarChartOptions } from 'ag-charts-community';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  standalone: false
})
export class DashboardComponent implements OnInit, OnDestroy {
  isDarkMode: boolean = false;
  private themeSubscription!: Subscription;

  monthlySubscriptionChartOptions!: AgChartOptions;
  paymentChartOptions!: AgChartOptions;
  userChartOptions!: AgChartOptions;
  subscriptionPlanChartOptions!: AgPolarChartOptions;

  responseData!: IDashboardResponseData;

  loading: boolean = false;

  constructor(
    private readonly _themeService: ThemeService,
    private readonly _dashboardService: DashboardService
  ) {}

  ngOnInit(): void {
    this.themeSubscription = this._themeService.theme$.subscribe((isDark) => {
      this.isDarkMode = isDark;
      this.getDashboardData();
    });
  }

  private get textColor(): string {
    return this.isDarkMode ? '#ffffff' : '#000000';
  }

  private get backgroundColor(): string {
    return this.isDarkMode ? '#1f2937' : '#ffffff';
  }

  private getDashboardData(): void {
    this._dashboardService
      .getDashboardData()
      .subscribe((response: IDashboardResponseData): void => {
        if (response) {
          this.responseData = response;
          this.loadChartConfigs();
        }
      });
  }

  loadChartConfigs(): void {
    this.monthlySubscriptionChartOptions = this.monthlySubscriptionChart();
    this.paymentChartOptions = this.paymentMetricChart();
    this.userChartOptions = this.userMetricChart();
    this.subscriptionPlanChartOptions = this.subscriptionPlanChart();

    this.loading = true;
  }

  private monthlySubscriptionChart(): AgChartOptions {
    return {
      data: this.responseData.monthlySubscriptions,
      title: {
        text: 'Monthly Subscriptions',
        color: this.textColor
      },
      background: { fill: this.backgroundColor },
      series: [
        {
          type: 'bar',
          xKey: 'month',
          yKey: 'count',
          fill: '#4ade80',
          legendItemName: 'month'
        }
      ],
      axes: [
        { type: 'category', position: 'bottom', label: { color: this.textColor } },
        { type: 'number', position: 'left', label: { color: this.textColor } }
      ]
    };
  }

  private paymentMetricChart(): AgChartOptions {
    const paymentMetricsData = [
      { metric: 'Overdue Payments', count: this.responseData.overduePayments },
      { metric: 'Total Payments', count: this.responseData.totalPayments }
    ];

    return {
      data: paymentMetricsData,
      title: {
        text: 'Payment Metrics',
        color: this.textColor
      },
      background: { fill: this.backgroundColor },
      series: [
        {
          type: 'bar',
          xKey: 'metric',
          yKey: 'count',
          fill: '#f87171'
        }
      ],
      axes: [
        { type: 'category', position: 'bottom', label: { color: this.textColor } },
        { type: 'number', position: 'left', label: { color: this.textColor } }
      ]
    };
  }

  private userMetricChart(): AgChartOptions {
    const userMetricsData = [
      { metric: 'Inactive Subscriptions', count: this.responseData.inactiveSubscriptions },
      { metric: 'Inactive Users', count: this.responseData.inactiveUsers },
      { metric: 'Total Users', count: this.responseData.totalUsers }
    ];

    return {
      data: userMetricsData,
      title: {
        text: 'User Metrics',
        color: this.textColor
      },
      background: { fill: this.backgroundColor },
      series: [
        {
          type: 'bar',
          xKey: 'metric',
          yKey: 'count',
          fill: '#a78bfa'
        }
      ],
      axes: [
        { type: 'category', position: 'bottom', label: { color: this.textColor } },
        { type: 'number', position: 'left', label: { color: this.textColor } }
      ]
    };
  }

  private subscriptionPlanChart(): AgPolarChartOptions {
    const subscriptionPlanData = this.responseData.subscriptionPlanStats.map((item) => ({
      plan: item.planType,
      count: item.count
    }));

    return {
      data: subscriptionPlanData,
      title: {
        text: 'Subscription Plans',
        color: this.textColor
      },
      background: { fill: this.backgroundColor },
      series: [
        {
          type: 'pie',
          angleKey: 'count',
          legendItemKey: 'plan',
          calloutLabelKey: 'plan',
          calloutLabel: {
            color: this.textColor
          },
          strokes: ['#000000']
        }
      ],
      legend: {
        item: {
          label: {
            color: this.textColor
          }
        }
      }
    };
  }

  ngOnDestroy(): void {
    if (this.themeSubscription) {
      this.themeSubscription.unsubscribe();
    }
  }
}
