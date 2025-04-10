export function splitFileNameAndExtension(str: string): { fileName: string, fileExtension: string } {
  const lastDotIndex = str.lastIndexOf('.');

  // If there is no dot, return the whole string as the name and an empty extension
  if (lastDotIndex === -1) {
    return { fileName: str, fileExtension: '' };
  }

  const fileName = str.slice(0, lastDotIndex);
  const fileExtension = str.slice(lastDotIndex + 1);

  return { fileName, fileExtension };
}


export const getHtmlVideoType = (file: string): string => {
  if(!file) {
    return "";
  }
  const {fileExtension} = splitFileNameAndExtension(file);

  // Map to HTML Video Source Type
  switch (fileExtension) {
    case 'mp4':
      return 'video/mp4';  // MP4 format
    case 'mkv':
      return 'video/x-matroska';  // MKV format
    case 'mov':
      return 'video/quicktime';  // MOV format
    case 'webm':
      return 'video/webm';  // WebM format
    case 'avi':
      return 'video/x-msvideo';  // AVI format
    case 'flv':
      return 'video/x-flv';  // FLV format
    case 'ogg':
      return 'video/ogg';  // OGG format
    case '3gp':
      return 'video/3gpp';  // 3GP format
    case 'wmv':
      return 'video/x-ms-wmv';  // WMV (Windows Media Video) format
    case 'mpeg':
    case 'mpg':
      return 'video/mpeg';  // MPEG format
    default:
      return '';  // Unknown format or unsupported
  }
}
