import {defineStore} from 'pinia';
import {useLocalStorage} from '@vueuse/core'

export const useDarkModeStore = defineStore('darkMode', () => {
  // 01. Get <html> element
  let htmlElement = document.documentElement;
  let classes = htmlElement.classList;

  // 02. Set initial value to localStorage
  const isDarkMode = useLocalStorage("isDarkMode", false)

  // 03. Toggle fn
  const toggleDarkMode = () => {
    isDarkMode.value = !isDarkMode.value

    if (isDarkMode.value) {
      classes.add('dark')
    } else {
      classes.remove('dark')
    }
  }

  // 04. Apply initial theme based on local storage value
  if (isDarkMode.value) {
    htmlElement.classList.add('dark');
  } else {
    htmlElement.classList.remove('dark');
  }

  return {isDarkMode, toggleDarkMode};
});
