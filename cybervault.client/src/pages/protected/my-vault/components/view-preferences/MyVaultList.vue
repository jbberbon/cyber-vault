<script setup lang="ts">
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import type {IFile} from "@/lib/interfaces/file-interface.ts";
import {ICONS_MAP} from "@/lib/constants/icons-map.ts";
import FilePopover from "@/pages/protected/my-vault/components/file/FilePopover.vue";
import FolderPopover from "@/pages/protected/my-vault/components/folder/FolderPopover.vue";
import {useRouter} from "vue-router";
import {FILE_TYPE_TO_GENERAL_TYPE} from "@/lib/constants/file-type-map.ts";

const props = defineProps<{
  files: IFile[]
}>()

const router = useRouter()
const navigateFolder = (folder: IFile) => {
  if (folder.contentType === 'directory') {
    router.push({name: 'my-vault-subdirectory', params: {id: folder.serverAssignedId}})
  }
}
const onRowClick = (event: { data: IFile }) => {
  navigateFolder(event.data);
}

const chooseIcon = (file: IFile) => {
  // If folder, return folder icon
  if (file.contentType === 'directory') {
    return ICONS_MAP['folder'];
  }

  // Extract file type and map to general type
  const fileType = file.name.includes('.') ? file.name.split('.')[1] : "";
  const generalType = FILE_TYPE_TO_GENERAL_TYPE[fileType] ?? null;
  return ICONS_MAP[generalType] ?? ICONS_MAP['default'];
}
</script>

<template>
  <DataTable :value="props.files" tableStyle="min-width: 50rem; border-radius: 50px"
             class="my-vault-table" removableSort @row-dblclick="onRowClick">
    <Column field="name" header="Name" :sortable="true">
      <template #body="{ data }">
        <div class="flex gap-3 items-center">
          <i :class="chooseIcon(data)"></i>
          <span>{{ data.name }}</span>
        </div>
      </template>
    </Column>
    <Column field="lastModified" header="Last modified" :sortable="true">
      <template #body="{ data }">
        <span>{{ data?.lastModified ? data.lastModified : '-' }}</span>
      </template>
    </Column>
    <Column field="size" header="File size" :sortable="true">
      <template #body="{ data }">
        <span>{{ data?.size ? data.size : '-' }}</span>
      </template>
    </Column>
    <Column>
      <template #body="{ data }">
        <FolderPopover v-if="data.contentType === 'directory'" :folder="data" :class="''"/>
        <FilePopover v-else :file="data"/>
      </template>
    </Column>
  </DataTable>
</template>

<style>
.my-vault-table .p-datatable-header-cell {
  background-color: transparent;
}

.my-vault-table .p-datatable-tbody > tr {
  background-color: transparent;

}

.my-vault-table .p-datatable-tbody > tr:hover {
  background-color: var(--p-surface-100);
  cursor: pointer;
}

.dark .my-vault-table .p-datatable-tbody > tr:hover {
  background-color: var(--p-surface-900);
  cursor: pointer;
}

.my-vault-table .p-datatable-column-title {
  font-weight: normal;
}
</style>
