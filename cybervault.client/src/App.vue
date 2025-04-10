<script setup>
import Toast from "primevue/toast";
import FileProgressToast from "@/components/file-progress-toast/FileProgressToast.vue";
import {useDarkModeStore} from "@/lib/stores/dark-mode-store.js";
import {useFilePreviewStore} from "@/lib/stores/file-preview-store.js";
import {defineAsyncComponent} from 'vue'
import {useFileProgressStore} from "@/lib/stores/file-progress-store.js";

const FilePreviewModal = defineAsyncComponent({
  loader: () => import('@/components/dialogs/FileViewerModal.vue'),
  delay: 200,
  timeout: 3000,
});

/* !!!! Ensures that isDarkMode is set on localstorage on first load
This avoids flicker when refreshing page. Light mode flicker */
useDarkModeStore()

const filePreview = useFilePreviewStore()
const fileProgressStore = useFileProgressStore()
</script>

<template>
  <div class="bg-[var(--p-surface-100)] dark:bg-[var(--p-surface-900)] relative">
    <Toast/>
    <Suspense>
      <template #default>
        <FileProgressToast v-if="fileProgressStore.files.length > 0"/>
      </template>
      <template #fallback>
        <!-- You can display a loading spinner or some placeholder while loading -->
        <div>Loading...</div>
      </template>
    </Suspense>

    <Suspense>
      <template #default>
        <FilePreviewModal v-if="filePreview.file"/>
      </template>
      <template #fallback>
        <!-- You can display a loading spinner or some placeholder while loading -->
        <div>Loading...</div>
      </template>
    </Suspense>
    <router-view/>
  </div>
</template>

<style>

</style>
