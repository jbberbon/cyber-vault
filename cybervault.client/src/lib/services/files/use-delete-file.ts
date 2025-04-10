import type {IFileDelete} from "@/lib/interfaces/file-interface.ts";
import type {AxiosError, AxiosResponse} from "axios";
import {axiosConfig} from "@/lib/configs/axios-config.ts";
import {deleteFile as deleteFileEndpoint} from "@/lib/api/endpoints/files.ts";
import {useMutation} from "@tanstack/vue-query";

const deleteFile = async (
  {fileName, parentDirectoryId}: IFileDelete
): Promise<AxiosResponse> => {
  return await axiosConfig.delete(
    deleteFileEndpoint, {
      data:
        {fileName, parentDirectoryId}
    }
  );
}

export const useDeleteFile = () => {
  return useMutation<AxiosResponse, AxiosError, IFileDelete>({
    mutationFn: deleteFile
  })
}
