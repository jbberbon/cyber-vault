import type {ICreateFolder} from "@/lib/interfaces/file-interface.ts";
import type {AxiosError, AxiosResponse} from "axios";
import {axiosConfig} from "@/lib/configs/axios-config.ts";
import {createFolder as createFolderEndpoint} from "@/lib/api/endpoints/folders.ts";
import {useMutation} from "@tanstack/vue-query";

const createFolder = async (body: ICreateFolder): Promise<AxiosResponse> => {
  if (!body.parentDirectoryId) {
    delete body.parentDirectoryId
  }
  return await axiosConfig.post(createFolderEndpoint, body)
}

export const useCreateFolder = () => {
  return useMutation<AxiosResponse, AxiosError, ICreateFolder>({
    mutationFn: createFolder
  })
}
