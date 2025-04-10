<script setup lang="ts">
import {
  PopoverArrow,
  PopoverContent,
  PopoverPortal,
  PopoverRoot,
  PopoverTrigger
} from 'radix-vue'
import Button from "primevue/button";
import {ref} from "vue";
import ConfirmFileDelete from "@/components/dialogs/ConfirmFileDelete.vue";
import type {IFile} from "@/lib/interfaces/file-interface.ts";
import DownloadDialogHidden from "@/components/dialogs/DownloadDialogHidden.vue";

const props = defineProps<{
  file: IFile;
  class: string
}>();

const isPopoverOpen = ref(false);

const isConfirmDeleteOpen = ref(false);
const isRenameDialogOpen = ref(false);
const isDownloadDialogOpen = ref(false)
</script>

<template>
  <PopoverRoot :open="isPopoverOpen" @update:open="isPopoverOpen = !isPopoverOpen">
    <PopoverTrigger class="bg-transparent border-none p-0" :class="props.class ?? ''">
      <Button
        ref="buttonEl"
        icon="pi pi-ellipsis-v"
        severity="contrast"
        variant="text"
        size="small"
        rounded
      />
    </PopoverTrigger>
    <PopoverPortal>
      <PopoverContent align="start" class="bg-white dark:bg-[var(--p-surface-900)] rounded-md shadow-md p-1">
        <div class="flex flex-col">
          <Button label="Rename" icon="pi pi-pen-to-square" severity="secondary" variant="text"
                  class="!flex !justify-start !w-full"
                  @click="isPopoverOpen = false; isRenameDialogOpen = true;"/>
          <Button label="Download" icon="pi pi-download" severity="secondary" variant="text"
                  class="!flex !justify-start !w-full"
                  @click="isPopoverOpen = false; isDownloadDialogOpen = true; "/>
          <Button label="Delete" icon="pi pi-trash"
                  class="p-button-text p-button-danger !flex !justify-start !w-full"
                  @click="isPopoverOpen = false; isConfirmDeleteOpen = true"/>
        </div>
        <PopoverArrow class="fill-white dark:fill-[var(--p-surface-900)]"/>
      </PopoverContent>
    </PopoverPortal>
  </PopoverRoot>

  <ConfirmFileDelete :name="props.file.name" v-model:is-open="isConfirmDeleteOpen"/>
  <DownloadDialogHidden :name="props.file.name" v-model:is-open="isDownloadDialogOpen"/>
</template>
