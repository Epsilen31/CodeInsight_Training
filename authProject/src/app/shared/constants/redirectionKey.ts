export const RedirectKey = {
  LOGIN: '/login',
  REGISTER: '/register',

  REDIRECT_TO_DASHBOARD: '/billing-subscription/dashboard',

  // USER REDIRECTOON ROUTE
  REDIRECT_TO_ALL_USER_DETAILS: '/billing-subscription/user/get-users',
  REDIRECT_TO_USER_DETAILS: '/billing-subscription/user/get-user-by-id',
  REDIRECT_TO_ADD_USER: '/billing-subscription/user/add-user',
  REDIRECT_TO_UPDATE_USER: '/billing-subscription/user/update-user',

  // SUBSCRIPTION REDIRECTS
  REDIRECT_TO_SUBSCRIPTION_DETAILS:
    '/billing-subscription/subscription/get-subscription-by-user-id',
  REDIRECT_TO_ADD_SUBSCRIPTION: '/billing-subscription/subscription/create-user-subscription-plan',
  REDIRECT_TO_UPDATE_SUBSCRIPTION: '/billing-subscription/subscription/update-user-subscription',

  // BILLING REDIRECTS
  REDIRECT_TO_UPDATE_BILLING: '/billing-subscription/billing/update-billing',
  REDIRECT_TO_USER_WITH_BILLING: '/billing-subscription/billing/get-users-with-billing',

  // PAYING REDIRECTS
  REDIRECT_TO_PAYING: '/billing-subscription/payment/create-payment',
  REDIRECT_TO_OVERDUE_PAYMENTS: '/billing-subscription/payment/get-overdue-payments'
};
