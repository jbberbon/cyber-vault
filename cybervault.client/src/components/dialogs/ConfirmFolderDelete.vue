<script setup lang="ts">
import Dialog from "primevue/dialog"
import Button from "primevue/button";
import {computed} from "vue";
import {useDeleteFolder} from "@/lib/services/files/use-file.ts";
import {useCallToast} from "@/lib/hooks/use-call-toast.ts";
import DELETE_FOLDER_ERR_MAP
  from "@/lib/constants/api-error-messages/file-error-messages.ts/delete-folder-error-msg.ts";

const props = defineProps({
  name: {
    type: String,
    required: true
  },
  directoryId: {
    type: String,
    required: true
  },
  isOpen: Boolean
})
const toast = useCallToast(DELETE_FOLDER_ERR_MAP)
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

// 03. Initialize Delete folder hook and onDelete handler
const {
  mutateAsync,
  error
} = useDeleteFolder();
const onDeleteFolder = async () => {
  try {
    await mutateAsync(props.directoryId)
    toast({
      summary: "Successfully deleted " + props.name
    })
    closeAndUpdate()
  } catch {
    toast({
      httpCode: error?.value?.response?.status ?? 500
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
        <Button @click="onDeleteFolder" size="small" severity="danger" label="Delete"></Button>
      </div>
    </div>
  </Dialog>
</template>
