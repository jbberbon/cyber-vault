<script setup lang="ts">
import Dialog from "primevue/dialog"
import Button from "primevue/button";
import {computed} from "vue";
import {useCallToast} from "@/lib/hooks/use-call-toast.ts";
import DELETE_FILE_ERR_MAP
  from "@/lib/constants/api-error-messages/file-error-messages.ts/delete-file-error-msg.ts";
import {useLogout} from "@/lib/services/auth/use-logout.ts";
import {useRouter} from "vue-router";

const props = defineProps({
  isOpen: Boolean
})
const router = useRouter()
const logoutToast = useCallToast(DELETE_FILE_ERR_MAP)

// 01. Create a computed property for binding to Dialog's visible prop
const emit = defineEmits();
const modelValue = computed({
  get: () => props.isOpen,
  set: (value) => emit('update:isOpen', value)
});

// 02. Emit an event to close the modal when Cancel or Logout button is clicked
const closeAndUpdate = () => {
  emit('update:isOpen', false);
};

// 03. Initialize Logout hook and onLogout handler
const {mutateAsync, error} = useLogout();
const onLogout = async () => {
  try {
    await mutateAsync()
    closeAndUpdate()
    await router.push('/login')
  } catch {
    logoutToast({
      httpCode: error?.value?.response?.status ?? 500
    })
    closeAndUpdate()
  }
}
</script>

<template>
  <Dialog v-model:visible="modelValue" modal header="Logout confirmation">
    <div class="flex flex-col gap-4 max-w-[280px]">
      <p>Do want to log out your account?</p>
      <div class="flex justify-end gap-2">
        <Button type="button" size="small" outlined label="Cancel" severity="secondary"
                @click="closeAndUpdate"></Button>
        <Button @click="onLogout" size="small" severity="danger" label="Log out"></Button>
      </div>
    </div>
  </Dialog>
</template>
