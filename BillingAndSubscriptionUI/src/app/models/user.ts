export interface IUser {
  id: number;
  name: string;
  email: string;
  password: string;
  phone: string;
  role?: {
    roleName: string;
  };
  isActive?: boolean;
}

export interface IUserResponse {
  message: string;
  users: IUser[];
}

export interface IUserDetail {
  message: string;
  user: IUpdateUser;
}

export interface IUpdateUser {
  id: number;
  name: string;
  email: string;
  password: string;
  phone: string;
  role: string;
}

export interface UploadResponse {
  message: string;
}
