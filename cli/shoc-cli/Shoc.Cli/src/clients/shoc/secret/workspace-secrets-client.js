import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "secret" service
 */
export default class WorkspaceSecretsClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-secret", config);
  }

  getAll(token, workspaceId) {
    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/workspace-secrets`
    });
    
    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  countAll(token, workspaceId) {
    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/workspace-secrets/count`
    });
    
    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  create(token, workspaceId, input) {
    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/workspace-secrets`
    });
    
    return this.webClient.post(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  updateById(token, workspaceId, id, input) {
    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/workspace-secrets/${id}`
    });
    
    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  updateValueById(token, workspaceId, id, input) {
    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/workspace-secrets/${id}/value`
    });
    
    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  deleteById(token, workspaceId, id) {
    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/workspace-secrets/${id}`
    });
    
    return this.webClient.delete(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
}