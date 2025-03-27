<script setup lang="ts">
import Button from "primevue/button";
import {PopoverArrow, PopoverContent, PopoverPortal, PopoverRoot, PopoverTrigger} from "radix-vue";
import {ref} from "vue";
import LogoutDialog from "@/components/dialogs/LogoutDialog.vue";

const isPopoverOpen = ref(false);

const isLogoutDialogOpen = ref(false);
</script>

<template>
  <PopoverRoot :open="isPopoverOpen" @update:open="isPopoverOpen = !isPopoverOpen">
    <PopoverTrigger class="bg-transparent border-none p-0">
      <Button icon="pi pi-user" variant="outlined" rounded aria-label="Notification"/>
    </PopoverTrigger>
    <PopoverPortal>
      <PopoverContent align="end" class="bg-white rounded-md shadow-md p-1">
        <div class="flex flex-col items-start">
          <Button label="Profile" icon="pi pi-user" severity="secondary" variant="text"
                  size="small"/>
          <Button label="Logout" icon="pi pi-sign-out" severity="secondary" variant="text"
                  class="!flex !justify-start !w-full"
                  size="small"
                  @click="isPopoverOpen = false; isLogoutDialogOpen = true;"/>
        </div>
        <PopoverArrow class="fill-white"/>
      </PopoverContent>
    </PopoverPortal>
  </PopoverRoot>

  <LogoutDialog v-model:is-open="isLogoutDialogOpen"/>
</template>
