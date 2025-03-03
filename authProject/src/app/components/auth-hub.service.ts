import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthHubService {
  public apiUrl: string = `${environment.baseurl}/api/BillingAndSubscription/Auth`;

  constructor() {}
}
