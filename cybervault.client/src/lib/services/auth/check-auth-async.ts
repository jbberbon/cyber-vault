import {checkAuth as checkAuthEndpoint} from "@/lib/api/endpoints/auth-endpoints.js";
import {axiosConfig} from "@/lib/configs/axios-config.ts";
export const checkAuthAsync = async (): Promise<boolean> => {
  try {
    await axiosConfig.get(checkAuthEndpoint);
    return true
  } catch {
    return false;
  }
};
