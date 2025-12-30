// payload kako dolazi iz JWT-a
export interface JwtPayloadDto {
  sub: string;
  nameIdentifier?: string;
  // some backends use `email` or `emailAdress` (typoed), accept both
  email?: string;
  emailAdress?: string;
  fullname?: string;
  // backend returns a single role string like "CLIENT", "THERAPIST", "ADMIN"
  role?: string;
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'?: string;
  therapistId?: string;
  ver: string;
  iat: number;
  exp: number;
  aud: string;
  iss: string;
}
