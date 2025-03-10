import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ErrorDialogService {
  private readonly _errorSubject: Subject<string> = new Subject<string>();
  error$: Observable<string> = this._errorSubject.asObservable();

  showError(message: string): void {
    console.log('[ErrorDialogService] Broadcasting error:', message);
    this._errorSubject.next(message);
  }
}
