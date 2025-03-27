<script setup lang="ts">
import MainLayout from "@/components/layouts/MainLayout.vue";
import IconField from 'primevue/iconfield';
import InputIcon from 'primevue/inputicon';
import InputText from 'primevue/inputtext';

</script>

<template>
  <MainLayout>
    <!--  Home Header  -->
    <section class="w-full flex justify-center">
      <IconField>
        <InputIcon>
          <i class="pi pi-search"/>
        </InputIcon>
        <InputText placeholder="Search in Vault" fluid class="max-w-[1000px]"/>
      </IconField>
    </section>
  </MainLayout>
</template>
