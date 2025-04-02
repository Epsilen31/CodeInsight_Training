import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from '../core/http-client.service';
import { RouteKey } from '../shared/constants/routeKey';
import { IBillingInfo, IUserWithBilling } from '../models/billing';

@Injectable({
  providedIn: 'root'
})
export class BillingService {
  constructor(private readonly _httpClientService: HttpClientService) {}

  updateBilling(billingData: IUserWithBilling): Observable<IUserWithBilling> {
    return this._httpClientService.put<IUserWithBilling>(
      `${RouteKey.UPDATE_BILLING_URL}`,
      billingData
    );
  }

  getUsersWithBilling(userId: number): Observable<IBillingInfo> {
    return this._httpClientService.get<IBillingInfo>(`${RouteKey.GET_USERS_WITH_BILLING}`, {
      userId: userId
    });
  }
}
