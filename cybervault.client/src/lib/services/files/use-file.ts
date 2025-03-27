import {useQuery, useMutation} from '@tanstack/vue-query';
import {type AxiosError, type AxiosResponse} from "axios";
import {axiosConfig} from "@/lib/configs/axios-config.ts";
import {
  listFiles as listFilesEndpoint,
  deleteFile as deleteFileEndpoint,
  uploadFile as uploadFileEndpoint,
  downloadFile as downloadFileEndpoint
} from "@/lib/api/endpoints/files.ts";
import {
  deleteFolder as deleteFolderEndpoint,
  createFolder as createFolderEndpoint,
} from "@/lib/api/endpoints/folders.ts";

import type {
  ICreateFolder,
  IFile,
  IFileDownloadOrDelete,
  IUploadFile
} from "@/lib/interfaces/file-interface.ts";
import {computed, type Ref} from "vue";
import {useUploadStore} from "@/lib/stores/upload-progress-store.ts";

const fetchFileList = async (folderId?: string): Promise<AxiosResponse<IFile[]>> => {
  return axiosConfig.get<IFile[]>(listFilesEndpoint, {
    params: folderId ? {directoryId: folderId} : undefined
  });
};

const deleteFolder = async (directoryId: string): Promise<AxiosResponse> => {
  return await axiosConfig.delete(deleteFolderEndpoint, {data: {directoryId}});
}

const deleteFile = async (
  {fileName, parentDirectoryId}: IFileDownloadOrDelete
): Promise<AxiosResponse> => {
  return await axiosConfig.delete(
    deleteFileEndpoint, {
      data:
        {fileName, parentDirectoryId}
    }
  );
}

const createFolder = async (body: ICreateFolder): Promise<AxiosResponse> => {
  if (!body.parentDirectoryId) {
    delete body.parentDirectoryId
  }
  return await axiosConfig.post(createFolderEndpoint, body)
}

const uploadFile = async (body: IUploadFile): Promise<AxiosResponse> => {
  // Step 1: Add the file to the upload list in the store
  const uploadStore = useUploadStore()
  uploadStore.addUpload(body.file);

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
        uploadStore.updateProgress(body.file.name, progress, progress === 100);
      }
    }
  });

  // 04. Mark the upload as done and not awaiting
  uploadStore.updateProgress(body.file.name, 100, false);

  // 05. Return
  return res;
}

const downloadFile = async (request: IFileDownloadOrDelete): Promise<AxiosResponse<{
  sasUrl: string
}>> => {
  let queryParams = `?fileName=${request.fileName}`
  if (request.parentDirectoryId) queryParams = `${queryParams}?parentDirectoryId=${request.parentDirectoryId}`

  return await axiosConfig.get(downloadFileEndpoint + queryParams)
}

export const useListFiles = (folderId: Ref<string | undefined>) => {
  const queryKey = computed(() => ['list-files', folderId.value || "root"]);
  return useQuery<AxiosResponse<IFile[]>, AxiosError>({
    queryKey: queryKey,
    queryFn: () => fetchFileList(folderId.value),
    enabled: true,
    //structuralSharing: false
    /*staleTime: 0,*/
    /*gcTime: 0*/
  });
};
export const useDeleteFile = () => {
  return useMutation<AxiosResponse, AxiosError, IFileDownloadOrDelete>({
    mutationFn: deleteFile
  })
}
export const useDeleteFolder = () => {
  return useMutation<AxiosResponse, AxiosError, string>({
    mutationFn: deleteFolder
  })
}

export const useCreateFolder = () => {
  return useMutation<AxiosResponse, AxiosError, ICreateFolder>({
    mutationFn: createFolder
  })
}

export const useUploadFile = () => {
  return useMutation<AxiosResponse, AxiosError, IUploadFile>({
    mutationFn: uploadFile
  })
}

export const useDownloadFile = (params: IFileDownloadOrDelete) => {
  const queryKey = ['download-file', params.fileName, params.parentDirectoryId || "root"]
  return useQuery<AxiosResponse<{ sasUrl: string }>, AxiosError>({
    queryKey,
    queryFn: () => downloadFile(params),
    enabled: false // Do not download upon instantiation
  })
}

