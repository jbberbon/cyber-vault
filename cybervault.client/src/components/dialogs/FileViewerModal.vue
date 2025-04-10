<script setup lang="ts">
import {computed, watch} from "vue";
import Drawer from 'primevue/drawer';
import {useGetFileUrl} from "@/lib/services/files/use-get-file-url.ts";
import {useFilePreviewStore} from "@/lib/stores/file-preview-store.ts";
import {getHtmlVideoType} from "@/lib/helpers/file-extension-helper.ts";
import {GENERAL_TYPES} from "@/lib/constants/file-type-map.ts";
import Toolbar from 'primevue/toolbar';
import Button from 'primevue/button'
import type {IFileSas} from "@/lib/interfaces/file-interface.ts";
import {triggerDownload} from "@/lib/helpers/trigger-download.ts";
import {useRoute} from "vue-router";

// Store for file preview
const filePreview = useFilePreviewStore();

// Compute drawer visibility directly from the store state
const isDrawerVisible = computed({
  get: () => !!filePreview.file,
  set: (value) => {
    if (!value) {
      // Close the drawer by clearing the file in the store
      filePreview.reset();
    }
  }
});

// Get current directory ID
const route = useRoute()
const currentDirectoryId = computed(() => {
  const id = route.params.id; // Returns array of IDs
  return Array.isArray(id) ? id[0] : id;
})

console.log('currentDirectoryId', currentDirectoryId.value)
// Fetch SAS URL
const getFileUrl = useGetFileUrl({
  fileName: filePreview.file?.name ?? "",
  parentDirectoryId: currentDirectoryId.value,
  "downloadOrPreview.forPreview": true,
  "downloadOrPreview.forDownload": true,
});

if (filePreview.file?.name) {
  await getFileUrl.refetch();

  const params: IFileSas = {
    downloadSas: getFileUrl.data.value?.data.downloadSas,
    previewSas: getFileUrl.data.value?.data.previewSas,
  }

  filePreview.updateSasUrl(params);
}


// Download the file
const handleDownload = () => {
  triggerDownload(filePreview.downloadSas || '')
}

</script>

<template>
  <Drawer
    v-model:visible="isDrawerVisible"
    position="full"
    class="!bg-white/80 dark:!bg-black/70"
  >
    <template #container="{ closeCallback }">
      <!--   Top bar   -->
      <Toolbar class="!bg-transparent !border-none">
        <template #start>
          <div class="flex items-center gap-2 w-[50%] sm:w-full">
            <Button icon="pi pi-times" class="mr-2" severity="secondary" variant="text" rounded text
                    @click="closeCallback"/>
            <h3 class="font-medium text-nowrap !truncate">{{ filePreview.file?.name }}</h3>
          </div>
        </template>

        <template #center>

        </template>

        <template #end>
          <div class="flex gap-2">
            <Button icon="pi pi-download" class="mr-2" severity="secondary" variant="text" rounded
                    text @click="handleDownload"/>
            <Button icon="pi pi-lock" label="Share"></Button>
          </div>
        </template>
      </Toolbar>


      <div v-if="filePreview.file" class="w-full h-full flex flex-col items-center justify-center">
        <!--   Loading VueQuery Fetch   -->
        <div v-if="getFileUrl.isLoading.value">loading...</div>

        <div v-else-if="!getFileUrl.isLoading.value && filePreview.previewSas"
             class="w-full h-full flex items-center justify-center">
          <!-- VIDEO -->
          <video
            v-if="filePreview.fileType === GENERAL_TYPES['video']"
            controls
            class="max-h-[80%]"
          >
            <source :src="filePreview.previewSas"
                    :type="getHtmlVideoType(filePreview.file?.name)"/>
            Your browser does not support the video tag, or the video could not be loaded.
          </video>

          <!-- IMAGES -->
          <img v-else-if="filePreview.fileType === GENERAL_TYPES['img']"
               :src="filePreview.previewSas"
               alt="file preview"/>

          <!-- PDF -->
          <iframe
            v-else-if="filePreview.fileType === GENERAL_TYPES['pdf']"
            type="application/pdf"
            :src="filePreview.previewSas"
            class="w-full md:w-[80%] h-full"
          />
        </div>
      </div>
    </template>
  </Drawer>
</template>
