<script setup lang="ts">
import Toolbar from 'primevue/toolbar';
import Button from 'primevue/button';
import Sidebar from '@/components/layouts/sidebar/Sidebar.vue';
import {useSidebarStore} from "@/lib/stores/sidebar-store.ts";
import {computed} from "vue";
import {useRouter} from "vue-router";

const router = useRouter();
const sidebar = useSidebarStore();

const mainContentClass = computed(() => sidebar.isOpen ? '!ml-[280px] w-[calc(100%-280px)]' : '!ml-0 w-full')
const toggleBtnPosition = computed(() => sidebar.isOpen ? "top-22 left-69" : "top-22 left-0");
</script>


<template>
  <Toolbar class="!bg-transparent !border-none !rounded-none">
    <template #start>
      <div class="flex items-center cursor-pointer" @click="router.push('/')">
        <img src="../../assets/logo.svg" alt="cyber vault logo" class="w-[48px]"/>
        <h1 class="text-lg cyber-vault-logo-txt hidden">Cyber Vault</h1>
      </div>
    </template>

    <template #end>
      <div class="sm:hidden" @click="sidebar.toggle">
        <Button
          icon="pi pi-bars"
          severity="secondary"
          variant="text"
          rounded
          aria-label="Sidebar toggle"
          size="large"
        />
      </div>

      <div class="hidden sm:flex items-center gap-1">
        <Button icon="pi pi-cog" severity="secondary" variant="text" rounded
                aria-label="Settings" size="large"/>
        <Button icon="pi pi-user" variant="outlined" rounded aria-label="Notification" />
      </div>
    </template>
  </Toolbar>

  <Sidebar :isOpen="sidebar.isOpen"/>
  <main
    class="flex-1 p-2 transition-all duration-300 ease-in-out"
    :class="mainContentClass"
  >
    <div
      class="w-full bg-white p-2 pt-4 sm:p-4 sm:pt-6 rounded-md h-[89vh] dark:bg-[var(--p-surface-950)] overflow-scroll">
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
</template>

<style scoped>

@media screen and (min-width: 600px) {
  .cyber-vault-logo-txt {
    display: inline-block;
  }
}
</style>
