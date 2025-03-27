<script setup lang="ts">
import {computed} from "vue";
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import Dialog from 'primevue/dialog';
import {useRoute} from 'vue-router';
import {useField, useForm} from 'vee-validate';
import {createFolderSchema} from "@/lib/schemas/create-folder-schema.ts";
import {useCreateFolder} from "@/lib/services/files/use-file.ts";
import type {ICreateFolder} from "@/lib/interfaces/file-interface.ts";
import {useCallToast} from "@/lib/hooks/use-call-toast.ts";
import CREATE_FOLDER_ERR_MAP
  from "@/lib/constants/api-error-messages/file-error-messages.ts/create-folder-error-msg.ts";

const props = defineProps({
  isOpen: Boolean,
});

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

const createFolderToast = useCallToast(CREATE_FOLDER_ERR_MAP)
const route = useRoute();
const folderId = computed(() => {
  const id = route.params.id; // Returns array of IDs
  return Array.isArray(id) ? id[0] : id;
});

const {handleSubmit, errors, resetForm} = useForm({
  validationSchema: createFolderSchema,
  initialValues: {
    folderName: "Untitled folder"
  }
});
const {value: folderName} = useField<string | null>('folderName');
const {mutateAsync, error} = useCreateFolder();

const onSubmit = handleSubmit(async (params: ICreateFolder) => {
  try {
    params.parentDirectoryId = folderId.value;
    await mutateAsync(params)
    closeAndUpdate()
    resetForm()
    createFolderToast({
      detail: "Successfully created " + params.folderName
    })
  } catch {
    createFolderToast({
      httpCode: error?.value?.response?.status ?? 500
    })
  }
})
</script>

<template>
  <Dialog v-model:visible="modelValue" modal header="New folder">
    <form @submit="onSubmit" class="flex flex-col gap-4 min-w-[280px]">
      <div class="flex flex-col">
        <InputText v-model="folderName"/>
        <span class="text-sm text-red-400">{{ errors.folderName }}</span>
      </div>
      <div class="flex justify-end gap-2">
        <Button type="button" label="Cancel" severity="secondary" @click="closeAndUpdate"></Button>
        <Button type="submit" label="Save"></Button>
      </div>
    </form>
  </Dialog>
</template>
