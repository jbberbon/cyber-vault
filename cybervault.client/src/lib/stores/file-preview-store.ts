import {defineStore} from 'pinia'
import {ref} from "vue";
import type {IFile, IFileSas} from "@/lib/interfaces/file-interface.ts";
import {FILE_TYPE_TO_GENERAL_TYPE} from "@/lib/constants/file-type-map.ts";
import {splitFileNameAndExtension} from "@/lib/helpers/file-extension-helper.ts";

export const useFilePreviewStore = defineStore('filePreview', () => {
  const file = ref<IFile>()
  const fileType = ref<string>()
  const previewSas = ref<string>()
  const downloadSas = ref<string>()

  const addFile = (newFile: IFile) => {
    if (!newFile) return
    file.value = newFile;

    const {fileExtension} = splitFileNameAndExtension(newFile.name)
    fileType.value = FILE_TYPE_TO_GENERAL_TYPE[fileExtension.toLowerCase()]
  }

  const updateSasUrl = (sasUrl: IFileSas) => {
    if (!file.value || !sasUrl) return

    if (sasUrl.previewSas) {
      previewSas.value = sasUrl.previewSas
    }

    if (sasUrl.downloadSas) {
      downloadSas.value = sasUrl.downloadSas
    }
  }

  const reset = () => {
    file.value = undefined;
  }

  return {
    file,
    fileType,
    previewSas,
    downloadSas,
    addFile,
    updateSasUrl,
    reset
  }
})
