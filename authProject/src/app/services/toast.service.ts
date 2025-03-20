import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  constructor(private readonly toastr: ToastrService) {}

  showSuccess(message: string): void {
    this.toastr.success(message);
  }

  showError(message: string): void {
    this.toastr.error(message);
  }

  showInfo(message: string): void {
    this.toastr.info(message);
  }

  showWarning(message: string): void {
    this.toastr.warning(message);
  }
}
