import type {AxiosError, AxiosResponse} from "axios";
import {axiosConfig} from "@/lib/configs/axios-config.ts";
import {deleteFolder as deleteFolderEndpoint} from "@/lib/api/endpoints/folders.ts";
import {useMutation} from "@tanstack/vue-query";

const deleteFolder = async (directoryId: string): Promise<AxiosResponse> => {
  return await axiosConfig.delete(deleteFolderEndpoint, {data: {directoryId}});
}
export const useDeleteFolder = () => {
  return useMutation<AxiosResponse, AxiosError, string>({
    mutationFn: deleteFolder
  })
}



