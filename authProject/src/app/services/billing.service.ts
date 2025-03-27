import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from '../core/http-client.service';
import { RouteKey } from '../shared/constants/routeKey';
import { IUserWithBilling } from '../models/billing';

@Injectable({
  providedIn: 'root'
})
export class BillingService {
  constructor(private readonly httpClientService: HttpClientService) {}

  updateBilling(billingData: IUserWithBilling): Observable<IUserWithBilling> {
    return this.httpClientService.put<IUserWithBilling>(
      `${RouteKey.UPDATE_BILLING_URL}`,
      billingData
    );
  }

  getUsersWithBilling(): Observable<IUserWithBilling[]> {
    return this.httpClientService.get<IUserWithBilling[]>(`${RouteKey.GET_USERS_WITH_BILLING}`);
  }
}
