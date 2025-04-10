export const BROWSER_MEDIA_FILE_TYPES = [
  "jpg", "jpeg", "png", "gif", "bmp", "webp", "avif",
  "mp3", "wma", "aac", "flac", "wav",
  "mp4", "mkv", "avi", "mov", "wmv", "flv",
  "pdf"
]

export const GENERAL_TYPES: Record<string, string> = {
  "img": "img",
  "video": "video",
  "audio": "audio",
  "word": "word",
  "pdf": "pdf",
  "txt": "txt"
}

export const FILE_TYPE_TO_GENERAL_TYPE: Record<string, string> = {
  // Images
  svg: GENERAL_TYPES["img"],
  jpeg: GENERAL_TYPES["img"],
  jpg: GENERAL_TYPES["img"],

  // Videos
  mp4: GENERAL_TYPES["video"],
  mkv: GENERAL_TYPES["video"],
  mov: GENERAL_TYPES["video"],
  avi: GENERAL_TYPES["video"],

  // Audios
  mp3: GENERAL_TYPES["audio"],
  wav: GENERAL_TYPES["audio"],
  flac: GENERAL_TYPES["audio"],
  aac: GENERAL_TYPES["audio"],

  // Word Documents
  doc: GENERAL_TYPES["word"],
  docx: GENERAL_TYPES["word"],

  // PDFs
  pdf: GENERAL_TYPES["pdf"],

  // Texts
  txt: GENERAL_TYPES["txt"],
  text: GENERAL_TYPES["txt"],
  json: GENERAL_TYPES["txt"],
  env: GENERAL_TYPES["txt"]
}

