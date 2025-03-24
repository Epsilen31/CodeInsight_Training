export interface IPaymentPayload {
  subscriptionId: number;
  amount: number;
  paymentDate: string;
  paymentStatus: number;
}

export interface IPaymentMessage {
  message: string;
}

export interface IOverduePayments {
  id: string;
  subscriptionId: number;
  amount: number;
  paymentDate: string;
  paymentStatus: number;
}
