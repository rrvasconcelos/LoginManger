export interface UserCreateRequest {
  email: string;
  document: string;
  password: string;
  confirmPassword: string;
}

export interface UserCreateResponse {
  id: string;
  email: string;
  document: string;
}
