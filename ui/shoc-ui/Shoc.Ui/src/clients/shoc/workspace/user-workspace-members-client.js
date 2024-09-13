import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "workspace" service
 */
export default class UserWorkspaceMembersClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-workspace", config);
  }


  getAll(token, workspaceId) {
    const url = this.urlify({
      api: `api/user-workspaces/${workspaceId}/members`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  updateById(token, workspaceId, id, input) {

    const url = this.urlify({
      api: `api/user-workspaces/${workspaceId}/members/${id}`
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  deleteById(token, workspaceId, id) {

    const url = this.urlify({
      api: `api/user-workspaces/${workspaceId}/members/${id}`
    });

    return this.webClient.delete(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
}