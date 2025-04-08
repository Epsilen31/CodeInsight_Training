import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  // duration 3 seconds
  private readonly duration = 3000; // Duration in milliseconds

  constructor(private readonly messageService: MessageService) {}

  showSuccess(message: string): void {
    this.messageService.add({
      severity: 'success',
      summary: 'Success', // title
      detail: message,
      life: this.duration
    });
  }

  showError(message: string): void {
    this.messageService.add({
      severity: 'error',
      summary: 'Error', // title
      detail: message,
      life: this.duration
    });
  }

  showInfo(message: string): void {
    this.messageService.add({
      severity: 'info',
      summary: 'Info', // title
      detail: message,
      life: this.duration
    });
  }

  showWarning(message: string): void {
    this.messageService.add({
      severity: 'warn',
      summary: 'Warning', // title
      detail: message,
      life: this.duration
    });
  }

  clear(): void {
    this.messageService.clear();
  }
}
