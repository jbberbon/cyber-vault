import {useMutation} from '@tanstack/vue-query';
import {login} from "@/lib/api/endpoints/auth-endpoints.js";
import {axiosConfig} from "@/lib/configs/axios-config.ts";
import type {ILoginUser, ILoginResponse} from '../../interfaces/user-interface.ts';
import type {AxiosError, AxiosResponse} from "axios";

const loginUser = async (credentials: ILoginUser): Promise<AxiosResponse<ILoginResponse>> => {
  return await axiosConfig.post(login, credentials);
};

export const useLogin = () => {
  return useMutation<AxiosResponse, AxiosError, ILoginUser>({
    mutationFn: loginUser,
  });
};
