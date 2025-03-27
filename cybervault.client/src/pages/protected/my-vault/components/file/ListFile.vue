<script setup lang="ts">
import {ICONS_MAP, FILE_TYPE_TO_GENERAL_TYPE} from "@/lib/constants/icons-map.ts";
import type {IFile} from "@/lib/interfaces/file-interface.ts";
import {defineProps} from "vue";
import FilePopover from "@/pages/protected/my-vault/components/file/FilePopover.vue";

const props = defineProps<{
  file: IFile;
}>();


// Extract FileName and Type
const fileName = props.file.name;
const fileType = fileName.includes('.') ? fileName.split('.')[1] : "";

// Map file to the nearest Icon that represents it
const generalType = FILE_TYPE_TO_GENERAL_TYPE[fileType] ?? null;
const icon = ICONS_MAP[generalType] ?? ICONS_MAP['default'];

</script>

<template>
  <div
    class="bg-[var(--p-surface-100)] dark:bg-[var(--p-surface-800)] flex items-center justify-between rounded-sm cursor-pointer hover:bg-[var(--p-surface-200)]">
    <div class="flex items-center flex-col !bg-[var(--p-surface-0) w-full items-center">
      <div class="flex justify-between w-full items-center truncate py-2 pl-3 pr-1">
        <p class="!truncate">{{ fileName }}</p>
        <FilePopover :file="props.file" />
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
</template>
