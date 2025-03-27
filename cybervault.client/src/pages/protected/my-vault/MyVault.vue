<script setup lang="ts">
import MainLayout from "@/components/layouts/MainLayout.vue";
import {useListFiles} from "@/lib/services/files/use-file.ts";
import {computed, watchEffect} from "vue";
import ListFolder from "@/pages/protected/my-vault/components/folder/ListFolder.vue";
import ListFile from "@/pages/protected/my-vault/components/file/ListFile.vue";
import SelectButton from 'primevue/selectbutton';
import {ref} from 'vue';
import {useCallToast} from "@/lib/hooks/use-call-toast.ts";
import {useRouter} from "vue-router";
import NewItemsDropdown from "@/components/new-items-dropdown/NewItemsDropdown.vue";

const props = defineProps({
  id: String
})
const router = useRouter();

// 01. Instantiate ToastCallers
const fetchListCallToast = useCallToast();

// 01. Fetch blob and folder list
const folderId = computed(() => props.id);
const {
  data,
  isLoading,
  error: fetchError,
  isError: isFetchError
} = useListFiles(folderId);
// Initialize as empty arrays
const folders = computed(() =>
  data.value?.data?.filter(item => item.contentType === "directory") || []
);

const files = computed(() =>
  data.value?.data?.filter(item => item.contentType !== "directory") || []
);
// Force refetch when folder ID changes
/*watch(folderId, (newId, oldId) => {
  console.log(`Folder ID changed from ${oldId} to ${newId}`);
  if (newId !== oldId) {
    console.log("Forcing refetch due to folder ID change");
    refetch();
  }
}, { immediate: false });
// ... (rest of the code remains the same)

watch(() => props.id, (newId, oldId) => {
  console.log(`Route ID changed from ${oldId} to ${newId}`);
});

watch(() => data.value?.data, (newData) => {
  console.log("Data updated:", newData);
  console.log("Current folders:", folders.value);
  console.log("Current files:", files.value);
}, {deep: true});*/
/*
const listedItems = computed(() => data?.value?.data);
const folders = computed(() => listedItems.value?.filter(item => item.contentType === "directory"));
const files = computed(() => listedItems.value?.filter(item => item.contentType !== "directory"));
console.log(listedItems)
/!*const folders = computed(() => listedItems?.filter(item => item.contentType === "directory"));
const files = computed(() => listedItems?.filter(item => item.contentType !== "directory"));*!/
*/

/*const folders = ref();
const files = ref();
watch(
  () => data?.value?.data, // Watch the actual array inside `data.value`
  (newValue, oldValue) => {
    console.log('Old data:', oldValue); // Log the old data
    console.log('New data:', newValue); // Log the new data

    folders.value = newValue ? newValue.filter(item => item.contentType === "directory") : undefined;
    files.value = newValue ? newValue.filter(item => item.contentType !== "directory") : undefined;
  },
  {immediate: true}
);*/


// 05. React to fetch list errors
watchEffect(() => {
  if (isFetchError.value) {
    const errCode = fetchError?.value?.response?.status
    // Display toast error
    if (errCode != 404 && errCode != 400) {
      fetchListCallToast({
        httpCode: fetchError?.value?.response?.status ?? 500
      })
    }

    // Redirect to error pages
    if (errCode === 404) {
      router.push({path: '/not-found', replace: true})
    }

    if (errCode === 400) {
      router.push({path: '/bad-request', replace: true})
    }
  }
});


const chosenView = ref("grid");
const viewOptions = [
  {
    icon: "pi pi-list",
    value: "list"
  },
  {
    icon: "pi pi-th-large",
    value: "grid"
  }
];


</script>

<template>
  <MainLayout>
    <div class="flex flex-col">
      <!--  01. Top Section + Filter  -->
      <section class="!p-0 !m-0">
        <div class="flex justify-between">
          <NewItemsDropdown>
            <div
              class="flex items-center gap-2 hover:bg-[var(--p-surface-100)] w-[140px] p-1 pl-0 rounded-md cursor-pointer">
              <h1 class="font-medium text-2xl text-[var(--p-text-color)]">My Vault</h1>
              <svg width="12" height="12" viewBox="0 0 34 17" fill="none"
                   xmlns="http://www.w3.org/2000/svg">
                <path d="M17 17L0.545524 0.500004L33.4545 0.500004L17 17Z"
                      fill="var(--p-text-color)"/>
              </svg>
            </div>
          </NewItemsDropdown>

          <!--  List / Grid View Toggle        -->
          <SelectButton
            class="select-view"
            v-model="chosenView"
            :options="viewOptions"
            optionLabel="value"
            optionValue="value"
            aria-labelledby="custom"
          >
            <template #option="slotProps">
              <i :class="slotProps.option.icon"></i>
            </template>
          </SelectButton>
        </div>
      </section>

      <!--  02. Main Content  -->
      <section v-if="isLoading" class="flex justify-center items-center py-8">
        <i class="pi pi-spin pi-spinner text-2"></i>
      </section>

      <!--  03. Folders  -->
      <section class="flex flex-col gap-2" v-if="folders && folders.length > 0">
        <h3 class="font-normal text-sm">Folders</h3>
        <div class="grid grid-cols-[repeat(auto-fill,minmax(280px,1fr))] gap-2">
          <ListFolder
            v-for="(file, index) in folders"
            :key="index"
            :folder="file"
          />
        </div>
      </section>

      <!--  04. Files  -->
      <section class="flex flex-col gap-2" v-if="files && files.length > 0">
        <h3 class="font-normal text-sm">Files</h3>
        <div class="grid grid-cols-[repeat(auto-fill,minmax(280px,1fr))] gap-2">
          <ListFile
            v-for="(file, index) in files"
            :key="index"
            :file="file"
          />
        </div>
      </section>

      <!-- 05. EMPTY BLOB    -->
      <section v-if="
        !isLoading &&
        folders &&
        files &&
        folders.length === 0 &&
        files.length === 0"
               class="flex justify-center items-center py-8">
        <p>No files or folders found in your vault.</p>
      </section>
    </div>
  </MainLayout>
</template>

<style>
.select-view .p-togglebutton-checked .p-togglebutton-content {
  background-color: var(--p-primary-300) !important;
  border-radius: 4px !important;
}

.select-view {
  border-radius: 4px !important;
}
</style>

