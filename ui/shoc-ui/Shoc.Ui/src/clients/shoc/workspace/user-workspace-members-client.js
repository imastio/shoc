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

 
  getAll(token, workspaceId){
    const url = this.urlify({
      api: `api/user-workspaces/${workspaceId}/members`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
}