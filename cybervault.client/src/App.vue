<script setup>
import Toast from "primevue/toast";
import UploadProgressToast from "@/components/upload-progress-toast/UploadProgressToast.vue";
import {useDarkModeStore} from "@/lib/stores/dark-mode-store.js";

/* !!!! Ensures that isDarkMode is set on localstorage on first load
This avoids flicker when refreshing page. Light mode flicker */
useDarkModeStore()
</script>

<template>
  <div class="bg-[var(--p-surface-100)] dark:bg-[var(--p-surface-900)] relative">
    <Toast/>
    <UploadProgressToast/>
    <router-view/>
  </div>
</template>

<style>

</style>
