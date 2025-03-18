export interface IUser {
  id: number;
  name: string;
  email: string;
  password: string;
  phone: string;
  role: string;
  isActive?: boolean;
}
