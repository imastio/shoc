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