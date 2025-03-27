<script setup lang="ts">
import ListFile from "@/pages/protected/my-vault/components/file/ListFile.vue";
import ListFolder from "@/pages/protected/my-vault/components/folder/ListFolder.vue";
import type {IFile} from "@/lib/interfaces/file-interface.ts";

const props = defineProps<{
  folders: IFile[],
  files: IFile[]
}>();
</script>

<template>
  <!--  03. Folders  -->
  <section class="flex flex-col gap-2" v-if="props.folders && props.folders.length > 0">
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
    <h3 class="font-normal text-sm pt-4">Files</h3>
    <div class="grid grid-cols-[repeat(auto-fill,minmax(280px,1fr))] gap-2">
      <ListFile
        v-for="(file, index) in props.files"
        :key="index"
        :file="file"
      />
    </div>
  </section>
</template>
