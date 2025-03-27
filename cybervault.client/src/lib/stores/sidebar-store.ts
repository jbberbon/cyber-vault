import {defineStore} from 'pinia'
import {useLocalStorage} from '@vueuse/core'

export const useSidebarStore = defineStore('sidebar', () => {
  const isOpen = useLocalStorage('isSidebarOpen', true);

  function toggle() {
    isOpen.value = !isOpen.value;
    console.log(isOpen.value)
  }

  return {isOpen, toggle}
})
