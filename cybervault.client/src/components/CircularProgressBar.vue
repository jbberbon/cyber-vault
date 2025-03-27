<template>
  <div class="progress-container">
    <svg width="25" height="25" viewBox="0 0 100 100">
      <!-- Background circle (gray) -->
      <circle cx="50" cy="50" r="45" stroke="#e6e6e6" stroke-width="5" fill="none" />

      <!-- Foreground circle (progress) -->
      <circle
        cx="50"
        cy="50"
        r="45"
        stroke="#4caf50"
        stroke-width="10"
        fill="none"
        :stroke-dasharray="circumference"
        :stroke-dashoffset="circumference - progressOffset"
        style="transition: stroke-dashoffset 0.35s"
      />
    </svg>
  </div>
</template>

<script setup lang="ts">
import { defineProps, computed } from 'vue';

const props = defineProps({
  progress: {
    type: Number,
    required: true,
    default: 0,
  },
});

const circumference = 2 * Math.PI * 45; // Circumference of the circle (r = 45)
const progressOffset = computed(() => (props.progress / 100) * circumference);
</script>

<style scoped>
.progress-container {
  display: flex;
  justify-content: center;
  align-items: center;
  position: relative;
}

.progress-text {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  font-weight: bold;
  font-size: 18px;
}
</style>
