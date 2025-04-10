<script setup lang="ts">
import {ICONS_MAP} from "@/lib/constants/icons-map.ts";
import type {IFile} from "@/lib/interfaces/file-interface.ts";
import FilePopover from "@/pages/protected/my-vault/components/file/FilePopover.vue";
import {useFilePreviewStore} from "@/lib/stores/file-preview-store.ts";
import {FILE_TYPE_TO_GENERAL_TYPE} from "@/lib/constants/file-type-map.ts";
import {splitFileNameAndExtension} from "@/lib/helpers/file-extension-helper.ts";

const props = defineProps<{
  file: IFile;
}>();


// 01. Extract FileName and File Extension
const fileName = props.file.name;
const {fileExtension} = splitFileNameAndExtension(fileName);

// 02. Map file to the nearest Icon that represents it
const generalType = FILE_TYPE_TO_GENERAL_TYPE[fileExtension] ?? null;
const icon = ICONS_MAP[generalType] ?? ICONS_MAP['default'];

// FilePreview Store
const filePreview = useFilePreviewStore();
const openFileViewer = () => {
  console.log("opening file viewer for", props.file);
  filePreview.addFile(props.file);
}
</script>

<template>
  <div class="relative">
    <div
      class="bg-[var(--p-surface-100)] dark:bg-[var(--p-surface-800)] flex items-center justify-between rounded-sm cursor-pointer hover:bg-[var(--p-surface-200)] dark:hover:bg-[var(--p-surface-700)]"
      @click="openFileViewer"
    >
      <div class="flex items-center flex-col !bg-[var(--p-surface-0) w-full items-center">
        <div class="flex justify-between w-full items-center truncate py-2 pl-3 pr-1">
          <p class=" w-[90%] !truncate">{{ fileName }}</p>
        </div>

        <div class="w-full p-2 pt-0">
          <div
            class="bg-[var(--p-surface-50)] dark:bg-[var(--p-surface-950)] w-full h-[200px] flex items-center justify-center rounded-md">
            <i :class="icon"
               class="scale-500 dark:text-[var(--p-surface-800)] text-[var(--p-surface-300)]"/>
          </div>
        </div>
      </div>
    </div>

    <FilePopover :file="props.file" class="absolute !top-1 !right-1"/>
  </div>
</template>
