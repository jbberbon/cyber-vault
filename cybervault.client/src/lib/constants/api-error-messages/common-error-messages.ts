export const VAGUE_ERR = "An unexpected error occurred. Please try again later.";

export const COMMON_ERR: Record<string, string> = {
  500: "Internal server error. Try again later.",
  404: VAGUE_ERR,
  401: "You are currently unauthenticated. Please log in to continue.",
  400: "The request was invalid. Please check your input and try again."
};
