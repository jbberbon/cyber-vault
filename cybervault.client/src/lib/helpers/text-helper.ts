export const getTextBeforeOrAfterSubstring = (substring: string, beforeOrAfter: 1|0, url?: string) => {
  if(!url) {
    return "";
  }
  const regex = new RegExp(`${substring}\\/([^\\/]+)`);
  const match = url.match(regex);

  return match ? match[beforeOrAfter] : "";
}

export const capitalizeFirstLetter = (word: string): string => {
  if (!word) {
    return "";
  }
  return word.charAt(0).toUpperCase() + word.slice(1);
};
