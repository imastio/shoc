import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "cluster" service
 */
export default class WorkspaceClusterInstanceClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-cluster", config);
  }

  getConnectivityById(token, workspaceId, id) {
    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/workspace-clusters/${id}/instance/connectivity`
    });
    console.log(url)
    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
}