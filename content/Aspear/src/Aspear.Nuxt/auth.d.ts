// auth.d.ts
declare module "#auth-utils" {
  interface User {
    sub: string;
    email: string;
    emailVerified: boolean;
    uuid: string;
    roles: string[];
    phoneNumber: string;
    firstName: string;
    lastName: string;
  }

  interface Tokens {
    accessToken: string;
    refreshToken: string;
  }

  interface UserSession {
    // Add your own fields
    tokens: Tokens;
  }
}

export { };
