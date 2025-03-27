import { Injectable } from '@angular/core';
import { HttpClientService } from '../core/http-client.service';
import { IOverduePaymentsResponse, IPaymentPayload } from '../models/payment';
import { Observable } from 'rxjs';
import { RouteKey } from '../shared/constants/routeKey';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  constructor(private readonly _httpService: HttpClientService) {}

  makePayment(payload: IPaymentPayload): Observable<IPaymentPayload> {
    return this._httpService.post(`${RouteKey.CREATE_PAYMENT_URL}`, payload);
  }

  fetchOverduePayments(subscriptionId: number): Observable<IOverduePaymentsResponse> {
    return this._httpService.get(`${RouteKey.FETCH_OVERDUE_PAYMENTS_URL}`, {
      subscriptionId: subscriptionId.toString()
    });
  }
}
