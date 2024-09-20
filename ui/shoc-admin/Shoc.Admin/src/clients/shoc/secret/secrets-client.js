import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "secret" service
 */
export default class SecretsClient extends BaseAxiosClient {
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
      api: `api/workspaces/${workspaceId}/secrets`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  getAllExtended(token, workspaceId) {

    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/secrets/extended`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  getById(token, workspaceId, id) {

    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/secrets/${id}`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  create(token, workspaceId, input) {

    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/secrets`
    });

    return this.webClient.post(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  updateById(token, workspaceId, id, input) {

    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/secrets/${id}`
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  updateValueById(token, workspaceId, id, input) {

    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/secrets/${id}/value`
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  deleteById(token, workspaceId, id) {

    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/secrets/${id}`
    });

    return this.webClient.delete(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

}