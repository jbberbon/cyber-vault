<script setup lang="ts">
import {ICONS_MAP} from "@/lib/constants/icons-map.ts";
import type {IFile} from "@/lib/interfaces/file-interface.ts";
import {ref, defineProps} from "vue";
import {useRouter} from "vue-router";
import FolderPopover from "@/pages/protected/my-vault/components/folder/FolderPopover.vue";

const props = defineProps<{
  folder: IFile;
}>();
const router = useRouter()
// 01. Process Toggle dependencies
const op = ref();
// 02. Extract FolderName and ID
const id = props?.folder?.serverAssignedId;
const folderName = props?.folder?.name;

/* Icon selection */
const folderIcon = ICONS_MAP['folder'];

// Handle Double Click on folder
const navigateFolder = () => {
  if (id) {
    router.push({name: 'my-vault-subdirectory', params: {id}})
  }
}

</script>

<template>
  <div class="relative">
    <div @click="navigateFolder"
         class="bg-[var(--p-surface-100)] dark:bg-[var(--p-surface-800)] py-2 px-3 pr-1 justify-between rounded-sm cursor-pointer hover:bg-[var(--p-surface-200)]">
      <div class="w-full h-full flex items-center gap-2 !truncate">
        <i :class="folderIcon"></i>
        <p class="!truncate">{{ folderName }}</p>
      </div>
    </div>

    <FolderPopover :folder="props.folder" class="absolute !top-1 !right-1 z-999"/>
  </div>
</template>
