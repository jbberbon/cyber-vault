interface IFile {
  serverAssignedId?: string,
  uri: string,
  name: string,
  contentType: string,
  content?: string
}

interface IFileDownloadOrDelete {
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

export type {IFile, IFileDownloadOrDelete, ICreateFolder, IUploadFile}
