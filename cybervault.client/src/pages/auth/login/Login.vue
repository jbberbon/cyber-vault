﻿<script setup lang="ts">
import {InputText, FloatLabel, Password, Button, Divider} from "primevue";
import {useField, useForm} from 'vee-validate';
import {loginSchema} from "@/lib/schemas/login-schema.ts";
import AuthLayout from "@/components/layouts/AuthLayout.vue";
import AuthFormTitle from "@/pages/auth/login/AuthFormTitle.vue";
import {useRouter} from 'vue-router';
import {useLogin} from "@/lib/services/auth/use-login.ts";
import type {ILoginUser} from "@/lib/interfaces/user-interface.ts";
import LOGIN_ERR from "@/lib/constants/api-error-messages/login-error-messages.ts";
import {useUserStore} from "@/lib/stores/user-store.ts";

const router = useRouter();
const {handleSubmit, errors, setFieldError} = useForm({
  validationSchema: loginSchema
});
const {value: email} = useField<string | null>('email');
const {value: password} = useField<string | null>('password');
const login = useLogin();
const isPageFrozen = login.isPending || login.isSuccess;

const userStore = useUserStore();

const onSubmit = handleSubmit(async (credentials: ILoginUser) => {
  try {
    await login.mutateAsync(credentials, {
      onSuccess: (data) => {
        if (data) {
          userStore.setUser(data.data)
        }
        router.push("/home");
      }
    });
  } catch {
    // Get appropriate error message from the backend response or fallback to default
    const errorMessage = login.error?.value?.response?.status ?
      LOGIN_ERR[login.error?.value?.response?.status] :
      LOGIN_ERR[500];

    // Set the error on the password field
    setFieldError("password", errorMessage);
  }
});

</script>

<template>
  <AuthLayout>
    <AuthFormTitle title="Welcome back!" subtitle="Security and convenience, in one place."/>
    <form @submit="onSubmit" class="w-full flex flex-col gap-4">
      <div class="w-full">
        <FloatLabel variant="on">
          <InputText fluid name="email" v-model="email" type="email"/>
          <label for="email">Email</label>
        </FloatLabel>
        <span class="text-sm text-red-400">{{ errors.email }}</span>
      </div>

      <div class="w-full flex flex-col gap-2">
        <div class="w-full">
          <FloatLabel variant="on">
            <Password fluid name="password" v-model="password" toggleMask :feedback="false"/>
            <label for="password">Password</label>
          </FloatLabel>
          <span class="text-sm text-red-400">{{ errors.password }}</span>
        </div>
        <router-link to=""
                     class="text-gray-900 dark:text-gray-100 hover:text-[var(--p-primary-500)] font-mono text-right">
          Forgot password?
        </router-link>
      </div>

      <div class="pt-4 w-full flex flex-col gap-2">
        <Button type="submit" fluid :disabled="isPageFrozen">Sign in</Button>
        <div class="flex flex-row gap-2">
          <Divider/>
          <p class="flex items-center">or</p>
          <Divider/>
        </div>
        <Button
          class="!bg-[var(--p-surface-200)] !border-none !outline-none !text-[var(--p-surface-900)]"
          fluid
        >
          <img src="../../../assets/google.svg" alt="google logo" class="w-[20px]">
          Sign in with Google
        </Button>
      </div>
    </form>
    <div class="flex gap-2">
      <p>Don't have an account?</p>
      <a class="text-[var(--p-primary-500)] underline cursor-pointer" href="/register">Sign up</a>
    </div>
  </AuthLayout>
</template>
