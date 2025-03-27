// stores/uploadStore.ts (Pinia Store)
import {defineStore} from 'pinia';
import {computed, ref} from 'vue';

interface IUploads {
  fileName: string,
  progress: number,
  isDoneButAwaiting: boolean,
  isError: boolean
}
/*  const uploads = ref<IUploads[]>([
    {
      fileName: "profile.jpeg",
      progress: 100,
      isDoneButAwaiting: true,
      isError: false
    },
    {
      fileName: "grad.pdf",
      progress: 50,
      isDoneButAwaiting: false,
      isError: false
    }
  ]);*/

export const useUploadStore = defineStore('upload', () => {
    const uploads = ref<IUploads[]>([]);

  const pendingCount = computed(() => {
    return uploads.value.filter(upload =>
      upload.progress < 100 && // Pending progress
      !upload.isDoneButAwaiting && // Processing in the backend
      !upload.isError // Do not include errored uploads
    ).length;
  });

  // Method to add a new upload
  const addUpload = (file: File) => {
    uploads.value.push({
      fileName: file.name,
      progress: 0, // start at 0%
      isDoneButAwaiting: false,
      isError: false
    });
  };

  // Method to update the progress of an upload
  const updateProgress = (fileName: string, progress?: number, isDoneButAwaiting?: boolean, isError?: boolean) => {
    const upload = uploads.value.find(upload => upload.fileName === fileName);
    if (upload) {
      if (progress !== undefined) upload.progress = progress; // check for undefined explicitly
      if (isDoneButAwaiting !== undefined) upload.isDoneButAwaiting = isDoneButAwaiting;
      if (isError !== undefined) upload.isError = isError;
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
    pendingCount,
    addUpload,
    updateProgress,
    removeUpload,
    reset
  };
});
