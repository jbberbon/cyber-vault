<script setup lang="ts">
import type {IDirectoryPath} from "@/lib/interfaces/file-interface.ts";
import {computed} from "vue";
import NewItemsDropdown from "@/components/new-items-dropdown/NewItemsDropdown.vue";
import {useRouter} from "vue-router";

const props = defineProps<{
  navItems: IDirectoryPath[];
}>();

const navItems = computed(() => {
  return [
    {
      name: "My Vault",
      serverAssignedId: ""
    },
    ...props.navItems
  ]
});
const last2NavItems = computed(() => navItems.value.slice(-2));

const router = useRouter();

const navigateBreadcrumb = (path?: string) => {
  if (!path) {
    router.push('/my-vault');
  } else {
    router.push({name: "my-vault-subdirectory", params: {id: path}});
  }
};


const hoverStyle = "hover:bg-[var(--p-surface-100)] dark:hover:bg-[var(--p-surface-800)] p-1 rounded-md cursor-pointer"
const textStyle = "font-medium text-2xl text-[var(--p-text-color)] max-w-[200px] truncate !text-nowrap"
const chevronRightStyle = "pi pi-chevron-right text-[var(--p-surface-500)] !text-xs"
const sortDownStyle = "pi pi-sort-down-fill !text-xs text-[var(--p-text-color)]"
</script>

<template>
  <!-- Limit to 3 current breadcrumbs at a time, Static Root INCLUDED -->
  <div v-if="navItems.length <= 3" class="w-full flex gap-4">
    <!--  Static Root Breadcrumb  -->
    <div :class="hoverStyle" @click="navigateBreadcrumb()">
      <div v-if="navItems.length > 0" class="flex items-center gap-4">
        <h1 :class="textStyle">My Vault</h1>
        <i :class="chevronRightStyle"></i>
      </div>

      <NewItemsDropdown v-else>
        <div class="flex items-center gap-4">
          <h1 :class="textStyle">My Vault</h1>
          <i :class="sortDownStyle"></i>
        </div>
      </NewItemsDropdown>
    </div>

    <!--  Fetched folder structure  -->
    <div v-for="(item, index) in navItems" :key="index">
      <!--   All elements except last   -->
      <div
        v-if="index < navItems.length - 1"
        class="flex items-center gap-4"
        :class="hoverStyle"
        @click="navigateBreadcrumb(item.serverAssignedId)"
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

  <!-- If > 2, then put the last 2 on the breadcrumb and the rest, on the dropdown list of folders -->
  <div v-else class="w-full flex gap-4">
    <div :class="hoverStyle" class="flex items-center gap-4">
      <i class="pi pi-ellipsis-h !text-xl text-[var(--p-text-color)]"></i>
      <i :class="chevronRightStyle"></i>
    </div>

    <div v-for="(item, index) in last2NavItems" :key="index">
      <!--   All elements except last   -->
      <div
        v-if="index < last2NavItems.length - 1"
        class="flex items-center gap-4"
        :class="hoverStyle"
        @click="navigateBreadcrumb(item.serverAssignedId)"
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
