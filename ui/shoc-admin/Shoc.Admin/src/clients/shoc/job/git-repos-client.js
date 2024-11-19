import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "job" service
 */
export default class GitReposClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-job", config);
  }

  getAll(token, workspaceId) {

    const url = this.urlify({
      api: `api/management/workspaces/${workspaceId}/git-repos`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  getById(token, workspaceId, id) {

    const url = this.urlify({
      api: `api/management/workspaces/${workspaceId}/git-repos/${id}`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  create(token, workspaceId, input) {

    const url = this.urlify({
      api: `api/management/workspaces/${workspaceId}/git-repos`
    });

    return this.webClient.post(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  deleteById(token, workspaceId, id) {

    const url = this.urlify({
      api: `api/management/workspaces/${workspaceId}/git-repos/${id}`
    });

    return this.webClient.delete(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

}