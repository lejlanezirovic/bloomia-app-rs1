export interface CurrentUserDto {
  userId?: number;
  therapistId?: number;
  nameIdentifier?: string;
  email?: string;
  fullname?: string;
  role: string;
  tokenVersion?: number;
 // isAdmin: boolean;
 // isClient: boolean;
  //isTherapist: boolean;
  //tokenVersion: number;

}
