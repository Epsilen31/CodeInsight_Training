import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  constructor(private readonly _toastr: ToastrService) {}

  showSuccess(message: string): void {
    this._toastr.success(message);
  }

  showError(message: string): void {
    this._toastr.error(message);
  }

  showInfo(message: string): void {
    this._toastr.info(message);
  }

  showWarning(message: string): void {
    this._toastr.warning(message);
  }
}
