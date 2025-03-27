<template>
  <div class="w-full h-[100vh] flex items-center justify-center !dark:bg-[var(--p-surface-100)]">
    <div class="w-full p-2 sm:p-6 sm:max-w-[500px]
    dark:sm:bg-[var(--p-surface-950)] sm:bg-white rounded-lg flex flex-col gap-6">
      <slot>
        <!-- children components -->
      </slot>
    </div>
  </div>
</template>
