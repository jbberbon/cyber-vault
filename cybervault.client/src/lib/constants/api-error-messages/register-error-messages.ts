import {COMMON_ERR} from "@/lib/constants/api-error-messages/common-error-messages.ts";

let REGISTER_ERR: Record<string, string> = {
  ...COMMON_ERR,
  409: "An account with this email already exists.",
}

export default REGISTER_ERR;

