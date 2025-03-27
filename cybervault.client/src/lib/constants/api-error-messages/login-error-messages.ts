import {COMMON_ERR} from "@/lib/constants/api-error-messages/common-error-messages.ts";

let LOGIN_ERR: Record<string, string> = {
  ...COMMON_ERR,
  429: "Your account is currently locked due to many failed attempts.",
  403: "Please confirm your account registration via your email."
}
LOGIN_ERR['401'] = "Invalid credentials."

export default LOGIN_ERR;

