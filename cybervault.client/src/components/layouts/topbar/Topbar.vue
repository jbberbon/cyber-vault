<script setup lang="ts">
import {useRouter} from "vue-router";
import {useSidebarStore} from "@/lib/stores/sidebar-store.ts";
import ProfileIconPopover from "@/components/layouts/topbar/ProfileIconPopover.vue";
import Toolbar from "primevue/toolbar";
import Button from "primevue/button"
import {useDarkModeStore} from "@/lib/stores/dark-mode-store.ts";

const router = useRouter();
const {toggle: toggleSidebar} = useSidebarStore();
const darkModeStore = useDarkModeStore();

</script>

<template>
  <Toolbar class="my-toolbar !bg-transparent !border-none !rounded-none">
    <template #start>
      <div class="flex items-center cursor-pointer" @click="router.push('/')">
        <img src="../../../assets/logo.svg" alt="cyber vault logo" class="w-[48px]"/>
        <h1 class="text-lg cyber-vault-logo-txt hidden">Cyber Vault</h1>
      </div>
    </template>

    <template #end>
      <div class="sm:hidden" @click="toggleSidebar">
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
        <Button
          :icon="darkModeStore.isDarkMode ? 'pi pi-sun' : 'pi pi-moon'"
          severity="secondary"
          variant="text" rounded
          aria-label="Dark Mode Toggle"
          size="large"
          @click="darkModeStore.toggleDarkMode"
        />
        <ProfileIconPopover/>
      </div>
    </template>
  </Toolbar>
</template>
<style>

@media screen and (min-width: 600px) {
  .cyber-vault-logo-txt {
    display: inline-block;
  }

  .my-toolbar .p-toolbar {
    min-height: 1000px !important;
  }
}
</style>

