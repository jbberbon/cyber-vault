import type {AxiosError, AxiosResponse} from "axios";
import {axiosConfig} from "@/lib/configs/axios-config.ts";
import {logout} from "@/lib/api/endpoints/auth-endpoints.ts";
import {useMutation} from "@tanstack/vue-query";

const logoutUser = async (): Promise<AxiosResponse> => {
  return await axiosConfig.post(logout);
};

export const useLogout = () => {
  return useMutation<AxiosResponse, AxiosError>({
    mutationFn: logoutUser,
  });
};
