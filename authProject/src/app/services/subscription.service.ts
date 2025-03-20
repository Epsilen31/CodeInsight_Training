import { Injectable } from '@angular/core';
import { HttpClientService } from '../core/http-client.service';
import { Observable } from 'rxjs';
import {
  ISubscription,
  ISubscriptionDetail,
  ISubscriptionRequest,
} from '../models/subscription';

@Injectable({
  providedIn: 'root',
})
export class SubscriptionService {
  constructor(private readonly _httpClientService: HttpClientService) {}

  getSubscriptionByUserId(userId: number): Observable<ISubscriptionDetail> {
    return this._httpClientService.get<ISubscriptionDetail>(
      `UserSubscription/GetSubscriptionByUserId/${userId}`
    );
  }

  createSubscription(
    subscription: ISubscriptionRequest
  ): Observable<ISubscriptionRequest> {
    console.log('Sending API request with data:', subscription);
    return this._httpClientService.post<ISubscriptionRequest>(
      'UserSubscription/CreateUserSubscriptionPlan',
      subscription
    );
  }

  updateSubscription(
    id: number,
    subscription: ISubscription
  ): Observable<ISubscriptionRequest> {
    console.log('Updating subscription', subscription);
    return this._httpClientService.put<ISubscription>(
      `UserSubscription/UpdateUserSubscriptionPlan/${id}`,
      subscription
    );
  }
}
