<script setup lang="ts">
import MainLayout from "@/components/layouts/MainLayout.vue";
import {useListFiles} from "@/lib/services/files/use-file.ts";
import {computed, defineAsyncComponent, watch} from "vue";
import {useCallToast} from "@/lib/hooks/use-call-toast.ts";
import {useRouter} from "vue-router";
import ViewPreferenceToggle from "@/pages/protected/my-vault/components/ViewPreferenceToggle.vue";
import {useDataViewPreferenceStore} from "@/lib/stores/data-view-preference-store.ts";

const NewItemsDropdown = defineAsyncComponent(() => import('@/components/new-items-dropdown/NewItemsDropdown.vue'));
const MyVaultGrid = defineAsyncComponent(() => import('./components/view-preferences/MyVaultGrid.vue'));
const MyVaultList = defineAsyncComponent(() => import('./components/view-preferences/MyVaultList.vue'))

const props = defineProps({
  id: String
})
const router = useRouter();
const fetchListCallToast = useCallToast();

// 01. Fetch blob and folder list
const folderId = computed(() => props.id);
const listFilesQuery = useListFiles(folderId);

// 02. Filter Folders and Files from fetched data
const folders = computed(() =>
  listFilesQuery.data.value?.data?.filter(item => item.contentType === "directory") || []
);
const files = computed(() =>
  listFilesQuery.data.value?.data?.filter(item => item.contentType !== "directory") || []
);


// 03. React to fetch list errors
watch(
  () => listFilesQuery.isError.value, // The reactive value to watch
  (isError, prevIsError) => {
    if (isError) {
      const errCode = listFilesQuery.error?.value?.response?.status;

      // 03.01 Display toast error
      if (errCode != 404 && errCode != 400) {
        fetchListCallToast({
          httpCode: listFilesQuery.error?.value?.response?.status ?? 500
        });
      }

      // 03.02 Redirect to error pages
      if (errCode === 404) {
        router.push({path: '/not-found', replace: true});
      }
      if (errCode === 400) {
        router.push({path: '/bad-request', replace: true});
      }
    }
  }
);

const viewPreferenceStore = useDataViewPreferenceStore()

</script>

<template>
  <MainLayout>
    <div class="flex flex-col">
      <!--  01. Top Section + Filter  -->
      <section class="!p-0 !m-0">
        <div class="flex justify-between">
          <!--  01.01 My Vault Dropdown        -->
          <NewItemsDropdown>
            <div
              class="flex items-center gap-2 hover:bg-[var(--p-surface-100)] dark:hover:bg-[var(--p-surface-700)] w-[140px] p-1 pl-0 rounded-md cursor-pointer">
              <h1 class="font-medium text-2xl text-[var(--p-text-color)]">My Vault</h1>
              <svg width="12" height="12" viewBox="0 0 34 17" fill="none"
                   xmlns="http://www.w3.org/2000/svg">
                <path d="M17 17L0.545524 0.500004L33.4545 0.500004L17 17Z"
                      fill="var(--p-text-color)"/>
              </svg>
            </div>
          </NewItemsDropdown>

          <!--  01.02 List / Grid View Toggle        -->
          <ViewPreferenceToggle v-model:chosen-view="viewPreferenceStore.viewPreference"/>
        </div>
      </section>

      <!--  02. Skeleton Loading States  -->
      <section v-if="listFilesQuery.isLoading.value" class="flex justify-center items-center py-8">
        <i class="pi pi-spin pi-spinner text-2"></i>
      </section>

      <!-- 03. EMPTY BLOB    -->
      <section v-if="listFilesQuery.data.value?.data.length === 0"
               class="flex justify-center items-center py-8">
        <p class="text-center">No files or folders found in your vault.</p>
      </section>


      <!--  02. Main Content  -->
      <template v-else>
        <MyVaultGrid v-if="viewPreferenceStore.viewPreference === 'grid'" :folders="folders"
                     :files="files"/>
        <MyVaultList v-else :files="listFilesQuery.data.value?.data ?? []"/>
      </template>
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

