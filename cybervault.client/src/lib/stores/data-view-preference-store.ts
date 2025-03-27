import {defineStore} from 'pinia'
import {useLocalStorage} from '@vueuse/core'

export const useDataViewPreferenceStore = defineStore('dataViewPreference', () => {
  const viewPreference = useLocalStorage('dataViewPreference', 'grid');

  const setViewPreference = (preference: string) => {
    viewPreference.value = preference;
  }


  return {viewPreference, setViewPreference}
})
