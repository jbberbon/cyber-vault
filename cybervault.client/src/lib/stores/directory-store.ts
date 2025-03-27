import {defineStore} from 'pinia'
import {ref} from "vue";

export const useDirectoryStore = defineStore('directory', () => {
  const isOpen = ref(true);

  function toggle() {
    isOpen.value = !isOpen.value;
    console.log(isOpen.value)
  }

  return {isOpen, toggle}
})
