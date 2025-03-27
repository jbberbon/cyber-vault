import {useMutation} from '@tanstack/vue-query';
import {register} from "@/lib/api/endpoints/auth-endpoints.js";
import {axiosConfig} from "@/lib/configs/axios-config.ts";
import type {IRegisterUser} from '../../interfaces/user-interface.ts';
import type {AxiosError, AxiosResponse} from "axios";

const registerUser = async (credentials: IRegisterUser): Promise<AxiosResponse> => {
  credentials.confirmPassword = credentials.password;
  return await axiosConfig.post(register, credentials);
};

export const useRegister = () => {
  return useMutation<AxiosResponse, AxiosError, IRegisterUser>({
    mutationFn: registerUser,
  });
};
