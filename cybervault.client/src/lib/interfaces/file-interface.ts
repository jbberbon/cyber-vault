interface IFile {
  serverAssignedId?: string,
  uri: string,
  name: string,
  contentType: string,
  content?: string
}

interface IFileSas {
  previewSas?: string,
  downloadSas?: string
}

interface IDirectory {
  serverAssignedId?: string,
  name?: string
}

interface IFileList {
  directoryPathArray: IDirectory[], // Sequence of directories from root to current directory
  items: IFile[]
}

interface IFileDownload {
  fileName: string,
  parentDirectoryId?: string,
  "downloadOrPreview.forPreview"?: boolean,
  "downloadOrPreview.forDownload"?: boolean,
}


interface IFileDelete {
  fileName: string,
  parentDirectoryId?: string
}

interface ICreateFolder {
  folderName: string,
  parentDirectoryId?: string
}

interface IUploadFile {
  file: File,
  parentFolderId?: string
}

export type {
  IFile,
  IFileSas,
  IFileList,
  IDirectory,
  IFileDownload,
  IFileDelete,
  ICreateFolder,
  IUploadFile
}
