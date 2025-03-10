import { Injectable, Type } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { UserComponent } from '../components/user/user.component';

@Injectable({
  providedIn: 'root',
})
export class DashboardService {
  private readonly _activeComponentSubject =
    new BehaviorSubject<Type<any> | null>(null);
  activeComponent$ = this._activeComponentSubject.asObservable();

  setActiveComponent(componentName: string): void {
    const componentMap: Record<string, Type<any>> = {
      UserComponent: UserComponent,
    };

    this._activeComponentSubject.next(componentMap[componentName] || null);
  }
}
