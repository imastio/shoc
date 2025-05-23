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

  getByName(token, workspaceId, name) {
    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/workspace-clusters/by-name/${name}`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  getPermissionsByName(token, workspaceId, name) {
    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/workspace-clusters/by-name/${name}/permissions`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  countAll(token, workspaceId) {
    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/workspace-clusters/count`
    });
    
    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  ping(token, workspaceId, input) {
    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/workspace-clusters/ping`
    });
    
    return this.webClient.post(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  create(token, workspaceId, input) {
    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/workspace-clusters`
    });
    
    return this.webClient.post(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  updateById(token, workspaceId, id, input) {
    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/workspace-clusters/${id}`
    });
    
    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  updateConfigurationById(token, workspaceId, id, input) {
    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/workspace-clusters/${id}/configuration`
    });
    
    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
}