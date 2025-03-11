import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LoadingService {
  private readonly _loadingSubject = new BehaviorSubject<boolean>(false);
  loading$ = this._loadingSubject.asObservable();

  loadingOn(): void {
    this._loadingSubject.next(true);
  }

  loadingOff(): void {
    this._loadingSubject.next(false);
  }
}
