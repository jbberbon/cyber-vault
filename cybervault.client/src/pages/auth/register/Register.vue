<script setup lang="ts">
import {Button, Divider, FloatLabel, InputText, Password} from "primevue";
import AuthLayout from "@/components/layouts/AuthLayout.vue";
import AuthFormTitle from "@/pages/auth/login/AuthFormTitle.vue";
import {useRouter} from "vue-router";
import {useCallToast} from "@/lib/hooks/use-call-toast.ts";
import {useField, useForm} from "vee-validate";
import type {IRegisterUser} from "@/lib/interfaces/user-interface.ts";
import {registerSchema} from "@/lib/schemas/register-schema.ts";
import {useRegister} from "@/lib/services/auth/useRegister.ts";
import REGISTER_ERR from "@/lib/constants/api-error-messages/register-error-messages.ts";


const router = useRouter();
const toast = useCallToast(REGISTER_ERR);
const {handleSubmit, errors} = useForm({
  validationSchema: registerSchema
});
const {value: firstName} = useField<string | null>('firstName');
const {value: lastName} = useField<string | null>('lastName');
const {value: email} = useField<string | null>('email');
const {value: password} = useField<string | null>('password');

const {mutateAsync, isPending, isSuccess, error} = useRegister();
const isPageFreezed = isPending || isSuccess;

const onSubmit = handleSubmit(async (credentials: IRegisterUser) => {
  try {
    await mutateAsync(credentials, {
      onSuccess: () => {
        toast({
          summary: "Account created successfully!",
        })
        router.push("/login");
      }
    });
  } catch {
    toast({
      httpCode: error?.value?.response?.status ?? 500
    })
  }
});


</script>
<template>
  <AuthLayout>
    <AuthFormTitle title="Hi there!" subtitle="Security and convenience, in one place."/>
    <form @submit="onSubmit" class="w-full flex flex-col gap-4">
      <div class="w-full">
        <FloatLabel variant="on">
          <InputText fluid name="firstName" v-model="firstName" type="text"/>
          <label for="firstName">First name</label>
        </FloatLabel>
        <span class="text-sm text-red-400">{{ errors.firstName }}</span>
      </div>

      <div class="w-full">
        <FloatLabel variant="on">
          <InputText fluid name="lastName" v-model="lastName" type="text"/>
          <label for="lastName">Last name</label>
        </FloatLabel>
        <span class="text-sm text-red-400">{{ errors.lastName }}</span>
      </div>

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
      </div>

      <div class="pt-4 w-full flex flex-col gap-2">
        <Button type="submit" fluid :disabled="isPageFreezed">Sign up</Button>
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
          Sign up with Google
        </Button>
      </div>
    </form>
    <div class="flex gap-2">
      <p>Already have an account?</p>
      <a class="text-[var(--p-primary-500)] underline cursor-pointer" href="/login">Sign in</a>
    </div>
  </AuthLayout>
</template>
