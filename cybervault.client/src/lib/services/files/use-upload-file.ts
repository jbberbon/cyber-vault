import type {IUploadFile} from "@/lib/interfaces/file-interface.ts";
import type {AxiosError, AxiosResponse} from "axios";
import {axiosConfig} from "@/lib/configs/axios-config.ts";
import {uploadFile as uploadFileEndpoint} from "@/lib/api/endpoints/files.ts";
import {useMutation} from "@tanstack/vue-query";
import {useFileProgressStore} from "@/lib/stores/file-progress-store.ts";

const uploadFile = async (body: IUploadFile): Promise<AxiosResponse> => {
  // Step 1: Add the file to the upload list in the store
  const fileProgressStore = useFileProgressStore()
  fileProgressStore.addFile(body.file.name, true);

  // 02. Store the Payload in FormData
  const formData = new FormData();
  formData.append('file', body.file);
  if (body.parentFolderId) {
    formData.append('parentFolderId', body.parentFolderId)
  }

  // 03. Perform axios request
  const res = await axiosConfig.post(uploadFileEndpoint, formData, {
    timeout: 1000 * 60 * 5,
    headers: {
      'Content-Type': 'multipart/form-data',
    },
    onUploadProgress: (progressEvent) => {
      if (progressEvent.total) {
        // Calculate the progress percentage
        const progress = Math.round((progressEvent.loaded * 100) / progressEvent.total);

        // Continuously update the progress of the upload in the store
        fileProgressStore.updateProgress(body.file.name, progress, progress === 100);
      }
    }
  });

  // 04. Mark the upload as done and not awaiting
  fileProgressStore.updateProgress(body.file.name, 100, false);

  // 05. Return
  return res;
}

export const useUploadFile = () => {
  return useMutation<AxiosResponse, AxiosError, IUploadFile>({
    mutationFn: uploadFile
  })
}
