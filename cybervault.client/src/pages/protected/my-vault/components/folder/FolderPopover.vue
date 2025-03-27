<script setup lang="ts">
import {
  PopoverArrow,
  PopoverContent,
  PopoverPortal,
  PopoverRoot,
  PopoverTrigger
} from 'radix-vue'
import Button from "primevue/button";
import {defineProps, ref} from "vue";
import type {IFile} from "@/lib/interfaces/file-interface.ts";
import ConfirmFolderDelete from "@/components/dialogs/ConfirmFolderDelete.vue";

const props = defineProps<{
  folder: IFile;
  class: String
}>();

const folderId = props.folder.serverAssignedId

const isConfirmDeleteOpen = ref(false);
const isRenameDialogOpen = ref(false);

</script>

<template>
  <PopoverRoot>
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
      <PopoverContent align="start"
                      class="bg-white dark:bg-[var(--p-surface-900)] rounded-md shadow-md p-1">
        <div class="flex flex-col">
          <Button label="Rename" icon="pi pi-pen-to-square" severity="secondary" variant="text"
                  class="!flex !justify-start !w-full"
                  @click="isRenameDialogOpen = true; "/>
          <Button label="Delete" icon="pi pi-trash"
                  class="p-button-text p-button-danger !flex !justify-start !w-full"
                  @click="isConfirmDeleteOpen = true"/>
        </div>
        <PopoverArrow class="fill-white fill-white dark:fill-[var(--p-surface-900)]"/>
      </PopoverContent>
    </PopoverPortal>
  </PopoverRoot>

  <ConfirmFolderDelete :name="props.folder.name" :directory-id="folderId"
                       v-model:is-open="isConfirmDeleteOpen"/>
</template>

<style>

</style>
