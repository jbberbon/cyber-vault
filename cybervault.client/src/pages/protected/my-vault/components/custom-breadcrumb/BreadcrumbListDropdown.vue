<script setup lang="ts">
import {ref} from "vue";
import TieredMenu from 'primevue/tieredmenu';
import {useRouter} from "vue-router";
import type {IDirectory} from "@/lib/interfaces/file-interface.ts";
import {navigateBreadcrumb} from "@/pages/protected/my-vault/components/custom-breadcrumb/util.ts";

const props = defineProps<{
  items: IDirectory[];
}>()
const router = useRouter();
const newButtonPopout = ref();
const toggleNewButton = (event: MouseEvent) => {
  newButtonPopout.value.toggle(event);
};

const items = ref([
  ...props.items.map((item) => ({
    label: item.name,
    icon: "pi pi-folder",
    command: () => navigateBreadcrumb(router, item.serverAssignedId)

  }))
]);

</script>
<template>
  <div @click="toggleNewButton" aria-controls="overlay_newButtonMenu">
    <!-- This is where the button trigger goes in -->
    <slot></slot>
  </div>
  <TieredMenu ref="newButtonPopout" id="overlay_newButtonMenu" :model="items" popup/>
</template>
