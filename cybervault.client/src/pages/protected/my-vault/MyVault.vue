<script setup lang="ts">
import MainLayout from "@/components/layouts/MainLayout.vue";
import {useListFiles} from "@/lib/services/files/use-file.ts";
import {computed, defineAsyncComponent, watch, watchEffect} from "vue";
import {useCallToast} from "@/lib/hooks/use-call-toast.ts";
import {useRouter} from "vue-router";
import ViewPreferenceToggle from "@/pages/protected/my-vault/components/ViewPreferenceToggle.vue";
import {useDataViewPreferenceStore} from "@/lib/stores/data-view-preference-store.ts";
import CustomBreadcrumb from "@/pages/protected/my-vault/components/custom-breadcrumb/CustomBreadcrumb.vue";

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
  listFilesQuery.data.value?.data?.items.filter(item => item.contentType === "directory") || []
);
const files = computed(() =>
  listFilesQuery.data.value?.data?.items.filter(item => item.contentType !== "directory") || []
);

watchEffect(
  () =>
    console.log("fetched data", listFilesQuery.data.value?.data)
)


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
    <div class="flex flex-col w-full">
      <!--  01. Breadcrumb + Filter + View Toggle -->
      <section class="flex justify-between w-full">
        <!--  01.01 Breadcrumb  -->
        <CustomBreadcrumb :nav-items="listFilesQuery.data.value?.data?.directoryPathArray ?? []"/>

        <!--  01.02 List / Grid View Toggle        -->
        <ViewPreferenceToggle v-model:chosen-view="viewPreferenceStore.viewPreference"/>
      </section>

      <!--  02. Skeleton Loading States  -->
      <section v-if="listFilesQuery.isLoading.value" class="flex justify-center items-center py-8">
        <i class="pi pi-spin pi-spinner text-2"></i>
      </section>

      <!-- 03. EMPTY BLOB    -->
      <section v-if="listFilesQuery.data.value?.data.items.length === 0"
               class="flex justify-center items-center py-8">
        <p class="text-center">No files or folders found in your vault.</p>
      </section>


      <!--  02. Main Content  -->
      <template v-else>
        <MyVaultGrid v-if="viewPreferenceStore.viewPreference === 'grid'" :folders="folders"
                     :files="files"/>
        <MyVaultList v-else :files="listFilesQuery.data.value?.data.items ?? []"/>
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

