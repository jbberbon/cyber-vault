import {useToast} from "primevue/usetoast";
import {capitalizeFirstLetter} from "@/lib/helpers/text-helper.ts";
import DELETE_FILE_ERR_MAP
  from "@/lib/constants/api-error-messages/file-error-messages.ts/delete-file-error-msg.ts";
import {COMMON_ERR, VAGUE_ERR} from "@/lib/constants/api-error-messages/common-error-messages.ts";

interface IBaseToast {
  httpCode?: number,
  severity?: "success" | "info" | "warn" | "error" | "secondary" | "contrast" | undefined,
  summary?: string
}

export const useCallToast = (errorMap?: Record<number, string>) => {
  const toast = useToast();
  return (params: IBaseToast) => {
    // Base the Toast on the httpCode
    if (params.httpCode) {
      const mappedSummary = errorMap ? errorMap[params.httpCode] : COMMON_ERR[params.httpCode];

      params.severity = "error";
      params.summary = mappedSummary ?? VAGUE_ERR;
      return toast.add({...params, life: 3000})
    }

    params.severity = params.severity ? params.severity : "success";
    params.summary = params.summary ? params.summary : capitalizeFirstLetter(params.severity)
    toast.add({...params, life: 3000})
  };
};
