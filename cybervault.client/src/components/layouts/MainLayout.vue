<script setup lang="ts">
import Button from 'primevue/button';
import Sidebar from '@/components/layouts/sidebar/Sidebar.vue';
import {useSidebarStore} from "@/lib/stores/sidebar-store.ts";
import {computed} from "vue";
import Topbar from "@/components/layouts/topbar/Topbar.vue";

const sidebar = useSidebarStore();

const mainContentClass = computed(() => sidebar.isOpen ? '!ml-[280px] w-[calc(100%-280px)]' : '!ml-0 w-full')
const toggleBtnPosition = computed(() => sidebar.isOpen ? "top-22 left-69" : "top-22 left-0");
</script>


<template>
  <section class="h-[100vh]">
    <Topbar/>
    <Sidebar :isOpen="sidebar.isOpen"/>
    <main
        class="flex-1 p-2 transition-all duration-300 ease-in-out h-[calc(100vh-72px)]"
        :class="mainContentClass"
    >
      <div
          class="w-full bg-white p-2 pt-4 sm:p-4 sm:pt-6 rounded-md dark:bg-[var(--p-surface-950)] overflow-y-scroll h-[100%]">
        <div class="hidden sm:block">
          <Button
              severity="secondary"
              class="!w-7 !h-7 !fixed top-4 z-[9999] !transition-all !duration-300 !ease-in-out"
              :class="toggleBtnPosition"
              :icon="sidebar.isOpen ? 'pi pi-angle-left' : 'pi pi-angle-right'"
              raised
              rounded
              aria-label="Toggle Sidebar"
              @click="sidebar.toggle"
          />
        </div>
        <slot><!-- Children components here! --></slot>
      </div>
    </main>
  </section>
</template>
