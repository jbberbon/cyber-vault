<script setup>
import Toast from "primevue/toast";
import UploadProgressToast from "@/components/upload-progress-toast/UploadProgressToast.vue";
import {ref} from "vue";

const isVisible = ref(true)
</script>

<template>
  <div class="bg-[var(--p-surface-100)] dark:bg-[var(--p-surface-900)] relative">
    <Toast/>
    <UploadProgressToast />
    <router-view/>
  </div>
</template>

<style>

</style>
