import {COMMON_ERR} from "@/lib/constants/api-error-messages/common-error-messages.ts";

let CREATE_FOLDER_ERR_MAP: Record<string, string> = {
  ...COMMON_ERR,
  409: "Folder name already exists."
}

export default CREATE_FOLDER_ERR_MAP;

