import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "cluster" service
 */
export default class WorkspaceClustersClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-cluster", config);
  }

  getAll(token, workspaceId) {
    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/workspace-clusters`
    });
    
    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
}