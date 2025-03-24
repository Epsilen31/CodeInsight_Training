import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  private readonly _loadingSubject: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  loading$: Observable<boolean> = this._loadingSubject.asObservable();

  loadingOn(): void {
    this._loadingSubject.next(true);
  }

  loadingOff(): void {
    this._loadingSubject.next(false);
  }
}
