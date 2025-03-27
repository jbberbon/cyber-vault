<script setup lang="ts">
defineProps({
  title: {
    type: String,
    required: true
  },
  subtitle: {
    type: String,
    required: true
  }
});
</script>

<template>
  <div class="flex flex-col items-center sm:items-start pt-2 gap-1">
    <img src="../../../assets/logo.svg" class="w-[60px]" alt="cyber vault logo">
    <div>
      <h1 class="text-center sm:text-left font-medium">{{ title }}</h1>
      <p class="text-center sm:text-left">{{ subtitle }}</p>
    </div>
  </div>
</template>
