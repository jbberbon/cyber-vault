<script setup lang="ts">
import {useRouter, useRoute} from "vue-router";
import {SIDEBAR_CONTENTS} from "@/lib/constants/sidebar-contents.ts";
import {computed} from "vue";
import NewItemsDropdown from "@/components/new-items-dropdown/NewItemsDropdown.vue";

const router = useRouter();
const route = useRoute();

const onNavigate = (path: string) => {
  router.push(path);
};

const props = defineProps({
  isOpen: {type: Boolean}
});

const sidebarClass = computed(() => props.isOpen ? "w-[280px]" : "w-0 opacity-0");


</script>

<template>
  <aside
    class="h-[90vh] transition-all duration-300 ease-in-out fixed"
    :class="sidebarClass">
    <div class="w-full p-4">
      <NewItemsDropdown>
        <div
          class="rounded-lg bg-white dark:bg-[var(--p-surface-800)] shadow-[var(--p-button-raised-shadow)] hover:bg-[var(--p-primary-300)]">
          <div class="p-4 flex items-center gap-2">
            <i class="pi pi-plus"/>
            <h3 class="font-normal text-sm">New</h3>
          </div>
        </div>
      </NewItemsDropdown>

      <!--   Sidebar Menus   -->
      <ul class="p-0 pt-4 list-none flex flex-col">
        <li
          v-for="(item, index) in SIDEBAR_CONTENTS.user"
          :key="index"
          class="p-2 cursor-pointer hover:bg-[var(--p-surface-200)] rounded-md"
          :class="{ 'active': route.path === item.path }"
        >
          <div @click="onNavigate(item.path)" class="flex gap-4">
            <i :class="item.icon" class="!flex !items-center !justify-center !text-xl"></i>
            <span class="flex items-center text-nowrap">{{ item.title }}</span>
          </div>
        </li>
      </ul>
    </div>
  </aside>
</template>

<style scoped>
.active {
  background-color: var(--p-primary-500);
  color: white;
}
</style>
