import type {Ref} from "vue";
import {computed} from "vue";
import {useQuery} from "@tanstack/vue-query";
import type {AxiosError, AxiosResponse} from "axios";
import type {IFileList} from "@/lib/interfaces/file-interface.ts";
import {axiosConfig} from "@/lib/configs/axios-config.ts";
import {listFiles as listFilesEndpoint} from "@/lib/api/endpoints/files.ts";


const fetchFileList = async (folderId?: string): Promise<AxiosResponse<IFileList>> => {
  return axiosConfig.get<IFileList>(listFilesEndpoint, {
    params: folderId ? {directoryId: folderId} : undefined
  });
};
export const useListFiles = (folderId: Ref<string | undefined>) => {
  const queryKey = computed(() => ['list-files', folderId.value || "root"]);
  return useQuery<AxiosResponse<IFileList>, AxiosError>({
    queryKey: queryKey,
    queryFn: () => fetchFileList(folderId.value),
    enabled: true,
    //structuralSharing: false
    /*staleTime: 0,*/
    /*gcTime: 0*/
  });
};
