export interface IUserWithBilling {
  id: number;
  billingAddress: string;
  paymentMethod: string;
  billingDate: string;
  userId: number;
}

export interface IBillingInfo {
  message: string;
  usersWithBilling: IUserWithBilling[];
}
