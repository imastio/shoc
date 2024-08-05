import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "workspace" service
 */
export default class UserWorkspaceInvitationsClient extends BaseAxiosClient {
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
      api: `api/user-workspaces/${workspaceId}/invitations`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  create(token, workspaceId, input) {

    const url = this.urlify({
      api: `api/user-workspaces/${workspaceId}/invitations`
    });

    return this.webClient.post(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  updateById(token, workspaceId, id, input) {

    const url = this.urlify({
      api: `api/user-workspaces/${workspaceId}/invitations/${id}`
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  deleteById(token, workspaceId, id) {

    const url = this.urlify({
      api: `api/user-workspaces/${workspaceId}/invitations/${id}`
    });

    return this.webClient.delete(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
}