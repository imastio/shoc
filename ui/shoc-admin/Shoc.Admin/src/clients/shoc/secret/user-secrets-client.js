import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "secret" service
 */
export default class UserSecretsClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-secret", config);
  }

  getAll(token, workspaceId, userId) {

    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/users/${userId}/secrets`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  getAllExtended(token, workspaceId, userId) {

    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/users/${userId}/secrets/extended`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  getById(token, workspaceId, userId, id) {

    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/users/${userId}/secrets/${id}`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  create(token, workspaceId, userId, input) {

    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/users/${userId}/secrets`
    });

    return this.webClient.post(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  updateById(token, workspaceId, userId, id, input) {

    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/users/${userId}/secrets/${id}`
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  updateValueById(token, workspaceId, userId, id, input) {

    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/users/${userId}/secrets/${id}/value`
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  deleteById(token, workspaceId, userId, id) {

    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/users/${userId}/secrets/${id}`
    });

    return this.webClient.delete(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

}