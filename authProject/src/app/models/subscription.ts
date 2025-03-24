export interface ISubscription {
  subscriptionId: number;
  userId: number;
  planType: number;
  startDate: string;
  endDate: string;
  subscriptionStatus: number;
}

export interface ISubscriptionRequest {
  planType: number;
  startDate: string;
  endDate: string;
  subscriptionStatus: number;
  userId: number;
}

export interface ISubscriptionDetail {
  message: string;
  subscription: ISubscription[];
}

export interface ICreateSubscriptionResponse {
  message: string;
  subscription: ISubscription;
}
