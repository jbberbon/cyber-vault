interface IFile {
  serverAssignedId?: string,
  uri: string,
  name: string,
  contentType: string,
  content?: string
}

interface IDirectoryPath {
  serverAssignedId?: string,
  name?: string
}
interface IFileList {
  directoryPathArray: IDirectoryPath[],
  items: IFile[]
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

export type {IFile, IFileList, IDirectoryPath, IFileDownloadOrDelete, ICreateFolder, IUploadFile}
