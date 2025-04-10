export const triggerDownload = (url?: string) => {
  if (!url) return

  const link = document.createElement('a');
  link.href = url;
  link.click();
}
