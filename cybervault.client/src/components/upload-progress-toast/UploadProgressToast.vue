<script setup lang="ts">
import Button from "primevue/button";
import {useUploadStore} from "@/lib/stores/upload-progress-store.ts";
import {ref, watchEffect} from "vue";
import CircularProgressBar from "@/components/CircularProgressBar.vue";
import ProgressSpinner from 'primevue/progressspinner';

const uploadStore = useUploadStore()
const isExpanded = ref<boolean>(true)

</script>

<template>
  <div class="fixed bottom-0 sm:right-4 right-0 w-[350px] shadow-md rounded-sm z-9999"
       v-if="uploadStore.uploads.length > 0">
    <div class="bg-[var(--p-surface-100)] dark:bg-[var(--p-surface-900)] rounded-t-sm p-3 flex items-center justify-between">
      <p v-if="uploadStore.pendingCount > 0">Uploading {{ uploadStore.pendingCount }} {{ uploadStore.pendingCount > 1 ? "items" : "item" }}</p>
      <p v-else>Uploaded all items</p>
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
    <div class="dark:bg-[var(--p-surface-800)] bg-white p-3" v-if="isExpanded">
      <div v-for="(upload, index) in uploadStore.uploads" :key="index"
           class="flex gap-2 items-center justify-between">
        <p class="max-w-[280px] !truncate">{{ upload.fileName }}</p>
        <div class="w-[25px] h-[25px]">
          <!-- Uploading to backend server: progress bar -->
          <CircularProgressBar :progress="upload.progress"/>

          <!-- Done but awaiting processing of Backend: Indefinite Spinner -->
          <!--          <ProgressSpinner class="!w-[20px] !h-[20px]" v-if="upload.progress === 100 && upload.isDoneButAwaiting"/>-->
<!--          <CircularProgressBar v-if="upload.progress === 100 && upload.isDoneButAwaiting"
                               :progress="upload.progress" :is-spinning="true"/>-->

          <!-- Done: Check Icon -->
<!--          <i class="pi pi-check text-green-600"
             v-if="upload.progress === 100 && !upload.isDoneButAwaiting"></i>-->

        </div>
      </div>
    </div>
  </div>
</template>
