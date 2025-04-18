﻿import {defineStore} from 'pinia';
import type {IUser} from "@/lib/interfaces/user-interface.ts"
import {useLocalStorage} from '@vueuse/core'

export const useUserStore = defineStore('auth', () => {
  const user = useLocalStorage('user',{
    firstName: "",
    lastName: "",
    email: "",
  });

  const setUser = (userParam: IUser) => {
    if (userParam.firstName) {
      user.value.firstName = userParam.firstName;
    }
    if (userParam.lastName) {
      user.value.lastName = userParam.lastName;
    }
    if (userParam.email) {
      user.value.email = userParam.email;
    }
  };

  return {user, setUser};
});
