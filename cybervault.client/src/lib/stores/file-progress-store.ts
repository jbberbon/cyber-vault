// stores/uploadStore.ts (Pinia Store)
import {defineStore} from 'pinia';
import {computed, ref} from 'vue';

interface IFileProgress {
  fileName: string,
  isUpload: boolean,
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

export const useFileProgressStore = defineStore('file-progress', () => {
  const files = ref<IFileProgress[]>([]);

  const pendingCount = computed(() => {
    return files.value.filter(file =>
      file.progress < 100 && // Pending progress
      !file.isDoneButAwaiting && // Processing in the backend
      !file.isError // Do not include errored uploads
    ).length;
  });

  // Method to add a new upload
  const addFile = (fileName: string, isUpload = true) => {
    files.value.push({
      fileName: fileName,
      isUpload,
      progress: 0, // start at 0%
      isDoneButAwaiting: false,
      isError: false
    });
  };

  // Method to update the progress of an upload
  const updateProgress = (fileName: string, progress?: number, isDoneButAwaiting?: boolean, isError?: boolean) => {
    const file = files.value.find(file => file.fileName === fileName);
    if (file) {
      if (progress !== undefined) file.progress = progress; // check for undefined explicitly
      if (isDoneButAwaiting !== undefined) file.isDoneButAwaiting = isDoneButAwaiting;
      if (isError !== undefined) file.isError = isError;
    }
  };

  // Method to remove an upload (when it's done)
  const removeUpload = (fileName: string) => {
    files.value = files.value.filter(file => file.fileName !== fileName);
  };


  const reset = () => {
    files.value = []
  }

  return {
    files,
    pendingCount,
    addFile,
    updateProgress,
    removeUpload,
    reset
  };
});
