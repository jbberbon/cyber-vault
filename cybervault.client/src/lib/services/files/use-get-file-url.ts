import type {IFileDownload, IFileSas} from "@/lib/interfaces/file-interface.ts";
import type {AxiosError, AxiosResponse} from "axios";
import {axiosConfig} from "@/lib/configs/axios-config.ts";
import {downloadFile as downloadFileEndpoint} from "@/lib/api/endpoints/files.ts";
import {useQuery} from "@tanstack/vue-query";

const fetchFileUrl = async (request: IFileDownload): Promise<AxiosResponse<IFileSas>> => {
  let queryParams = `?fileName=${request.fileName}`
  if (request.parentDirectoryId) queryParams = `${queryParams}&parentDirectoryId=${request.parentDirectoryId}`

  // If forPreview is True, add it to the query params
  if (request["downloadOrPreview.forPreview"] === true) {
    queryParams = `${queryParams}&downloadOrPreview.forPreview=${request["downloadOrPreview.forPreview"]}`
  }

  // If forDownload is True, add it to the query params
  if (request["downloadOrPreview.forDownload"] === true) {
    queryParams = `${queryParams}&downloadOrPreview.forDownload=${request["downloadOrPreview.forDownload"]}`
  }

  return await axiosConfig.get(downloadFileEndpoint + queryParams)
}

export const useGetFileUrl = (params: IFileDownload) => {
  const queryKey = [
    'download-file',
    params.fileName,
    params.parentDirectoryId || "root",
    params["downloadOrPreview.forPreview"] || false,
    params["downloadOrPreview.forDownload"] || false
  ]
  return useQuery<AxiosResponse<IFileSas>, AxiosError>({
    queryKey,
    queryFn: () => fetchFileUrl(params),
    enabled: false // Do not download upon instantiation
  })
}
