import { Injectable } from '@angular/core';
import { HttpClientService } from '../core/http-client.service';
import { map, Observable } from 'rxjs';
import {
  ICreateSubscriptionResponse,
  ISubscriptionDetail,
  ISubscriptionRequest
} from '../models/subscription';
import { RouteKey } from './../shared/constants/routeKey';
import { ISubscription } from './../models/subscription';

@Injectable({
  providedIn: 'root'
})
export class SubscriptionService {
  constructor(private readonly _httpClientService: HttpClientService) {}

  getSubscriptionByUserId(userId: number): Observable<ISubscriptionDetail> {
    return this._httpClientService.get<ISubscriptionDetail>(
      `${RouteKey.GET_SUBSCRIPTION_BY_ID_URL}/${userId}`
    );
  }

  createSubscription(subscription: ISubscriptionRequest): Observable<ICreateSubscriptionResponse> {
    const { subscriptionId, ...subscriptionPayload } = subscription as any;
    return this._httpClientService
      .post<ICreateSubscriptionResponse>(`${RouteKey.CREATE_SUBSCRIPTION_URL}`, subscriptionPayload)
      .pipe(
        map((response: ICreateSubscriptionResponse) => {
          return response;
        })
      );
  }

  updateSubscription(
    userId: number,
    subscription: ISubscription
  ): Observable<ISubscriptionRequest> {
    return this._httpClientService.put<ISubscription>(
      `${RouteKey.UPDATE_SUBSCRIPTION_URL}/${userId}`,
      subscription
    );
  }
}
