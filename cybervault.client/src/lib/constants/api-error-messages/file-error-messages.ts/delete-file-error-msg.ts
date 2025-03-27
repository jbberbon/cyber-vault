import {COMMON_ERR} from "@/lib/constants/api-error-messages/common-error-messages.ts";

let DELETE_FILE_ERR_MAP: Record<string, string> = {
  ...COMMON_ERR,
  404: "File no longer exists."
}

export default DELETE_FILE_ERR_MAP;

