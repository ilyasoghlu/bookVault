import api from "../axios";

export interface LoginRequest{
    email: string;
    password: string;
}

export interface LoginResponse{
    token:string;
}

export interface MeResponse {
  id: string;
  username: string;
  permissions: string[];
}

export const login = async (
    data: LoginRequest
): Promise<LoginResponse> =>{
    const response = await api.post<LoginResponse>(
        "/api/Auth/login",
        data
    )

    return response.data
}

export const getMe = async (): Promise<MeResponse> => {
  const response = await api.get<MeResponse>(
    "/api/Auth/me"
  );

  return response.data;
};