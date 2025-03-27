interface IUser {
  firstName?: string,
  lastName?: string,
  email?: string,
}

interface ILoginUser {
  email: string,
  password: string
}

interface ILoginResponse {
  email: string
}

interface IRegisterUser {
  firstName?: string,
  lastName?: string,
  email: string,
  password: string
  confirmPassword?: string
}

export type {IUser, IRegisterUser, ILoginUser, ILoginResponse}

