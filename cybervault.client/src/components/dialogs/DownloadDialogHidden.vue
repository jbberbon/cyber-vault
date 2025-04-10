<script setup lang="ts">

import {useGetFileUrl} from "@/lib/services/files/use-get-file-url.ts";
import {watchEffect} from "vue";
import {useRouter} from "vue-router";
import {useCallToast} from "@/lib/hooks/use-call-toast.ts";

const props = defineProps({
  name: {
    type: String,
    required: true
  },
  parentDirectoryId: String,
  isOpen: Boolean
})

const emit = defineEmits();

const router = useRouter()
const toast = useCallToast()
const {data, refetch, error} = useGetFileUrl({
  fileName: props.name,
  parentDirectoryId: props.parentDirectoryId
});

const closeAndUpdate = () => {
  emit('update:isOpen', false);
};

const handleDownloadFile = async () => {
  await refetch()

  if (!error.value && data.value) {
    // Create a-tag and append sasUrl
    const a = document.createElement('a');
    a.href = data.value.data?.sasUrl

    // Append the anchor element to the document body (it doesn't need to be visible)
    document.body.appendChild(a);

    // Download file by simulating a click event
    a.click();

    // Remove the anchor element from the document after the download starts
    document.body.removeChild(a);
  }
  closeAndUpdate()
}

// 05. Manually download if Dialog is open and manage fetch errors
watchEffect(async () => {
  if (props.isOpen) {
    await handleDownloadFile()
  }

  if (error.value) {
    const errCode = error.value?.response?.status
    // Display toast error
    if (errCode != 404 && errCode != 400) {
      toast({
        httpCode: error.value?.response?.status ?? 500
      })
    }

    // Redirect to error pages
    if (errCode === 404) {
      await router.push({path: '/not-found', replace: true})
    }

    if (errCode === 400) {
      await router.push({path: '/bad-request', replace: true})
    }
  }
});

</script>

<template>
  <div class="you-should-not-see-this hidden"></div>
</template>
