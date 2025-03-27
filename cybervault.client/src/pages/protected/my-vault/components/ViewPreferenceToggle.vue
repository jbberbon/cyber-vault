<script setup lang="ts">
import SelectButton from "primevue/selectbutton";

const viewOptions = [
  {
    icon: "pi pi-list",
    value: "list"
  },
  {
    icon: "pi pi-th-large",
    value: "grid"
  }
];

const props = defineProps<{
  chosenView: string;
}>();

const emit = defineEmits<{
  (event: 'update:chosenView', value: string): void;
}>();

</script>

<template>
  <!--  List / Grid View Toggle        -->
  <SelectButton
    class="select-view"
    v-model="props.chosenView"
    :options="viewOptions"
    optionLabel="value"
    optionValue="value"
    aria-labelledby="custom"
    @change="emit('update:chosenView', $event.value)"
  >
    <template #option="slotProps">
      <i :class="slotProps.option.icon"></i>
    </template>
  </SelectButton>
</template>

<style>
.dark .select-view .p-togglebutton {
  background: var(--p-surface-800) !important;
  border-color: var(--p-surface-800) !important;
}
</style>
