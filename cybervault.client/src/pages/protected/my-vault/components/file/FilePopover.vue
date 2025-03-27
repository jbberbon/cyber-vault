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
import ConfirmFileDelete from "@/components/dialogs/ConfirmFileDelete.vue";
import type {IFile} from "@/lib/interfaces/file-interface.ts";

const props = defineProps<{
  file: IFile;
}>();

const isConfirmDeleteOpen = ref(false);
const isRenameDialogOpen = ref(false);

</script>

<template>
  <PopoverRoot>
    <PopoverTrigger class="bg-transparent border-none p-0">
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
      <PopoverContent align="start" class="bg-white rounded-md shadow-md p-1">
        <div class="flex flex-col">
          <Button label="Rename" icon="pi pi-pen-to-square" severity="secondary" variant="text"
                  class="!flex !justify-start !w-full"
                  @click="isRenameDialogOpen = true; "/>
          <Button label="Delete" icon="pi pi-trash"
                  class="p-button-text p-button-danger !flex !justify-start !w-full"
                  @click="isConfirmDeleteOpen = true"/>
        </div>
        <PopoverArrow class="fill-white"/>
      </PopoverContent>
    </PopoverPortal>
  </PopoverRoot>

  <ConfirmFileDelete :name="props.file.name" v-model:is-open="isConfirmDeleteOpen"/>
</template>

<style>

</style>
