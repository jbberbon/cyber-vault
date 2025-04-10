<script setup lang="ts">
import type {IDirectory} from "@/lib/interfaces/file-interface.ts";
import {computed} from "vue";
import NewItemsDropdown from "@/components/new-items-dropdown/NewItemsDropdown.vue";
import {useRouter} from "vue-router";
import BreadcrumbListDropdown
  from "@/pages/protected/my-vault/components/custom-breadcrumb/BreadcrumbListDropdown.vue";
import {navigateBreadcrumb} from "@/pages/protected/my-vault/components/custom-breadcrumb/util.ts";

const props = defineProps<{
  navItems: IDirectory[];
}>();

// Computed property that adds a static item to the beginning of the navItems
const tempNavItems = computed(() => {
  return [
    {
      name: "My Vault",
      serverAssignedId: ""
    },
    ...props.navItems
  ]
});

// Computed property that gets the last two items from tempNavItems
const last2NavItems = computed(() => tempNavItems.value.slice(-2));

// Final computed property that either returns the full array or just the last two items
const navItems = computed(() => {
  return tempNavItems.value.length > 3 ? last2NavItems.value : tempNavItems.value;
});

const router = useRouter()

const hoverStyle = "hover:bg-[var(--p-surface-100)] dark:hover:bg-[var(--p-surface-800)] p-1 rounded-md cursor-pointer"
const textStyle = "font-medium text-2xl text-[var(--p-text-color)] max-w-[150px] sm:max-w-[200px] truncate !text-nowrap"
const chevronRightStyle = "pi pi-chevron-right text-[var(--p-surface-500)] !text-xs"
const sortDownStyle = "pi pi-sort-down-fill !text-xs text-[var(--p-text-color)]"
</script>

<template>
  <!-- Mobile -->
  <div class="flex sm:hidden w-full" :class="hoverStyle">
    <NewItemsDropdown>
      <div class="flex items-center gap-2">
        <h1 :class="textStyle">{{ navItems[navItems.length - 1].name }}</h1>
        <i :class="sortDownStyle"></i>
      </div>
    </NewItemsDropdown>
  </div>

  <!-- NON-Mobile Limit to 3 current breadcrumbs at a time -->
  <div class="hidden sm:flex w-full gap-4 items-center">
    <!--  CONDITIONAL: Dropdown  -->
    <BreadcrumbListDropdown v-if="tempNavItems.length > 3" :items="tempNavItems.slice(0, -2)">
      <div class="flex items-center gap-4" :class="hoverStyle">
        <i class="pi pi-ellipsis-h !text-xl text-[var(--p-text-color)]"></i>
        <i :class="chevronRightStyle"></i>
      </div>
    </BreadcrumbListDropdown>

    <!--  Breadcrumbs  -->
    <div
      v-for="(item, index) in navItems"
      :key="index"
    >
      <!--   All elements except last   -->
      <div
        v-if="index < navItems.length - 1"
        class="flex items-center gap-4"
        :class="hoverStyle"
        @click="navigateBreadcrumb(router, item.serverAssignedId)"
      >
        <h1 :class="textStyle">{{ item.name }}</h1>
        <i :class="chevronRightStyle"></i>
      </div>

      <!--   Last element   -->
      <div v-else :class="hoverStyle">
        <NewItemsDropdown>
          <div class="flex items-center gap-4">
            <h1 :class="textStyle">{{ item.name }}</h1>
            <i :class="sortDownStyle"></i>
          </div>
        </NewItemsDropdown>
      </div>
    </div>
  </div>
</template>
