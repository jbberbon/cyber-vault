import {COMMON_ERR} from "@/lib/constants/api-error-messages/common-error-messages.ts";

let DELETE_FOLDER_ERR_MAP: Record<string, string> = {
  ...COMMON_ERR,
  404: "Folder no longer exists."
}

export default DELETE_FOLDER_ERR_MAP;

