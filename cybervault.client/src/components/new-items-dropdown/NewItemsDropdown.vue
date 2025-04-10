<script setup lang="ts">
import {computed, ref} from "vue";
import TieredMenu from 'primevue/tieredmenu';
import NewFolderModal from "@/components/dialogs/NewFolderModal.vue";
import {useUploadFile} from "@/lib/services/files/use-upload-file.ts";
import {useRoute} from "vue-router";
import type {IUploadFile} from "@/lib/interfaces/file-interface.ts";

const newButtonPopout = ref();
const toggleNewButton = (event: MouseEvent) => {
  newButtonPopout.value.toggle(event);
};

// List of Modal Refs
const isNewFolderModalOpen = ref(false)

const items = ref([
  {
    label: 'File upload',
    icon: 'pi pi-file',
    command: () => {
      fileInputRef.value?.click();
    }
  },
  {
    label: 'Folder upload',
    icon: 'pi pi-folder',
    command: () => {
    }
  },
  {
    separator: true
  },
  {
    label: 'Create folder',
    icon: 'pi pi-folder-plus',
    command: () => {
      isNewFolderModalOpen.value = true
    }
  }
]);

const fileInputRef = ref<HTMLInputElement | null>(null);

// Retrieve Parent Directory ID
const route = useRoute();
const folderId = computed(() => {
  const id = route.params.id; // Returns array of IDs
  return Array.isArray(id) ? id[0] : id;
});

// Initialize UseFileUpload Hook and fileUpload Handler
const {mutateAsync, error} = useUploadFile()

const handleUploadFile = async (event: Event) => {
  const file = (event.target as HTMLInputElement).files?.[0];
  if (file) {
    try {
      const params: IUploadFile = {file}
      if (folderId) {
        params.parentFolderId = folderId.value
      }
      await mutateAsync(params)
    } catch {
      console.log("something went wrong", error)
    }
  }
}

</script>
<template>
  <div @click="toggleNewButton" aria-controls="overlay_newButtonMenu">
    <!-- This is where the button trigger goes in -->
    <slot></slot>
  </div>
  <TieredMenu ref="newButtonPopout" id="overlay_newButtonMenu" :model="items" popup/>

  <!-- Hidden File Input -->
  <input
    type="file"
    ref="fileInputRef"
    style="display: none"
    @change="handleUploadFile"
    accept="*/*"/>

  <!-- List of modals  -->
  <NewFolderModal v-model:is-open="isNewFolderModalOpen"/>
</template>
