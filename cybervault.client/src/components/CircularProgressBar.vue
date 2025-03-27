<template>
  <div class="progress-container">
    <!-- Adjusted viewBox to include stroke width -->
    <svg width="100%" height="100%" viewBox="0 0 120 120" preserveAspectRatio="xMidYMid meet">
      <!-- Moved circles to center of expanded viewBox -->
      <circle cx="60" cy="60" r="45" stroke="#e6e6e6" stroke-width="15" fill="none"/>

      <circle
        cx="60"
        cy="60"
        r="45"
        stroke="#4caf50"
        stroke-width="15"
        fill="none"
        :stroke-dasharray="circumference"
        :stroke-dashoffset="circumference - progressOffset"
        style="transition: stroke-dashoffset 0.35s"
      />
    </svg>

    <div v-if="showCompletionText" class="completion-text flex items-center"><i
      class="pi pi-check text-green-600 !text-xs"></i></div>
  </div>
</template>

<script setup lang="ts">
import {defineProps, computed} from 'vue';

const props = defineProps({
  progress: {
    type: Number,
    required: false,
    default: 0,
  },
  isFinished: {
    type: Boolean,
    required: false,
    default: false,
  }
});

const circumference = 2 * Math.PI * 45; // Circumference of the circle (r = 45)
const progressOffset = computed(() => (props.progress / 100) * circumference);

// Show YAY when progress is 100% or isFinished is true
const showCompletionText = computed(() => {
  return props.progress >= 100 || props.isFinished;
});
</script>

<style scoped>
.progress-container {
  display: flex;
  justify-content: center;
  align-items: center;
  position: relative;
  width: 100%;
  height: 100%;
}

svg {
  display: block; /* Removes any extra space */
  overflow: visible; /* Ensures strokes aren't clipped */
}

/* Styles for the completion text */
.completion-text {
  position: absolute;
  font-weight: bold;
  font-size: 14px;
  color: #4caf50;
}
</style>
