<script setup lang="ts">
import Button from "primevue/button";
import {useUploadStore} from "@/lib/stores/upload-progress-store.ts";
import {ref} from "vue";
import CircularProgressBar from "@/components/CircularProgressBar.vue";

const uploadStore = useUploadStore()
const isExpanded = ref<boolean>(true)


</script>

<template>
  <div class="fixed bottom-0 sm:right-4 right-0 w-[350px] shadow-md rounded-sm"
       v-if="uploadStore.uploads.length > 0">
    <div class="bg-[var(--p-surface-100)] rounded-t-sm p-3 flex items-center justify-between">
      <p>Uploading 1 item</p>
      <div>
        <Button
          :icon="isExpanded ? 'pi pi-chevron-down' : 'pi pi-chevron-up'"
          rounded size="small"
          text severity="contrast"
          @click="isExpanded = !isExpanded"
        />
        <Button icon="pi pi-times" rounded size="small" text severity="contrast"
                @click="uploadStore.reset"/>
      </div>
    </div>
    <div class="bg-white p-3" v-if="isExpanded">
      <div v-for="(upload, index) in uploadStore.uploads" :key="index"
           class="flex gap-2 items-center justify-between">
        <p>{{ upload.fileName }}</p>
        <div class="w-[20px]">
          <i class="pi pi-check text-green-600" v-if="upload.progress === 100"></i>
          <CircularProgressBar v-else :progress="upload.progress"/>
        </div>
      </div>

    </div>
  </div>
</template>
