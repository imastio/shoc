import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "package" service
 */
export default class WorkspacePackagesClient extends BaseAxiosClient {

  constructor(config) {
    super("shoc-package", config);
  }

  fromCache(token, workspaceId, input) {
    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/packages/from-cache`
    });
    
    return this.webClient.post(url, input, {
      headers: {
        ...this.authBearer(token)
      },
      timeout: 2 * 60 * 60 * 1000
    });
  }
}