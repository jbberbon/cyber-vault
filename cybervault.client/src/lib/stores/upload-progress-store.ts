// stores/uploadStore.ts (Pinia Store)
import {defineStore} from 'pinia';
import {ref} from 'vue';

interface IUploads {
  fileName: string,
  progress: number,
  isError: boolean
}

export const useUploadStore = defineStore('upload', () => {
  const uploads = ref<IUploads[]>([]);

 /* const uploads = ref<IUploads[]>([
    {
      fileName: "profile.jpeg",
      progress: 100
    },
    {
      fileName: "grad.pdf",
      progress: 50
    }
  ]);*/

  // Method to add a new upload
  const addUpload = (file: File) => {
    uploads.value.push({
      fileName: file.name,
      progress: 0, // start at 0%
      isError: false
    });
  };

  // Method to update the progress of an upload
  const updateProgress = (fileName: string, progress: number) => {
    const upload = uploads.value.find(upload => upload.fileName === fileName);
    if (upload) {
      upload.progress = progress;
    }
  };

  // Method to remove an upload (when it's done)
  const removeUpload = (fileName: string) => {
    uploads.value = uploads.value.filter(upload => upload.fileName !== fileName);
  };

  const reset = () => {
    uploads.value = []
  }

  return {
    uploads,
    addUpload,
    updateProgress,
    removeUpload,
    reset
  };
});
