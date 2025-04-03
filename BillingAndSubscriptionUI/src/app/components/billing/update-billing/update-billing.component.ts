import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ThemeService } from '../../../services/theme.service';
import { BillingService } from '../../../services/billing.service';
import { ToastService } from '../../../services/toast.service';
import { SessionHelperService } from '../../../core/session-helper.service';
import { IUserWithBilling } from './../../../models/billing';

@Component({
  selector: 'app-update-billing',
  templateUrl: './update-billing.component.html',
  styleUrls: ['./update-billing.component.scss'],
  standalone: false
})
export class UpdateBillingComponent implements OnInit {
  billingForm!: FormGroup;
  isDarkMode: boolean = false;
  userId: number = -1;

  constructor(
    private readonly _fb: FormBuilder,
    private readonly _themeService: ThemeService,
    private readonly _billingService: BillingService,
    private readonly _toastService: ToastService,
    private readonly _sessionHelper: SessionHelperService
  ) {
    const storedUser: { id: number } | null = this._sessionHelper.getItem<{ id: number }>('user');
    this.userId = storedUser ? storedUser.id : -1;
  }

  ngOnInit(): void {
    this.billingForm = this._fb.group({
      PaymentMethod: [1, Validators.required],
      BillingAddress: ['', Validators.required],
      UserId: [null, [Validators.required, Validators.min(1)]]
    });

    this._themeService.theme$.subscribe((dark: boolean) => {
      this.isDarkMode = dark;
    });
  }

  onSubmit(): void {
    if (this.billingForm.invalid) {
      this._toastService.showError('Please fill all required fields correctly.');
      return;
    }

    const billingData: IUserWithBilling = this.billingForm.value;

    this._billingService.updateBilling(billingData).subscribe({
      next: (): void => this._toastService.showSuccess('Billing updated successfully.'),
      error: (err: Error): void =>
        this._toastService.showError(`Failed to update billing: ${err?.message || 'Unknown error'}`)
    });
  }
}
