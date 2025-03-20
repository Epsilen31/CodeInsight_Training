export interface ILogin {
  id: string;
  name: string;
  email: string;
  role: string;
  token: string;
}
export interface IRegisterUser {
  name: string;
  email: string;
  password: string;
  phone?: string;
  roleId?: string;
}
