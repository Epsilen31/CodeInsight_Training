export const RouteKey = {
  // AUTHENTICATION ROUTES
  AUTH_LOGIN_URL: 'Auth/login',
  AUTH_REGISTER_URL: 'Auth/register',

  // SIDEBAR MENU ROUTES
  GET_ALL_MENU_ITEMS_URL: 'Menu/GetSidebarMenu',

  // SUBSCRIPTION ROUTES
  GET_SUBSCRIPTION_BY_ID_URL: 'UserSubscription/GetSubscriptionByUserId',
  CREATE_SUBSCRIPTION_URL: 'UserSubscription/CreateUserSubscriptionPlan',
  UPDATE_SUBSCRIPTION_URL: 'UserSubscription/UpdateUserSubscriptionPlan',

  // USER ROUTES
  GET_USER_BY_ID_URL: 'User/GetUserById',
  GET_ALL_USERS_URL: 'User/GetUsers',
  CREATE_USER_URL: 'User/AddUser',
  UPDATE_USER_URL: 'User/UpdateUser',
  DELETE_USER_URL: 'User/DeleteUser',

  // PAYMENT ROUTES
  CREATE_PAYMENT_URL: 'Payments/CreatePayment',
  FETCH_OVERDUE_PAYMENTS_URL: 'Payments/GetOverduePayments',

  // BILLING ROUTES
  UPDATE_BILLING_URL: 'Billing/UpdateBilling',
  GET_USERS_WITH_BILLING: 'Billing/GetUsersWithBilling'
};
