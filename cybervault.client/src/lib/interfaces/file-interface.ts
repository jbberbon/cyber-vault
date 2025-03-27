interface IFile {
  serverAssignedId?: string,
  uri: string,
  name: string,
  contentType: string,
  content?: string
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

export type {IFile, IFileDelete, ICreateFolder, IUploadFile}
