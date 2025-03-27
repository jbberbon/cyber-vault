import './assets/main.css'
import {createApp} from 'vue'
import App from './App.vue';
import PrimeVue from 'primevue/config';
import {VueQueryPlugin} from '@tanstack/vue-query';
import ToastService from 'primevue/toastservice';
import {Toast} from "primevue";
import {createPinia} from 'pinia';
import router from "@/lib/configs/router-config.ts";
import customStyle from "@/lib/constants/primevue-custom-style.ts";

const app = createApp(App);

// Use PrimeVue plugin
app.use(PrimeVue, {
  theme: {
    preset: customStyle,
    options: {
      prefix: 'p',
      darkModeSelector: '.dark',
      cssLayer: false
    }
  }
});
// Pinia
const pinia = createPinia()
app.use(pinia)

// Vue Router
app.use(router);

// Vue Query
app.use(VueQueryPlugin);

// Toaster
app.use(ToastService);
app.component('Toast', Toast);

// Mount the app
app.mount('#app');
