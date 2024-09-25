export interface Config {
  providers: { name: string, url: string }[];
  contexts: { name: string; provider: string; workspace: string }[];
  defaultContext: string;
}

export interface ResolvedContext {
  name: string,
  providerName: string,
  providerUrl: URL,
  workspace: string
}

export interface AuthenticatedContext {
  session: AuthSession,
  accessToken: string,
  refreshToken: string | null,
}

export interface AuthSession {
  id: string,
  sub: string,
  email: string,
  userType: string,
  name: string,
  username: string,
  expires: Date,
  expired: boolean
}