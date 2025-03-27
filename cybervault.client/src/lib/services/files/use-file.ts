import {useQuery, useMutation} from '@tanstack/vue-query';
import type {AxiosError, AxiosResponse} from "axios";
import {axiosConfig} from "@/lib/configs/axios-config.ts";
import {
  listFiles as listFilesEndpoint,
  deleteFile as deleteFileEndpoint,
  uploadFile as uploadFileEndpoint
} from "@/lib/api/endpoints/files.ts";
import {
  deleteFolder as deleteFolderEndpoint,
  createFolder as createFolderEndpoint,
} from "@/lib/api/endpoints/folders.ts";

import type {
  ICreateFolder,
  IFile,
  IFileDelete,
  IUploadFile
} from "@/lib/interfaces/file-interface.ts";
import {computed, type Ref} from "vue";
import {useUploadStore} from "@/lib/stores/upload-progress-store.ts";

const fetchFileList = async (folderId?: string) => {
  return axiosConfig.get<IFile[]>(listFilesEndpoint, {
    params: folderId ? {directoryId: folderId} : undefined
  });
};

const deleteFolder = async (directoryId: string): Promise<AxiosResponse> => {
  return await axiosConfig.delete(deleteFolderEndpoint, {data: {directoryId}});
}

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

  console.log("FORM DATA", formData)

  // Step 2: Upload the file with Axios and track progress
  return await axiosConfig.post(uploadFileEndpoint, formData, {
    headers: {
      'Content-Type': 'multipart/form-data',
    },
    onUploadProgress: (progressEvent) => {
      if (progressEvent.total) {
        // Calculate the progress percentage
        const progress = Math.round((progressEvent.loaded * 100) / progressEvent.total);

        // Step 3: Continuously update the progress of the upload in the store
        uploadStore.updateProgress(body.file.name, progress); // Updates the progress in the store
        console.log("Current upload and progress", uploadStore.uploads)
      }
    }
  });
}


export const useListFiles = (folderId: Ref<string | undefined>) => {
  const queryKey = computed(() => ['list-files', folderId.value || "root"]);
  return useQuery<AxiosResponse<IFile[]>, AxiosError>({
    queryKey: queryKey,
    queryFn: () => fetchFileList(folderId.value),
    enabled: true,
    structuralSharing: false
    /*staleTime: 0,*/
    /*gcTime: 0*/
  });
};
export const useDeleteFile = () => {
  return useMutation<AxiosResponse, AxiosError, IFileDelete>({
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

