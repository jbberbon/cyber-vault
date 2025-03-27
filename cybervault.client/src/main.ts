import './assets/main.css'
import {createApp} from 'vue'
import App from './App.vue';
import PrimeVue from 'primevue/config';
import Aura from '@primeuix/themes/aura';
import {definePreset} from '@primeuix/themes';
import {createRouter, createWebHistory} from "vue-router";
import {VueQueryPlugin} from '@tanstack/vue-query';
import ToastService from 'primevue/toastservice';
import {Toast} from "primevue";
import {createPinia} from 'pinia'
import {SIDEBAR_ROUTES} from "@/lib/constants/sidebar-routes.ts";

const CustomStyling = definePreset(Aura, {
  semantic: {
    primary: {
      50: '#F7F7FD',
      100: '#D7D8F6',
      200: '#B7B8EF',
      300: '#9799E7',
      400: '#777AE0',
      500: '#575bd9',
      600: '#4A4DB8',
      700: '#3D4098',
      800: '#303277',
      900: '#232457',
      950: '#161736'
    },
    surface: {
      1000: '#101010'
    }

    /*colorScheme: {
      dark: {

      }
    }*/
  }

});

const app = createApp(App);

// Use PrimeVue plugin
app.use(PrimeVue, {
  theme: {
    preset: CustomStyling,
    options: {
      prefix: 'p',
      darkModeSelector: '.dark',
      cssLayer: false
    }
  }
});

// Configure routes
const router = createRouter({
  routes: SIDEBAR_ROUTES,
  history: createWebHistory()
});
app.use(router);

app.use(VueQueryPlugin);

// Toast
app.use(ToastService);
app.component('Toast', Toast);

// Pinia
const pinia = createPinia()
app.use(pinia)

// Mount the app
app.mount('#app');
