﻿<script setup lang="ts">
import Dialog from "primevue/dialog"
import Button from "primevue/button";
import {computed} from "vue";
import {useDeleteFile} from "@/lib/services/files/use-delete-file.ts";
import type {IFileDelete} from "@/lib/interfaces/file-interface.ts";
import {useCallToast} from "@/lib/hooks/use-call-toast.ts";
import DELETE_FILE_ERR_MAP
  from "@/lib/constants/api-error-messages/file-error-messages.ts/delete-file-error-msg.ts";
import {useRoute} from "vue-router";

const props = defineProps({
  name: {
    type: String,
    required: true
  },
  isOpen: Boolean
})
const route = useRoute()
const parentFolderId = Array.isArray(route.params.id)
  ? route.params.id[0]  // Get the first instance of id
  : route.params.id;

const deleteFileCallToast = useCallToast(DELETE_FILE_ERR_MAP)
// 01. Create a computed property for binding to Dialog's visible prop
const emit = defineEmits();
const modelValue = computed({
  get: () => props.isOpen,
  set: (value) => emit('update:isOpen', value)
});

// 02. Emit an event to close the modal when Cancel or Save is clicked
const closeAndUpdate = () => {
  emit('update:isOpen', false);
};

// 03. Initialize Delete file hook and onDelete handler
const {
  mutateAsync: fileMutateAsync,
  error: deleteFileError
} = useDeleteFile();
const onDeleteFile = async () => {
  const params: IFileDelete = {
    fileName: props.name,
    parentDirectoryId: parentFolderId
  }
  try {
    await fileMutateAsync(params)
    deleteFileCallToast({
      summary: "Deleted " + params.fileName
    })
    closeAndUpdate()
  } catch {
    deleteFileCallToast({
      httpCode: deleteFileError?.value?.response?.status ?? 500
    })
    closeAndUpdate()
  }
}
</script>

<template>
  <Dialog v-model:visible="modelValue" modal header="Confirmation">
    <div class="flex flex-col gap-4 max-w-[280px]">
      <p>Do you want to delete <b>{{ props.name }}</b>?</p>
      <div class="flex justify-end gap-2">
        <Button type="button" size="small" outlined label="Cancel" severity="secondary"
                @click="closeAndUpdate"></Button>
        <Button @click="onDeleteFile" size="small" severity="danger" label="Delete"></Button>
      </div>
    </div>
  </Dialog>
</template>
